using Common.Application.Interfaces;
using Microsoft.EntityFrameworkCore.Storage;

namespace Common.Infrastructure.Implements;

public class DbTransaction(IDbContextTransaction transaction) : ITransaction
{
    public async Task CommitAsync(CancellationToken cancellationToken = default)
    {
        await transaction.CommitAsync(cancellationToken);
    }

    public async void Dispose()
    {
        await transaction.DisposeAsync();
        GC.SuppressFinalize(this);
    }

    public async Task RollbackAsync(CancellationToken cancellationToken = default)
    {
        await transaction.RollbackAsync(cancellationToken);
    }
}
