using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using SharedKernel.Entities;
using SharedKernel.Extensions;
using SharedKernel.Interfaces;
using System.Data;

namespace SharedKernel.Implements;

public abstract class Repository<T>(DbContext context, IMapper mapper) : IRepository<T> where T : AuditableEntity
{
    private readonly DbSet<T> _dbSet = context.Set<T>();
    protected readonly IMapper _mapper = mapper;

    public async Task<T?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await context.Available<T>(false).FirstOrDefaultAsync(p => p.Id == id, cancellationToken);
    }

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

    public async Task<ITransaction> BeginTransactionAsync(CancellationToken cancellationToken = default)
    {
        var transaction = await context.Database.BeginTransactionAsync(cancellationToken);
        return new Transaction(transaction);
    }

    public async Task<ITransaction> BeginTransactionAsync(IsolationLevel isolationLevel, CancellationToken cancellationToken = default)
    {
        var transaction = await context.Database.BeginTransactionAsync(isolationLevel, cancellationToken);
        return new Transaction(transaction);
    }

    public async Task<TDto?> GetDtoByIdAsync<TDto>(Guid id, CancellationToken cancellationToken = default) where TDto : class
    {
        return await context.Available<T>(false)
            .Where(p => p.Id == id)
            .ProjectTo<TDto>(_mapper.ConfigurationProvider)
            .FirstOrDefaultAsync(cancellationToken);
    }
}