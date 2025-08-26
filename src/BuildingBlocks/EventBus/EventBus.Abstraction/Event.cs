using EventBus.Abstraction.Interfaces;

namespace EventBus.Abstraction;

public class Event : IEvent
{
    public Guid Id { get; private set; }
    public DateTime CreatedOn { get; private set; }

    protected Event()
    {
        Id = Guid.NewGuid();
        CreatedOn = DateTime.UtcNow;
    }
}
