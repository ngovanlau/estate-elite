
using RabbitMQ.Client;

namespace EventBus.RabbitMQ.Interfaces;

public interface IRabbitMQConnection : IAsyncDisposable
{
    bool IsConnected { get; }
    Task<bool> TryConnectAsync();
    Task<IChannel> CreateChannelAsync();
}
