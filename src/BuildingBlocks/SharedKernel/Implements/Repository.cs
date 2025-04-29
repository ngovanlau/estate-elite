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

    public async Task<bool> AddEntityAsync(T entity, CancellationToken cancellationToken = default)
    {
        await _dbSet.AddAsync(entity, cancellationToken);
        return await SaveChangeAsync(cancellationToken);
    }

    public async Task<T?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await context.Available<T>().FirstOrDefaultAsync(p => p.Id == id, cancellationToken);
    }

    public async Task<T?> GetByIdWithIncludeAsync(
        Guid id,
        Func<IQueryable<T>, IQueryable<T>> include,
        CancellationToken cancellationToken = default)
    {
        return await include(context.Available<T>()).FirstOrDefaultAsync(e => e.Id == id, cancellationToken);
    }

    public T Attach(T entity)
    {
        var entry = _dbSet.Attach(entity);
        entry.State = EntityState.Modified;
        return entity;
    }

    public async Task<bool> SaveChangeAsync(CancellationToken cancellationToken = default)
    {
        var res = await context.SaveChangesAsync(cancellationToken);
        return res > 0;
    }

    public async Task<ITransaction> BeginTransactionAsync(CancellationToken cancellationToken = default)
    {
        var transaction = await context.Database.BeginTransactionAsync(cancellationToken);
        return new DbTransaction(transaction);
    }

    public async Task<ITransaction> BeginTransactionAsync(IsolationLevel isolationLevel, CancellationToken cancellationToken = default)
    {
        var transaction = await context.Database.BeginTransactionAsync(isolationLevel, cancellationToken);
        return new DbTransaction(transaction);
    }

    public async Task<TDto?> GetDtoByIdAsync<TDto>(Guid id, CancellationToken cancellationToken = default) where TDto : class
    {
        return await context.Available<T>(false)
            .Where(p => p.Id == id)
            .ProjectTo<TDto>(_mapper.ConfigurationProvider)
            .FirstOrDefaultAsync(cancellationToken);
    }
}