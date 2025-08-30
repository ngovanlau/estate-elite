/*
 * Why define DomainEvent this way:
 * --------------------------------
 * 1. Base Class for Events:
 *    - Provides common metadata for all domain events.
 *    - Ensures every event has a unique Id and a timestamp (OccurredOn).
 *
 * 2. Event Identity:
 *    - `Id` uniquely identifies each event (useful for tracing, deduplication, logging).
 *    - Automatically generated with Guid.NewGuid() in the default constructor.
 *
 * 3. Timestamp:
 *    - `OccurredOn` records when the event happened (in UTC).
 *    - Useful for debugging, auditing, replaying events, or event sourcing.
 *
 * 4. Constructors:
 *    - Default constructor sets Id and OccurredOn automatically.
 *    - Alternate constructor allows explicit values (useful when replaying events from storage).
 *
 * 5. Extensibility:
 *    - Concrete domain events (e.g., UserRegisteredEvent, OrderPlacedEvent)
 *      inherit from DomainEvent and add domain-specific properties.
 *
 * Overall:
 * DomainEvent provides a consistent foundation for all domain events,
 * ensuring traceability, uniqueness, and alignment with DDD best practices.
 */

namespace Core.Domain.Primitives;

/// <summary>
/// Base class for domain events.
/// </summary>
public abstract class DomainEvent
{
    /// <inheritdoc />
    public Guid Id { get; init; }

    /// <inheritdoc />
    public DateTime OccurredOn { get; init; }

    /// <summary>
    /// Default constructor that sets ID and OccurredOn.
    /// </summary>
    protected DomainEvent()
    {
        Id = Guid.NewGuid();
        OccurredOn = DateTime.UtcNow;
    }

    /// <summary>
    /// Constructor with explicit ID and OccurredOn.
    /// </summary>
    /// <param name="id">Event identifier</param>
    /// <param name="occurredOn">Event occurrence date and time</param>
    protected DomainEvent(Guid id, DateTime occurredOn)
    {
        Id = id;
        OccurredOn = occurredOn;
    }
}
