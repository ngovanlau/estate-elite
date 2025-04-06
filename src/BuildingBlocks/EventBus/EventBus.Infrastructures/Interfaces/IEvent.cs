namespace EventBus.Infrastructures.Interfaces;
public interface IEvent
{
    Guid Id { get; }
    DateTime CreatedOn { get; }
}
