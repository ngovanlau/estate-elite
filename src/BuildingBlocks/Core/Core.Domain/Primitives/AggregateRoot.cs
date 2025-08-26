using System.ComponentModel.DataAnnotations.Schema;
using Core.Domain.Abstractions;

namespace Core.Domain.Primitives;

public abstract class AggregateRoot : Entity
{
    private readonly List<IDomainEvent> _domainEvents = [];

    /// <summary>
    /// Protected constructor for EF Core
    /// </summary>
    protected AggregateRoot()
    {
    }

    /// <summary>
    /// Protected constructor with explicit ID
    /// </summary>
    /// <param name="id">Entity identifier</param>
    protected AggregateRoot(Guid id) : base(id)
    {
    }

    /// <summary>
    /// Domain events raised by this aggregate
    /// </summary>
    [NotMapped]
    public IReadOnlyCollection<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();

    /// <summary>
    /// Raises a domain event
    /// </summary>
    /// <param name="domainEvent">Domain event to raise</param>
    protected void RaiseDomainEvent(IDomainEvent domainEvent)
    {
        ArgumentNullException.ThrowIfNull(domainEvent);
        _domainEvents.Add(domainEvent);
    }

    /// <summary>
    /// Clears all domain events
    /// </summary>
    public void ClearDomainEvents()
    {
        _domainEvents.Clear();
    }

    /// <summary>
    /// Checks if there are any domain events
    /// </summary>
    /// <returns>True if has pending events</returns>
    public bool HasDomainEvents => _domainEvents.Count > 0;

    /// <summary>
    /// Gets and clears all domain events
    /// </summary>
    /// <returns>List of domain events</returns>
    public IReadOnlyCollection<IDomainEvent> PopDomainEvents()
    {
        var events = _domainEvents.ToList().AsReadOnly();
        _domainEvents.Clear();
        return events;
    }
}