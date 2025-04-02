
namespace EventBus.Interfaces;

public interface IEventBus
{
    Task Publish(IIntegrationEvent @event);
    void Subscribe<T, TH>() where T : IIntegrationEvent where TH : IIntegrationEventHandler<T>;
    void Unsubscribe<T, TH>() where T : IIntegrationEvent where TH : IIntegrationEventHandler<T>;
}
