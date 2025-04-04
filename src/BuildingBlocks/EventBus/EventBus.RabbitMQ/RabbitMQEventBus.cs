﻿using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Serilog;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using System.Text.Json;
using RabbitMQ.Client.Exceptions;
using System.Net.Sockets;
using Polly;

namespace EventBus.RabbitMQ;

using Infrastructures.Interfaces;
using Interfaces;

public class RabbitMQEventBus : IEventBus
{
    private readonly IRabbitMQConnection _connection;
    private readonly ILogger _logger;
    private readonly IEventBusSubscriptionsManager _manager;
    private readonly IServiceProvider _provider;
    private readonly int _retryCount;
    private IChannel _channel;
    private string _queueName;
    private const string BROKER_NAME = "EstateEliteEventBus";

    private RabbitMQEventBus(IRabbitMQConnection connection, ILogger logger, IServiceProvider provider, IEventBusSubscriptionsManager manager, string queueName = "", int retryCount = 5)
    {
        _connection = connection;
        _logger = logger;
        _manager = manager;
        _queueName = queueName;
        _provider = provider;
        _retryCount = retryCount;
        _channel = default!;
    }

    public static async Task<RabbitMQEventBus> CreateAsync(IRabbitMQConnection connection, ILogger logger, IServiceProvider provider, IEventBusSubscriptionsManager manager, string queueName = "", int retryCount = 5)
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

    public async Task PublishAsync(IIntegrationEvent @event)
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
                        _logger.Warning(ex, "Could not publish event: {EventId} after {Timeout}s ({ExceptionMessage})", @event.Id, $"{time.TotalSeconds:n1}", ex.Message);
                    });

        var eventName = @event.GetType().Name;

        _logger.Information("Creating RabbitMQ channel to publish event: {EventId} ({EventName})", @event.Id, eventName);

        using var channel = await _connection.CreateChannelAsync();

        var body = JsonSerializer.SerializeToUtf8Bytes(@event, @event.GetType(), new JsonSerializerOptions { WriteIndented = true });

        await policy.ExecuteAsync(async () =>
        {
            var properties = new BasicProperties
            {
                DeliveryMode = DeliveryModes.Persistent
            };

            _logger.Information("Publishing event to RabbitMQ: {EventId}", @event.Id);

            await channel.BasicPublishAsync(
                exchange: BROKER_NAME,
                routingKey: eventName,
                mandatory: true,
                basicProperties: properties,
                body: body
            );
        });
    }

    public async Task Subscribe<T, TH>() where T : IIntegrationEvent where TH : IIntegrationEventHandler<T>
    {
        var eventName = _manager.GetEventKey<T>();
        await DoInternalSubscription(eventName);

        _logger.Information("Subscribing to event {EventName} with {EventHandler}", eventName, typeof(TH).GetType().Name);

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

    public void Unsubscribe<T, TH>() where T : IIntegrationEvent where TH : IIntegrationEventHandler<T>
    {
        var eventName = _manager.GetEventKey<T>();

        _logger.Information("Unsubscribing from event {EventName}", eventName);

        _manager.RemoveSubscription<T, TH>();
    }

    public async ValueTask DisposeAsync()
    {
        if (_channel != null)
        {
            await _channel.DisposeAsync();
        }

        _manager.Clear();
    }

    private async Task<IChannel> CreateConsumerChannelAsync()
    {
        if (!_connection.IsConnected)
        {
            await _connection.TryConnectAsync();
        }

        _logger.Information("Creating RabbitMQ consumer channel");

        var channel = await _connection.CreateChannelAsync();

        await channel.ExchangeDeclareAsync(exchange: BROKER_NAME, type: ExchangeType.Direct);

        await channel.QueueDeclareAsync(queue: _queueName, durable: true, exclusive: false, autoDelete: false, arguments: null);

        channel.CallbackExceptionAsync += async (sender, ea) =>
        {
            _logger.Warning(ea.Exception, "Recreating RabbitMQ consumer channel");

            await _channel.DisposeAsync();
            _channel = await CreateConsumerChannelAsync();
            await StartBasicConsumeAsync();
        };

        return channel;
    }

    private async Task StartBasicConsumeAsync()
    {
        _logger.Information("Starting RabbitMQ basic consume");

        if (_channel != null)
        {
            var consumer = new AsyncEventingBasicConsumer(_channel);

            consumer.ReceivedAsync += OnReceivedAsync;

            await _channel.BasicConsumeAsync(queue: _queueName, autoAck: false, consumer: consumer);
        }
        else
        {
            _logger.Error("StartBasicConsume can't call on _channel == null");
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

    private async Task ProcessEventAsync(string eventName, string message)
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
}
