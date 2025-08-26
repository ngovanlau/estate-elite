namespace EventBus.Abstraction.Interfaces;

public interface IEvent
{
    Guid Id { get; }
    DateTime CreatedOn { get; }
}
