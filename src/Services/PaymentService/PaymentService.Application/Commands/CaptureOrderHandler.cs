using AutoMapper;
using DistributedCache.Redis;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using PaymentService.Application.Dtos;
using PaymentService.Application.Interfaces;
using PaymentService.Application.Requests;
using PaymentService.Domain.Entities;
using SharedKernel.Enums;
using SharedKernel.Extensions;
using SharedKernel.Responses;
using static SharedKernel.Constants.ErrorCode;

namespace PaymentService.Application.Commands;

public class CaptureOrderHandler(
    IValidator<CaptureOrderRequest> validator,
    ITransactionRepository repository,
    IPaypalService paypalService,
    IMapper mapper,
    IDistributedCache cache,
    ILogger<CaptureOrderHandler> logger) : IRequestHandler<CaptureOrderRequest, ApiResponse>
{
    public async Task<ApiResponse> Handle(CaptureOrderRequest request, CancellationToken cancellationToken)
    {
        var res = new ApiResponse();

        try
        {
            var validationResult = await validator.ValidateAsync(request, cancellationToken);
            if (!validationResult.IsValid)
            {
                var errors = validationResult.Errors.ToDic();
                return res.SetError(nameof(E001), E001, errors);
            }

            var cacheKey = CacheKeys.ForEntity<Transaction>(request.TransactionId);
            var (success, transaction) = await cache.TryGetValueAsync<Transaction>(cacheKey, cancellationToken);
            if (!success || transaction is null)
            {
                transaction = await repository.GetByIdAsync(request.TransactionId, cancellationToken);
            }
            else
            {
                repository.Attach(transaction);
            }

            if (transaction is null)
            {
                return res.SetError(nameof(E008), string.Format(E008, "Transaction"));
            }

            if (transaction.Status != TransactionStatus.Pending)
            {
                return res.SetError(nameof(E000), "Transaction is not pending");
            }

            if (!await paypalService.CaptureOrderAsync(request.OrderId, cancellationToken))
            {
                return res.SetError(nameof(E120), E120);
            }

            transaction.Status = TransactionStatus.Completed;
            if (!await repository.SaveChangeAsync(cancellationToken))
            {
                return res.SetError(nameof(E120), E120);
            }

            await cache.RemoveAsync(cacheKey, cancellationToken);

            return res.SetSuccess(mapper.Map<TransactionDto>(transaction));
        }
        catch (Exception ex)
        {
            return res.SetError(nameof(E000), E000, ex.Message);
        }
    }
}