
using RabbitMQ.Client;

namespace EventBus.RabbitMQ.Interfaces;

public interface IRabbitMQPersistentConnection : IDisposable
{
    bool IsConnected { get; }
    Task<bool> TryConnect();
    Task<IChannel> CreateChannel();
}
