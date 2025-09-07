/*
 * Why define IDomainEvent (with MediatR.INotification):
 * -----------------------------------------------------
 * 1. Domain-Driven Design (DDD) principle:
 *    - Domain events represent something important that happened in the domain
 *      (e.g., "OrderPlaced", "UserRegistered").
 *    - They allow the domain model to express side effects without directly invoking services.
 *
 * 2. Decoupling via MediatR:
 *    - IDomainEvent extends INotification, making it compatible with MediatR’s
 *      publish/subscribe mechanism.
 *    - Handlers can react to events asynchronously without the entity knowing about them.
 *
 * 3. Event Metadata:
 *    - `Id` ensures each event is uniquely identifiable.
 *    - `OccurredOn` records when the event happened (in UTC), which is useful for
 *      logging, auditing, replaying events, or debugging.
 *
 * 4. Testability:
 *    - Events can be raised and verified in unit tests without involving infrastructure.
 *
 * 5. Clean separation of concerns:
 *    - Entities only raise events.
 *    - Application layer or infrastructure handles what to do with them
 *      (e.g., sending emails, updating read models, publishing to message bus).
 *
 * Overall:
 * IDomainEvent provides a consistent, lightweight contract for capturing and
 * dispatching domain events across the system while staying aligned with DDD
 * and Clean Architecture principles.
 */

using MediatR;

namespace Core.Domain.Abstractions;

/// <summary>
/// Interface for domain events
/// </summary>
public interface IDomainEvent : INotification
{
    /// <summary>
    /// Unique identifier for the event
    /// </summary>
    Guid Id { get; }

    /// <summary>
    /// Timestamp when the event occurred (UTC)
    /// </summary>
    DateTime OccurredOn { get; }
}
