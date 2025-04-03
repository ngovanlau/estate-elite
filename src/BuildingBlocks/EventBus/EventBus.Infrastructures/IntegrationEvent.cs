namespace EventBus.Infrastructures;

using Interfaces;

public class IntegrationEvent : IIntegrationEvent
{
    public Guid Id { get; private set; }
    public DateTime CreatedOn { get; private set; }

    protected IntegrationEvent()
    {
        Id = Guid.NewGuid();
        CreatedOn = DateTime.UtcNow;
    }
}
