namespace SharedKernel.Interfaces;

public interface IRepository<T>
{
    Task<bool> SaveChangeAsync(CancellationToken cancellationToken = default);
    T Attach(T entity);
}

