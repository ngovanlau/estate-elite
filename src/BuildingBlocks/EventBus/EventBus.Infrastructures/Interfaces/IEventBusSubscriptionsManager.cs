namespace EventBus.Abstraction.Interfaces;

public interface IEventBusSubscriptionsManager
{
    bool IsEmpty { get; }
    event Func<string, Task> EventRemovedAsync;
    void AddSubscription<T, TH>() where T : IEvent where TH : IEventHandler<T>;
    void RemoveSubscription<T, TH>() where T : IEvent where TH : IEventHandler<T>;
    bool HasSubscriptionsForEvent<T>() where T : IEvent;
    bool HasSubscriptionsForEvent(string eventName);
    Type GetEventTypeByName(string eventName);
    void Clear();
    IEnumerable<SubscriptionInfo> GetHandlersForEvent<T>() where T : IEvent;
    IEnumerable<SubscriptionInfo> GetHandlersForEvent(string eventName);
    string GetEventKey<T>();
}
