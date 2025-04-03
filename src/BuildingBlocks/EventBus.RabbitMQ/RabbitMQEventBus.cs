using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Serilog;
using System.Text;



namespace EventBus.RabbitMQ;

using EventBus.Interfaces;
using Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System.Text.Json;
using System.Threading.Tasks;

public class RabbitMQEventBus : IEventBus
{
    private readonly IRabbitMQPersistentConnection _connection;
    private readonly ILogger _logger;
    private readonly IEventBusSubscriptionsManager _manager;
    private readonly IServiceProvider _provider;
    private readonly int _retryCount;

    private IChannel _channel;
    private string _queueName;
    private const string BROKER_NAME = "EstateEliteEventBus";

    public RabbitMQEventBus(IRabbitMQPersistentConnection connection, ILogger logger, IServiceProvider provider, IEventBusSubscriptionsManager manager, string queueName = "", int retryCount = 5)
    {
        _connection = connection ?? throw new ArgumentNullException(nameof(connection));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _provider = provider ?? throw new ArgumentNullException(nameof(provider));
        _manager = manager ?? throw new ArgumentNullException(nameof(manager));
        _queueName = queueName;
        _retryCount = retryCount;
        _channel = CreateConsumerChanel();
    }

    private async Task<IChannel> CreateConsumerChanel()
    {
        if (!_connection.IsConnected)
        {
            await _connection.TryConnect();
        }

        _logger.Information("Creating RabbitMQ consumer channel");

        var channel = await _connection.CreateChannel();

        await channel.ExchangeDeclareAsync(exchange: BROKER_NAME, type: ExchangeType.Direct);

        await channel.QueueDeclareAsync(queue: _queueName, durable: true, exclusive: false, autoDelete: false, arguments: null);

        channel.CallbackExceptionAsync += async (sender, ea) =>
        {
            _logger.Warning(ea.Exception, "Recreating RabbitMQ consumer channel");

            _channel.Dispose();
            _channel = await CreateConsumerChanel();
            await StartBasicConsume();
        };

        return channel;
    }

    private async Task StartBasicConsume()
    {
        _logger.Information("Starting RabbitMQ basic consume");

        if (_channel != null)
        {
            var consumer = new AsyncEventingBasicConsumer(_channel);

            consumer.ReceivedAsync += ConsumerReceived;

            await _channel.BasicConsumeAsync(queue: _queueName, autoAck: false, consumer: consumer);
        }
        else
        {
            _logger.Error("StartBasicConsume can't call on _channel == null");
        }
    }

    private async Task ConsumerReceived(object sender, BasicDeliverEventArgs eventArgs)
    {
        var eventName = eventArgs.RoutingKey;
        var message = Encoding.UTF8.GetString(eventArgs.Body.Span);

        try
        {
            if (message.Contains("throw-fake-exception", StringComparison.InvariantCultureIgnoreCase))
            {
                throw new InvalidOperationException($"Fake exception requested: \"{message}\"");
            }

            await ProcessEvent(eventName, message);
        }
        catch (Exception ex)
        {
            _logger.Warning(ex, "Error Processing message \"{Message}\"", message);
        }

        // Even on exception we take the message off the queue.
        // in a REAL WORLD app this should be handled with a Dead Letter Exchange (DLX). 
        // For more information see: https://www.rabbitmq.com/dlx.html
        await _channel.BasicAckAsync(eventArgs.DeliveryTag, multiple: false);
    }

    private async Task ProcessEvent(string eventName, string message)
    {
        _logger.Information("Processing RabbitMQ event: {EventName}", eventName);

        if (_manager.HasSubscriptionsForEvent(eventName))
        {
            using var scope = _provider.CreateScope();

            var subscriptions = _manager.GetHandlersForEvent(eventName);

            foreach (var subscription in subscriptions)
            {
                var handler = scope.ServiceProvider.GetService(subscription.HandlerType);
                if (handler == null)
                {
                    _logger.Warning("Handler not found for event: {EventName}", eventName);
                    continue;
                }
                var eventType = _manager.GetEventTypeByName(eventName);
                var integrationEvent = JsonSerializer.Deserialize(message, eventType, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                if (integrationEvent == null)
                {
                    _logger.Warning("Integration event not found for event: {EventName}", eventName);
                    continue;
                }

                var concreteType = typeof(IIntegrationEventHandler<>).MakeGenericType(eventType);

                await Task.Yield();
                concreteType.GetMethod("Handle")?.Invoke(handler, [integrationEvent]);
            }
        }
        else
        {
            _logger.Warning("No subscriptions for event: {EventName}", eventName);
        }
    }

    public Task Publish(IIntegrationEvent @event)
    {
        throw new NotImplementedException();
    }

    public void Subscribe<T, TH>()
        where T : IIntegrationEvent
        where TH : IIntegrationEventHandler<T>
    {
        throw new NotImplementedException();
    }

    public void Unsubscribe<T, TH>()
        where T : IIntegrationEvent
        where TH : IIntegrationEventHandler<T>
    {
        throw new NotImplementedException();
    }

    public void Dispose()
    {
        throw new NotImplementedException();
    }
}
