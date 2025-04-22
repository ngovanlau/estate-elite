namespace SharedKernel.Interfaces;

public interface IRepository<T> where T : class
{
    Task<bool> SaveChangeAsync(CancellationToken cancellationToken = default);
    T Attach(T entity);
}

