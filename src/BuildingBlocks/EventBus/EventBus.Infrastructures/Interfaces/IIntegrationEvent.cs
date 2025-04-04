namespace EventBus.Infrastructures.Interfaces;
public interface IIntegrationEvent
{
    Guid Id { get; }
    DateTime CreatedOn { get; }
}
