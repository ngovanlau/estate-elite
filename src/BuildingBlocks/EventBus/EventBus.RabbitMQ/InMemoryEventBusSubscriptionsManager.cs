namespace EventBus.RabbitMQ;

using System.Collections.Concurrent;
using EventBus.Infrastructures;
using Infrastructures.Interfaces;

public class InMemoryEventBusSubscriptionsManager : IEventBusSubscriptionsManager
{
    private readonly ConcurrentDictionary<string, List<SubscriptionInfo>> _handlers;
    private readonly List<Type> _eventTypes;

    public event Func<string, Task>? EventRemovedAsync;

    public InMemoryEventBusSubscriptionsManager()
    {
        _handlers = new ConcurrentDictionary<string, List<SubscriptionInfo>>();
        _eventTypes = new List<Type>();
    }

    public bool IsEmpty => !_handlers.Keys.Any();

    public void Clear() => _handlers.Clear();

    public void AddSubscription<T, TH>() where T : IIntegrationEvent where TH : IIntegrationEventHandler<T>
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
            _handlers.GetOrAdd(eventName, _ => new List<SubscriptionInfo>());
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
        if (subsToRemove == null) return;

        if (_handlers.TryGetValue(eventName, out var handlers))
        {
            handlers.Remove(subsToRemove);
        }


        _handlers[eventName].Remove(subsToRemove);
        if (_handlers[eventName].Count == 0)
        {
            _handlers.TryRemove(eventName, out _);
            var eventType = _eventTypes.SingleOrDefault(e => e.Name == eventName);
            if (eventType != null)
            {
                _eventTypes.Remove(eventType);
            }
            RaiseOnEventRemoved(eventName);
        }
    }

    public IEnumerable<SubscriptionInfo> GetHandlersForEvent<T>() where T : IIntegrationEvent
    {
        var key = GetEventKey<T>();
        return GetHandlersForEvent(key);
    }

    public IEnumerable<SubscriptionInfo> GetHandlersForEvent(string eventName) =>
        HasSubscriptionsForEvent(eventName) ? _handlers[eventName] : Enumerable.Empty<SubscriptionInfo>();

    private void RaiseOnEventRemoved(string eventName)
    {
        var handler = EventRemovedAsync;
        handler?.Invoke(eventName);
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

    public bool HasSubscriptionsForEvent<T>() where T : IIntegrationEvent
    {
        var key = GetEventKey<T>();
        return HasSubscriptionsForEvent(key);
    }

    public bool HasSubscriptionsForEvent(string eventName) => _handlers.ContainsKey(eventName);

    public Type GetEventTypeByName(string eventName) =>
        _eventTypes.SingleOrDefault(t => t.Name == eventName) ?? throw new ArgumentException($"Event {eventName} not found", nameof(eventName));

    public string GetEventKey<T>() => typeof(T).Name;
}
