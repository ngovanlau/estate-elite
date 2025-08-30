/*
 * Why define IUnitOfWork:
 * -----------------------
 * 1. Unit of Work Pattern:
 *    - Ensures that multiple operations against the data store are committed
 *      as a single transaction.
 *    - Tracks changes to multiple entities and saves them atomically.
 *
 * 2. Abstraction:
 *    - IUnitOfWork decouples application layer from concrete DbContext or ORM.
 *    - Application services depend on this interface rather than EF Core directly.
 *
 * 3. SaveChangesAsync:
 *    - Central method to persist all changes.
 *    - Supports asynchronous operations with CancellationToken.
 *
 * 4. Benefits:
 *    - Maintains consistency when working with multiple repositories/entities.
 *    - Makes it easy to mock or replace the persistence mechanism for testing.
 *    - Supports Clean Architecture by enforcing separation of concerns.
 *
 * Overall:
 * IUnitOfWork provides a simple, testable contract to commit multiple changes
 * in a single transaction, ensuring data integrity and clean separation
 * between application and infrastructure layers.
 */

namespace Core.Application.Abstractions.Data;

/// <summary>
/// Unit of Work pattern interface
/// </summary>
public interface IUnitOfWork
{
    /// <summary>
    /// Saves all changes to the data store
    /// </summary>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Number of affected records</returns>
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
