using Polly;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQ.Client.Exceptions;
using System.Net.Sockets;

namespace EventBus.RabbitMQ;

using Interfaces;
using Serilog;

public class RabbitMQPersistentConnection(IConnectionFactory factory, ILogger logger, int retryCount = 5) : IRabbitMQPersistentConnection
{
    private readonly IConnectionFactory _factory = factory ?? throw new ArgumentNullException(nameof(factory));
    private readonly ILogger _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    private readonly int _retryCount = retryCount;
    private IConnection _connection = default!;
    private bool _disposed;
    private readonly SemaphoreSlim _connectionLock = new(1, 1);

    public bool IsConnected => _connection != null && _connection.IsOpen && !_disposed;

    public async Task<IChannel> CreateChannel()
    {
        if (!IsConnected)
        {
            await TryConnect().ConfigureAwait(false);
        }

        if (!IsConnected)
        {
            throw new InvalidOperationException("No RabbitMQ connections are available to perform this action");
        }

        return await _connection.CreateChannelAsync().ConfigureAwait(false);
    }

    public async Task<bool> TryConnect()
    {
        _logger.Information("RabbitMQ Client is trying to connect");

        try
        {
            await _connectionLock.WaitAsync().ConfigureAwait(false);

            if (IsConnected)
            {
                return true;
            }

            var policy = Policy.Handle<SocketException>()
                .Or<BrokerUnreachableException>()
                .WaitAndRetryAsync(_retryCount, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)),
                (ex, time) =>
                {
                    _logger.Warning(ex, "RabbitMQ Client could not connect after {TimeOut}s ({ExceptionMessage})", $"{time.TotalSeconds:n1}", ex.Message);
                });

            // Await the connection attempt
            _connection = await policy.ExecuteAsync(async () =>
                await _factory.CreateConnectionAsync().ConfigureAwait(false)).ConfigureAwait(false);

            if (IsConnected)
            {
                _connection.ConnectionShutdownAsync += OnConnectionShutdown;
                _connection.CallbackExceptionAsync += OnCallbackException;
                _connection.ConnectionBlockedAsync += OnConnectionBlocked;

                _logger.Information("RabbitMQ Client acquired a persistent connection to '{HostName}' and is subscribed to failure events", _connection.Endpoint.HostName);

                return true;
            }
            else
            {
                _logger.Fatal("FATAL ERROR: RabbitMQ connections could not be created and opened");
                return false;
            }
        }
        finally
        {
            _connectionLock.Release();
        }
    }

    private async Task OnConnectionShutdown(object sender, ShutdownEventArgs reason)
    {
        if (_disposed) return;

        _logger.Warning("A RabbitMQ connection is on shutdown. Trying to re-connect...");

        await TryConnect();
    }

    private async Task OnCallbackException(object sender, CallbackExceptionEventArgs e)
    {
        if (_disposed) return;

        _logger.Warning("A RabbitMQ connection threw an exception. Trying to re-connect...");

        await TryConnect();
    }

    private async Task OnConnectionBlocked(object sender, ConnectionBlockedEventArgs e)
    {
        if (_disposed) return;

        _logger.Warning("A RabbitMQ connection is blocked. Trying to re-connect...");

        await TryConnect();
    }

    public void Dispose()
    {
        if (_disposed) return;

        _disposed = true;

        try
        {
            _connection.Dispose();
        }
        catch (IOException ex)
        {
            _logger.Fatal("FATAL ERROR: RabbitMQ connection could not be disposed: {Message}", ex.Message);
        }
    }
}