using EventBus.Interfaces;

namespace EventBus.RabbitMQ;

public class InMemoryEventBusSubscriptionsManager : IEventBusSubscriptionsManager
{
    private readonly Dictionary<string, List<SubscriptionInfo>> _handlers;
    private readonly List<Type> _eventTypes;

    public event EventHandler<string>? OnEventRemoved;

    public InMemoryEventBusSubscriptionsManager()
    {
        _handlers = [];
        _eventTypes = [];
    }

    public bool IsEmpty => !_handlers.Keys.Any();

    public void Clear() => _handlers.Clear();

    public void AddSubscription<T, TH>()
        where T : IIntegrationEvent
        where TH : IIntegrationEventHandler<T>
    {
        var eventName = GetEventKey<T>();

        AddSubscription(typeof(TH), eventName);

        if (!_eventTypes.Contains(typeof(T)))
        {
            _eventTypes.Add(typeof(T));
        }
    }

    private void AddSubscription(Type handlerType, string eventName)
    {
        if (!HasSubscriptionsForEvent(eventName))
        {
            _handlers.Add(eventName, []);
        }

        if (_handlers[eventName].Any(s => s.HandlerType == handlerType))
        {
            throw new ArgumentException($"Handler Type {handlerType.Name} already registered for '{eventName}'", nameof(handlerType));
        }

        _handlers[eventName].Add(SubscriptionInfo.Typed(handlerType));
    }

    public void RemoveSubscription<T, TH>()
        where T : IIntegrationEvent
        where TH : IIntegrationEventHandler<T>
    {
        var handlerToRemove = FindSubscriptionToRemove<T, TH>();
        var eventName = GetEventKey<T>();
        RemoveHandler(eventName, handlerToRemove);
    }

    private void RemoveHandler(string eventName, SubscriptionInfo? subsToRemove)
    {
        if (subsToRemove != null)
        {
            _handlers[eventName].Remove(subsToRemove);
            if (_handlers[eventName].Count == 0)
            {
                _handlers.Remove(eventName);

                var eventType = _eventTypes.SingleOrDefault(e => e.Name == eventName);

                if (eventType != null)
                {
                    _eventTypes.Remove(eventType);
                }
                OnEventRemoved?.Invoke(this, eventName);
            }
        }
    }

    private SubscriptionInfo? FindSubscriptionToRemove<T, TH>()
        where T : IIntegrationEvent
        where TH : IIntegrationEventHandler<T>
    {
        var eventName = GetEventKey<T>();
        return FindSubscriptionToRemove<T, TH>(eventName, typeof(TH));
    }

    private SubscriptionInfo? FindSubscriptionToRemove<T, TH>(string eventName, Type handlerType)
    {
        if (!HasSubscriptionsForEvent(eventName))
        {
            return null;
        }

        return _handlers[eventName].SingleOrDefault(s => s.HandlerType == handlerType);
    }

    public IEnumerable<SubscriptionInfo> GetHandlersForEvent<T>() where T : IIntegrationEvent
    {
        var key = GetEventKey<T>();
        return GetHandlersForEvent(key);
    }

    public IEnumerable<SubscriptionInfo> GetHandlersForEvent(string eventName) =>
        HasSubscriptionsForEvent(eventName) ? _handlers[eventName] : Enumerable.Empty<SubscriptionInfo>();

    public string GetEventKey<T>() => typeof(T).Name;

    public Type GetEventTypeByName(string eventName) =>
        _eventTypes.SingleOrDefault(t => t.Name == eventName) ?? throw new ArgumentException($"Event {eventName} not found", nameof(eventName));

    public bool HasSubscriptionsForEvent<T>() where T : IIntegrationEvent
    {
        var key = GetEventKey<T>();
        return HasSubscriptionsForEvent(key);
    }

    public bool HasSubscriptionsForEvent(string eventName) => _handlers.ContainsKey(eventName);
}

