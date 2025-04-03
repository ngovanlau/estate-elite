namespace EventBus.Infrastructures.Interfaces;

public interface IEventBus : IAsyncDisposable
{
    Task PublishAsync(IIntegrationEvent @event);
    Task Subscribe<T, TH>() where T : IIntegrationEvent where TH : IIntegrationEventHandler<T>;
    void Unsubscribe<T, TH>() where T : IIntegrationEvent where TH : IIntegrationEventHandler<T>;
}
