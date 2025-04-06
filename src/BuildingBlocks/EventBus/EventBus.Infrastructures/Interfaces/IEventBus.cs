namespace EventBus.Infrastructures.Interfaces;

public interface IEventBus : IAsyncDisposable
{
    Task PublishAsync(IEvent @event);
    Task Subscribe<T, TH>() where T : IEvent where TH : IEventHandler<T>;
    void Unsubscribe<T, TH>() where T : IEvent where TH : IEventHandler<T>;
}
