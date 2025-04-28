using AutoMapper;
using DistributedCache.Redis;
using FluentValidation;
using Grpc.Net.Client;
using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using PaymentService.Application.Dtos;
using PaymentService.Application.Interfaces;
using PaymentService.Application.Requests;
using PaymentService.Domain.Entities;
using SharedKernel.Enums;
using SharedKernel.Extensions;
using SharedKernel.Interfaces;
using SharedKernel.Protos;
using SharedKernel.Responses;
using static SharedKernel.Constants.ErrorCode;

namespace PaymentService.Application.Commands;

public class RentPropertyHandler(
    IValidator<RentPropertyRequest> validator,
    ITransactionRepository repository,
    ICurrentUserService currentUserService,
    IPaypalService paypalService,
    IMapper mapper,
    IDistributedCache cache,
    ILogger<RentPropertyHandler> logger) : IRequestHandler<RentPropertyRequest, ApiResponse>
{
    public async Task<ApiResponse> Handle(RentPropertyRequest request, CancellationToken cancellationToken)
    {
        var response = new ApiResponse();

        try
        {
            // Validate request
            var validationResult = await validator.ValidateAsync(request, cancellationToken);
            if (!validationResult.IsValid)
            {
                var errors = validationResult.Errors.ToDic();
                logger.LogWarning("Validation failed for RentPropertyRequest: {Errors}", errors);
                return response.SetError(nameof(E000), E000, errors);
            }

            // Check current user
            if (currentUserService.Id is null)
            {
                logger.LogWarning("Current user ID not found");
                return response.SetError(nameof(E103), E103);
            }
            var userId = currentUserService.Id.Value;

            // Get property and owner details
            logger.LogInformation("Getting property details for PropertyId: {PropertyId}", request.PropertyId);
            var property = await GetPropertyByGrpcAsync(request.PropertyId, cancellationToken);

            logger.LogInformation("Getting seller details for OwnerId: {OwnerId}", property.OwnerId);
            var seller = await GetUserByGrpcAsync(property.OwnerId, cancellationToken);

            if (property is null || seller is null)
            {
                logger.LogError("Property or owner not found. PropertyId: {PropertyId}, OwnerId: {OwnerId}",
                    request.PropertyId, property?.OwnerId);
                return response.SetError(nameof(E008), string.Format(E008, "Property or owner"));
            }

            // Calculate amount
            var amount = request.RentalPeriod * property.Price;
            logger.LogInformation("Calculated rental amount: {Amount} {Currency} for {Period} periods",
                amount, property.CurrencyUnit, request.RentalPeriod);

            // Create payment order
            logger.LogInformation("Creating PayPal order for property: {PropertyTitle}", property.Title);
            var paymentResult = await paypalService.CreateOrderAsync(
                property.Title,
                amount,
                property.CurrencyUnit,
                seller,
                request.ReturnUrl,
                request.CancelUrl,
                cancellationToken);

            if (!paymentResult.Success)
            {
                logger.LogError("Failed to create PayPal order for PropertyId: {PropertyId}", request.PropertyId);
                return response.SetError(nameof(E119), E119);
            }

            // Create transaction record
            var transaction = new Transaction
            {
                Id = paymentResult.TransactionId,
                Amount = amount,
                CurrencyUnit = property.CurrencyUnit,
                PaymentMethod = PaymentMethod.Paypal,
                UserId = userId,
                PropertyId = property.Id,
                OrderId = paymentResult.OrderId
            };

            logger.LogInformation("Creating transaction record with TransactionId: {TransactionId}", transaction.Id);
            if (!await repository.CreateTransaction(transaction, cancellationToken))
            {
                logger.LogError("Failed to create transaction record for TransactionId: {TransactionId}", transaction.Id);
                return response.SetError(nameof(E119), E119);
            }

            var cacheKey = CacheKeys.ForEntity<Transaction>(transaction.Id);
            await cache.SetAsync(cacheKey, transaction, cancellationToken: cancellationToken);

            logger.LogInformation("Successfully processed rental request for PropertyId: {PropertyId}", request.PropertyId);
            return response.SetSuccess(new
            {
                OrderId = paymentResult.OrderId,
                TransactionId = transaction.Id,
                Links = paymentResult.Links
            });
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unexpected error occurred while processing rental request");
            return response.SetError(nameof(E000), E000, ex);
        }
    }

    private async Task<PropertyDto> GetPropertyByGrpcAsync(Guid propertyId, CancellationToken cancellationToken)
    {
        try
        {
            using var channel = GrpcChannel.ForAddress("https://localhost:5102");
            var client = new PropertyService.PropertyServiceClient(channel);

            var request = new GetPropertyRequest { Id = propertyId.ToString() };
            var response = await client.GetPropertyAsync(request, cancellationToken: cancellationToken)
                ?? throw new Exception("Property not found");

            logger.LogDebug("Retrieved property details for PropertyId: {PropertyId}", propertyId);
            return mapper.Map<PropertyDto>(response);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error occurred while getting property details for PropertyId: {PropertyId}", propertyId);
            throw;
        }
    }

    private async Task<SellerDto> GetUserByGrpcAsync(Guid userId, CancellationToken cancellationToken)
    {
        try
        {
            using var channel = GrpcChannel.ForAddress("https://localhost:5101");
            var client = new UserService.UserServiceClient(channel);

            var request = new GetUserRequest { Id = userId.ToString() };
            var response = await client.GetUserAsync(request, cancellationToken: cancellationToken);

            if (response == null)
            {
                throw new Exception($"User not found for UserId: {userId}");
            }

            logger.LogDebug("Retrieved user details for UserId: {UserId}", userId);
            return mapper.Map<SellerDto>(response);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error occurred while getting user details for UserId: {UserId}", userId);
            throw;
        }
    }
}