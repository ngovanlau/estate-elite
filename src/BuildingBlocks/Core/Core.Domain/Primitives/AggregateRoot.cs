/*
 * Why define AggregateRoot this way:
 * ----------------------------------
 * 1. Aggregate Root in DDD:
 *    - Represents the entry point to an aggregate (a cluster of entities/values).
 *    - Enforces business invariants and ensures consistency inside the aggregate.
 *    - Only the aggregate root can be referenced from outside, not inner entities.
 *
 * 2. Inherits from Entity:
 *    - AggregateRoot is still an entity (has identity).
 *    - Adds extra behaviors specific to aggregates, such as domain event handling.
 *
 * 3. Domain Events Handling:
 *    - Maintains a private list of IDomainEvent.
 *    - Provides controlled methods:
 *        - RaiseDomainEvent(...) → records new events.
 *        - ClearDomainEvents()  → resets after publishing.
 *        - HasDomainEvents      → quick check.
 *        - PopDomainEvents()    → retrieves and clears at once.
 *    - Marked with [NotMapped] to ensure EF Core does not persist domain events.
 *
 * 4. Encapsulation:
 *    - The _domainEvents list is private and exposed only as IReadOnlyCollection.
 *    - Prevents external code from mutating domain events directly.
 *
 * 5. Constructors:
 *    - Protected default constructor for EF Core materialization.
 *    - Protected constructor with explicit Guid id for explicit initialization.
 *
 * Overall:
 * This design makes AggregateRoot the central authority for:
 * - Maintaining invariants within aggregates.
 * - Raising and tracking domain events in a consistent way.
 * - Integrating cleanly with EF Core and MediatR for event dispatching.
 */

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