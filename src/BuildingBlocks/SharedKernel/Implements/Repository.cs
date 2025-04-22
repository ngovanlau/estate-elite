using Microsoft.EntityFrameworkCore;
using SharedKernel.Interfaces;

namespace SharedKernel.Implements;

public abstract class Repository<T>(DbContext context) : IRepository<T> where T : class
{
    private readonly DbSet<T> _dbSet = context.Set<T>();

    public T Attach(T entity)
    {
        var entry = _dbSet.Attach(entity);
        entry.State = EntityState.Modified;
        return entity;
    }

    public async Task<bool> SaveChangeAsync(CancellationToken cancellationToken = default)
    {
        return await context.SaveChangesAsync(cancellationToken) > 0;
    }
}