using AutoMapper;
using Common.Application.Responses;
using Common.Application.Settings;
using Common.Domain.Enums;
using Common.Infrastructure.Extensions;
using Contracts.Grpc.Protos;
using DistributedCache.Redis;
using FluentValidation;
using Grpc.Net.Client;
using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using PaymentService.Application.Dtos;
using PaymentService.Application.Interfaces;
using PaymentService.Application.Requests;
using PaymentService.Domain.Entities;
using static Common.Domain.Constants.ErrorCode;

namespace PaymentService.Application.Commands;

public class CaptureOrderHandler(
    IValidator<CaptureOrderRequest> validator,
    ITransactionRepository repository,
    IPaypalService paypalService,
    IMapper mapper,
    IDistributedCache cache,
    IOptions<GrpcEndpointSetting> options,
    ILogger<CaptureOrderHandler> logger) : IRequestHandler<CaptureOrderRequest, ApiResponse>
{
    private readonly GrpcEndpointSetting _setting = options.Value;

    public async Task<ApiResponse> Handle(CaptureOrderRequest request, CancellationToken cancellationToken)
    {
        var res = new ApiResponse();

        try
        {
            logger.LogInformation("Starting order capture for transaction {TransactionId}", request.TransactionId);

            // Validation
            var validationResult = await validator.ValidateAsync(request, cancellationToken);
            if (!validationResult.IsValid)
            {
                var errors = validationResult.Errors;
                logger.LogWarning("Validation failed for transaction {TransactionId}. Errors: {Errors}",
                    request.TransactionId, errors);
                return res.SetError(nameof(E001), E001, errors);
            }

            // Cache/DB lookup
            var cacheKey = CacheKeys.ForEntity<Transaction>(request.TransactionId);
            var (success, transaction) = await cache.TryGetValueAsync<Transaction>(cacheKey, cancellationToken);

            if (!success || transaction is null)
            {
                logger.LogDebug("Transaction {TransactionId} not found in cache, querying database", request.TransactionId);
                transaction = await repository.GetByIdAsync(request.TransactionId, cancellationToken);
            }
            else
            {
                repository.Attach(transaction);
            }

            if (transaction is null)
            {
                logger.LogWarning("Transaction {TransactionId} not found", request.TransactionId);
                return res.SetError(nameof(E008), string.Format(E008, "Transaction"));
            }

            // Transaction status check
            if (transaction.Status != TransactionStatus.Pending)
            {
                logger.LogWarning("Transaction {TransactionId} is not in pending state. Current status: {Status}",
                    request.TransactionId, transaction.Status);
                return res.SetError(nameof(E000), "Transaction is not pending");
            }

            // PayPal capture
            logger.LogInformation("Attempting to capture PayPal order {OrderId}", request.OrderId);
            if (!await paypalService.CaptureOrderAsync(request.OrderId, cancellationToken))
            {
                logger.LogError("Failed to capture PayPal order {OrderId}", request.OrderId);
                return res.SetError(nameof(E120), E120);
            }

            // Update transaction
            transaction.Status = TransactionStatus.Completed;
            if (!await repository.SaveChangeAsync(cancellationToken))
            {
                logger.LogError("Failed to save transaction {TransactionId} after PayPal capture", request.TransactionId);
                return res.SetError(nameof(E120), E120);
            }

            await cache.RemoveAsync(cacheKey, cancellationToken);
            logger.LogDebug("Removed transaction {TransactionId} from cache", request.TransactionId);

            // Create property rental
            logger.LogInformation("Creating property rental for transaction {TransactionId}", request.TransactionId);
            if (!await CreatePropertyRentAsync(transaction.PropertyId, transaction.UserId, transaction.RentalPeriod, cancellationToken))
            {
                logger.LogError("Failed to create property rental for transaction {TransactionId}", request.TransactionId);
                return res.SetError(nameof(E120), E120);
            }

            logger.LogInformation("Successfully processed transaction {TransactionId}", request.TransactionId);
            return res.SetSuccess(mapper.Map<TransactionDto>(transaction));
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unexpected error while processing transaction {TransactionId}", request.TransactionId);
            return res.SetError(nameof(E000), E000, ex.Message);
        }
    }

    private async Task<bool> CreatePropertyRentAsync(Guid propertyId, Guid userId, int rentalPeriod, CancellationToken cancellationToken)
    {
        try
        {
            logger.LogDebug("Creating gRPC channel for PropertyService");
            using var channel = GrpcChannel.ForAddress(_setting.Property);
            var client = new PropertyService.PropertyServiceClient(channel);

            var request = new CreatePropertyRentalRequest
            {
                PropertyId = propertyId.ToString(),
                UserId = userId.ToString(),
                RentalPeriod = rentalPeriod
            };

            logger.LogInformation("Calling PropertyService for property {PropertyId} and user {UserId}",
                propertyId, userId);
            var response = await client.CreatePropertyRentalAsync(request, cancellationToken: cancellationToken)
                ?? throw new Exception("Null response received from PropertyService");

            logger.LogDebug("PropertyService response: {Success}", response.Success);
            return response.Success;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error creating property rental for property {PropertyId}", propertyId);
            throw;
        }
    }
}
