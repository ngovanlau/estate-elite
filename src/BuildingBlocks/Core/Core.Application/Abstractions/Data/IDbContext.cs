/*
 * Why define IDbContext:
 * ----------------------
 * 1. Abstraction over EF Core:
 *    - Prevents direct dependency on DbContext in domain/application layers.
 *    - Makes it easier to swap or mock database context in tests.
 *
 * 2. Provides core EF Core operations:
 *    - Set<TEntity>() → access DbSet for querying or updating entities.
 *    - Entry<TEntity>(entity) → access change tracking info for entity.
 *    - SaveChangesAsync() → commit changes to database asynchronously.
 *
 * 3. Testability:
 *    - Allows mocking IDbContext in unit tests without connecting to real database.
 *    - Promotes dependency inversion principle (application depends on abstraction, not concrete DbContext).
 *
 * 4. Separation of Concerns:
 *    - Application services interact with IDbContext.
 *    - Infrastructure layer provides concrete EF Core DbContext implementation.
 *
 * 5. Clean Architecture Principle:
 *    - Domain and application layers do not directly reference EF Core.
 *    - Reduces coupling, improves maintainability, and supports unit testing.
 *
 * Overall:
 * IDbContext provides a clean, testable, and decoupled interface for database
 * operations while preserving the flexibility and features of EF Core.
 */

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Core.Application.Abstractions.Data;

/// <summary>
/// Database context interface
/// </summary>
public interface IDbContext
{
    /// <summary>
    /// Gets DbSet for entity type
    /// </summary>
    /// <typeparam name="TEntity">Entity type</typeparam>
    /// <returns>DbSet</returns>
    DbSet<TEntity> Set<TEntity>() where TEntity : class;

    /// <summary>
    /// Gets entity entry
    /// </summary>
    /// <typeparam name="TEntity">Entity type</typeparam>
    /// <param name="entity">Entity instance</param>
    /// <returns>Entity entry</returns>
    EntityEntry<TEntity> Entry<TEntity>(TEntity entity) where TEntity : class;

    /// <summary>
    /// Saves changes asynchronously
    /// </summary>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Number of affected records</returns>
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}