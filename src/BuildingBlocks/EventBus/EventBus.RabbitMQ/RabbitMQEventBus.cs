using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using EventBus.Abstraction.Interfaces;
using EventBus.RabbitMQ.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Polly;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQ.Client.Exceptions;

namespace EventBus.RabbitMQ;

public class RabbitMQEventBus : IEventBus
{
    private readonly IRabbitMQConnection _connection;
    private readonly ILogger<RabbitMQEventBus> _logger;
    private readonly IEventBusSubscriptionsManager _manager;
    private readonly IServiceProvider _provider;
    private readonly int _retryCount;
    private IChannel _channel;
    private string _queueName;
    private const string BROKER_NAME = "EstateEliteEventBus";

    private RabbitMQEventBus(IRabbitMQConnection connection, ILogger<RabbitMQEventBus> logger, IServiceProvider provider, IEventBusSubscriptionsManager manager, string queueName = "", int retryCount = 5)
    {
        _connection = connection;
        _logger = logger;
        _manager = manager;
        _queueName = queueName;
        _provider = provider;
        _retryCount = retryCount;
        _channel = default!;
        _manager.EventRemovedAsync += OnEventRemovedAsync;
    }

    public static async Task<RabbitMQEventBus> CreateAsync(IRabbitMQConnection connection, ILogger<RabbitMQEventBus> logger, IServiceProvider provider, IEventBusSubscriptionsManager manager, string queueName = "", int retryCount = 5)
    {
        if (string.IsNullOrEmpty(queueName))
        {
            throw new ArgumentException("Queue name cannot be null or empty", nameof(queueName));
        }

        var eventBus = new RabbitMQEventBus(connection, logger, provider, manager, queueName, retryCount);
        await eventBus.InitializeAsync();

        return eventBus;
    }

    private async Task InitializeAsync()
    {
        _channel = await CreateConsumerChannelAsync();
        await StartBasicConsumeAsync();
    }

    private async Task OnEventRemovedAsync(string eventName)
    {
        if (!_connection.IsConnected)
        {
            await _connection.TryConnectAsync();
        }

        using var channel = await _connection.CreateChannelAsync();
        await channel.QueueUnbindAsync(queue: _queueName, exchange: BROKER_NAME, routingKey: eventName);

        if (_manager.IsEmpty)
        {
            _queueName = string.Empty;
            await _channel.CloseAsync();
        }
    }

