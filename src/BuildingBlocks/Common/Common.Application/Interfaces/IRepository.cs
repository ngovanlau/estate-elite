using System.Data;
using System.Linq.Expressions;
using Core.Domain.Primitives;

namespace Common.Application.Interfaces;

public interface IRepository<T> where T : AuditableEntity
{
    Task<bool> AddEntityAsync(T entity, CancellationToken cancellationToken = default);
    Task<T?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<T?> GetByIdWithIncludeAsync(Guid id, Func<IQueryable<T>, IQueryable<T>> include, CancellationToken cancellationToken = default);
    Task<TDto?> GetDtoByIdAsync<TDto>(Guid id, CancellationToken cancellationToken = default) where TDto : class;
    Task<TDto?> GetDtoByIdAsync<TDto>(Guid id, Expression<Func<T, bool>>? additionalCondition = null, CancellationToken cancellationToken = default) where TDto : class;
    Task<bool> SaveChangeAsync(CancellationToken cancellationToken = default);
    T Attach(T entity);
    Task<ITransaction> BeginTransactionAsync(CancellationToken cancellationToken = default);
}

