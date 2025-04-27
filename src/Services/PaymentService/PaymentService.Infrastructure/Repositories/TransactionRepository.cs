using AutoMapper;
using PaymentService.Application.Interfaces;
using PaymentService.Domain.Entities;
using PaymentService.Infrastructure.Data;
using SharedKernel.Implements;

namespace PaymentService.Infrastructure.Repositories;

public class TransactionRepository(
    PaymentContext context,
    IMapper mapper) : Repository<Transaction>(context, mapper), ITransactionRepository
{
    public async Task<bool> CreateTransaction(Transaction transaction, CancellationToken cancellationToken = default)
    {
        await context.Transactions.AddAsync(transaction, cancellationToken);
        return await SaveChangeAsync(cancellationToken);
    }
}
