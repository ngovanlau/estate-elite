/*
 * Why define IRepository<T>:
 * -------------------------
 * 1. Repository Pattern:
 *    - Provides an abstraction over data access.
 *    - Allows application code to work with entities without knowing ORM details.
 *    - Supports testability by mocking repositories in unit tests.
 *
 * 2. Generic Repository:
 *    - Works for any entity type T that inherits from Entity.
 *    - Centralizes common CRUD operations:
 *        - GetByIdAsync, GetAllAsync
 *        - Add, Update, Remove
 *        - ExistsAsync
 *
 * 3. Separation of Concerns:
 *    - Domain/application layers do not depend on EF Core or any database directly.
 *    - Infrastructure layer implements the repository with EF Core, MongoDB, etc.
 *
 * 4. Asynchronous Operations:
 *    - Async methods allow non-blocking database calls.
 *    - CancellationToken supports cooperative cancellation.
 *
 * 5. Benefits:
 *    - Reduces duplication of data access code.
 *    - Improves maintainability and testability.
 *    - Provides a consistent API for working with entities across the application.
 *
 * Overall:
 * IRepository<T> provides a clean, testable, and reusable abstraction for
 * performing CRUD operations on entities, decoupling the application layer
 * from the underlying persistence technology.
 */

using Core.Domain.Primitives;

namespace Core.Application.Abstractions.Data;

/// <summary>
/// Generic repository interface
/// </summary>
/// <typeparam name="T">Entity type</typeparam>
public interface IRepository<T> where T : Entity
{
    /// <summary>
    /// Gets entity by ID
    /// </summary>
    /// <param name="id">Entity ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Entity or null if not found</returns>
    Task<T?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets all entities
    /// </summary>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>List of entities</returns>
    Task<List<T>> GetAllAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Adds entity to repository
    /// </summary>
    /// <param name="entity">Entity to add</param>
    void Add(T entity);

    /// <summary>
    /// Updates entity in repository
    /// </summary>
    /// <param name="entity">Entity to update</param>
    void Update(T entity);

    /// <summary>
    /// Removes entity from repository
    /// </summary>
    /// <param name="entity">Entity to remove</param>
    void Remove(T entity);

    /// <summary>
    /// Checks if entity exists by ID
    /// </summary>
    /// <param name="id">Entity ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>True if exists</returns>
    Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken = default);
}
