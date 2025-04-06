namespace EventBus.Infrastructures.Interfaces;

public interface IEventHandler
{
}

public interface IEventHandler<in TEvent> : IEventHandler where TEvent : IEvent
{
    Task Handle(TEvent @event);
}
