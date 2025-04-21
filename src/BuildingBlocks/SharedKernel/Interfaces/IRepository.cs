namespace SharedKernel.Interfaces;

public interface IRepository<T>
{
    T Attach(T entity);
}

