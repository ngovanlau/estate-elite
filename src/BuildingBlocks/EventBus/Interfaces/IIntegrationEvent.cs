using System;

namespace EventBus.Interfaces;

public interface IIntegrationEvent
{
    public Guid Id { get; }
    public DateTime CreatedOn { get; }
}
