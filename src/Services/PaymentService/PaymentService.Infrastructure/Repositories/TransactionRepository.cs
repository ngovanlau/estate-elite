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
}
