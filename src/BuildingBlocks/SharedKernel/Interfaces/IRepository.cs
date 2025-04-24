using SharedKernel.Entities;
using System.Data;

namespace SharedKernel.Interfaces;

public interface IRepository<T> where T : AuditableEntity
{
    Task<T?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<TDto?> GetDtoByIdAsync<TDto>(Guid id, CancellationToken cancellationToken = default) where TDto : class;
    Task<bool> SaveChangeAsync(CancellationToken cancellationToken = default);
    T Attach(T entity);

    Task<ITransaction> BeginTransactionAsync(CancellationToken cancellationToken = default);
    Task<ITransaction> BeginTransactionAsync(IsolationLevel isolationLevel, CancellationToken cancellationToken = default);

}

