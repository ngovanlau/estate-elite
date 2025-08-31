/*
 * Unit of Work Pattern (UoW)
 * --------------------------
 * 1. Purpose:
 *    - Coordinates the writing out of changes across multiple repositories.
 *    - Ensures data consistency and atomicity when dealing with a database.
 *    - Abstracts transaction management away from application/business logic.
 *
 * 2. Responsibilities:
 *    - SaveChangesAsync:
 *        → Persists changes tracked by the DbContext (or equivalent data store).
 *    - BeginTransactionAsync:
 *        → Explicitly starts a transaction (optional, depending on strategy).
 *        → Useful when multiple operations must be grouped together.
 *    - CommitTransactionAsync:
 *        → Commits all changes if no errors occur.
 *    - RollbackTransactionAsync:
 *        → Reverts all changes if an error/exception happens.
 *
 * 3. Benefits:
 *    - Centralizes transaction handling instead of scattering it across handlers.
 *    - Supports atomic operations across multiple aggregates/entities.
 *    - Improves testability (mock IUnitOfWork in application layer).
 *
 * 4. Usage Example:
 *    ----------------
 *    public class CreateOrderHandler : IRequestHandler<CreateOrderCommand, Result>
 *    {
 *        private readonly IUnitOfWork _unitOfWork;
 *        private readonly IOrderRepository _orderRepository;
 *
 *        public async Task<Result> Handle(CreateOrderCommand request, CancellationToken ct)
 *        {
 *            await _unitOfWork.BeginTransactionAsync(ct);
 *            try
 *            {
 *                var order = new Order(request.CustomerId, request.Items);
 *                _orderRepository.Add(order);
 *
 *                await _unitOfWork.SaveChangesAsync(ct);
 *                await _unitOfWork.CommitTransactionAsync(ct);
 *
 *                return Result.Success();
 *            }
 *            catch (Exception)
 *            {
 *                await _unitOfWork.RollbackTransactionAsync(ct);
 *                throw;
 *            }
 *        }
 *    }
 *
 * 5. Best Practices:
 *    - Keep transactions short → avoid holding locks for long.
 *    - Use UoW in the Application Layer (Handlers/Services), not in Domain.
 *    - Combine with Repository Pattern for cleaner data access abstraction.
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

    /// <summary>
    /// Begin transaction explicitly (optional, depends on your EF strategy)
    /// </summary>
    /// <param name="cancellationToken">Cancellation token</param>
    Task BeginTransactionAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Commit transaction explicitly
    /// </summary>
    /// <param name="cancellationToken">Cancellation token</param>
    Task CommitTransactionAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Rollback transaction explicitly
    /// </summary>
    /// <param name="cancellationToken">Cancellation token</param>
    Task RollbackTransactionAsync(CancellationToken cancellationToken = default);
}
