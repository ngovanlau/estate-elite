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