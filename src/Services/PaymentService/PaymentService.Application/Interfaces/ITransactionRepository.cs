using SharedKernel.Interfaces;
using PaymentService.Domain.Entities;

namespace PaymentService.Application.Interfaces;

public interface ITransactionRepository : IRepository<Transaction>
{
    Task<bool> CreateTransaction(Transaction transaction, CancellationToken cancellationToken = default);
}
