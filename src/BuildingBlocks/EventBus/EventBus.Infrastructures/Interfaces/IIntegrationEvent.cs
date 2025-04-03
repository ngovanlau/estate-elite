namespace EventBus.Infrastructures.Interfaces;
public interface IIntegrationEvent
{
    public Guid Id { get; }
    public DateTime CreatedOn { get; }
}