    public async Task PublishAsync(IEvent @event)
    {
        if (!_connection.IsConnected)
        {
            await _connection.TryConnectAsync();
        }

        var policy = Policy.Handle<BrokerUnreachableException>()
            .Or<SocketException>()
            .WaitAndRetryAsync(_retryCount, retryAttempt =>
                TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)), (ex, time) =>
                    {
                        _logger.LogWarning(ex, "Could not publish event: {EventId} after {Timeout}s ({ExceptionMessage})", @event.Id, $"{time.TotalSeconds:n1}", ex.Message);
                    });

        var eventName = @event.GetType().Name;

        _logger.LogInformation("Creating RabbitMQ channel to publish event: {EventId} ({EventName})", @event.Id, eventName);

        using var channel = await _connection.CreateChannelAsync();

        var body = JsonSerializer.SerializeToUtf8Bytes(@event, @event.GetType(), new JsonSerializerOptions { WriteIndented = true });

        await policy.ExecuteAsync(async () =>
        {
            var properties = new BasicProperties
            {
                DeliveryMode = DeliveryModes.Persistent
            };

            _logger.LogInformation("Publishing event to RabbitMQ: {EventId}", @event.Id);

            await channel.BasicPublishAsync(
                exchange: BROKER_NAME,
                routingKey: eventName,
                mandatory: true,
                basicProperties: properties,
                body: body
            );
        });
    }

    public async Task Subscribe<T, TH>() where T : IEvent where TH : IEventHandler<T>
    {
        var eventName = _manager.GetEventKey<T>();
        await DoInternalSubscription(eventName);

        _logger.LogInformation("Subscribing to event {EventName} with {EventHandler}", eventName, typeof(TH).GetType().Name);

        _manager.AddSubscription<T, TH>();
        await StartBasicConsumeAsync();
    }

    private async Task DoInternalSubscription(string eventName)
    {
        var containsKey = _manager.HasSubscriptionsForEvent(eventName);
        if (!containsKey)
        {
            if (!_connection.IsConnected)
            {
                await _connection.TryConnectAsync();
            }

            using var channel = await _connection.CreateChannelAsync();
            await channel.QueueBindAsync(queue: _queueName, exchange: BROKER_NAME, routingKey: eventName);
        }
    }

    public void Unsubscribe<T, TH>() where T : IEvent where TH : IEventHandler<T>
    {
        var eventName = _manager.GetEventKey<T>();

        _logger.LogInformation("Unsubscribing from event {EventName}", eventName);

        _manager.RemoveSubscription<T, TH>();
    }

    public async ValueTask DisposeAsync()
    {
        if (_channel != null)
        {
            await _channel.DisposeAsync();
        }

        if (_connection != null)
        {
            await _connection.DisposeAsync();
        }

        _manager.Clear();

        GC.SuppressFinalize(this);
    }

    private async Task<IChannel> CreateConsumerChannelAsync()
    {
        if (!_connection.IsConnected)
        {
            await _connection.TryConnectAsync();
        }

        _logger.LogInformation("Creating RabbitMQ consumer channel");

        var channel = await _connection.CreateChannelAsync();

        await channel.ExchangeDeclareAsync(exchange: BROKER_NAME, type: ExchangeType.Direct);

        await channel.QueueDeclareAsync(queue: _queueName, durable: true, exclusive: false, autoDelete: false, arguments: null);

        channel.CallbackExceptionAsync += async (sender, ea) =>
        {
            _logger.LogWarning(ea.Exception, "Recreating RabbitMQ consumer channel");

            await _channel.DisposeAsync();
            _channel = await CreateConsumerChannelAsync();
            await StartBasicConsumeAsync();
        };

        return channel;
    }

    private async Task StartBasicConsumeAsync()
    {
        _logger.LogInformation("Starting RabbitMQ basic consume");

        if (_channel != null)
        {
            var consumer = new AsyncEventingBasicConsumer(_channel);

            consumer.ReceivedAsync += OnReceivedAsync;

            await _channel.BasicConsumeAsync(queue: _queueName, autoAck: false, consumer: consumer);
        }
        else
        {
            _logger.LogError("StartBasicConsume can't call on _channel == null");
        }
    }

    private async Task OnReceivedAsync(object sender, BasicDeliverEventArgs eventArgs)
    {
        var eventName = eventArgs.RoutingKey;
        var message = Encoding.UTF8.GetString(eventArgs.Body.Span);

        try
        {
            if (message.Contains("throw-fake-exception", StringComparison.InvariantCultureIgnoreCase))
            {
                throw new InvalidOperationException($"Fake exception requested: \"{message}\"");
            }

            await ProcessEventAsync(eventName, message);

            await _channel.BasicAckAsync(eventArgs.DeliveryTag, multiple: false);
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Error Processing message \"{Message}\"", message);

            // Choose a clear error handling strategy
            // Option 1: Requeue for later retry
            await _channel.BasicNackAsync(eventArgs.DeliveryTag, false, true);

            // Option 2: Send to Dead Letter Exchange if configured
            // await _channel.BasicNackAsync(eventArgs.DeliveryTag, false, false);

            // Option 3: Acknowledge and log error (removes message)
            // await _channel.BasicAckAsync(eventArgs.DeliveryTag, multiple: false);
        }
    }

    private async Task ProcessEventAsync(string eventName, string message)
    {
        _logger.LogInformation("Processing RabbitMQ event: {EventName}", eventName);

        if (_manager.HasSubscriptionsForEvent(eventName))
        {
            using var scope = _provider.CreateScope();

            var subscriptions = _manager.GetHandlersForEvent(eventName);

            foreach (var subscription in subscriptions)
            {
                var handler = scope.ServiceProvider.GetService(subscription.HandlerType);
                if (handler == null)
                {
                    _logger.LogWarning("Handler not found for event: {EventName}", eventName);
                    continue;
                }
                var eventType = _manager.GetEventTypeByName(eventName);
                var integrationEvent = JsonSerializer.Deserialize(message, eventType, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                if (integrationEvent == null)
                {
                    _logger.LogWarning("Integration event not found for event: {EventName}", eventName);
                    continue;
                }

                var concreteType = typeof(IEventHandler<>).MakeGenericType(eventType);

                await Task.Yield();
                concreteType.GetMethod("Handle")?.Invoke(handler, [integrationEvent]);
            }
        }
        else
        {
            _logger.LogWarning("No subscriptions for event: {EventName}", eventName);
        }
    }
}
