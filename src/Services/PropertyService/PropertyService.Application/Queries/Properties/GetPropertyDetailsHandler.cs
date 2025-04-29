using AutoMapper;
using DistributedCache.Redis;
using FluentValidation;
using Grpc.Net.Client;
using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using PropertyService.Application.Dtos.Properties;
using PropertyService.Application.Interfaces;
using PropertyService.Application.Requests.Properties;
using PropertyService.Domain.Entities;
using SharedKernel.Extensions;
using SharedKernel.Protos;
using SharedKernel.Responses;
using SharedKernel.Settings;
using static SharedKernel.Constants.ErrorCode;

namespace PropertyService.Application.Queries.Properties;

public class GetPropertyDetailsHandler(
    IValidator<GetPropertyDetailsRequest> validator,
    IPropertyRepository repository,
    IDistributedCache cache,
    IMapper mapper,
    IOptions<MinioSetting> options,
    ILogger<GetPropertyDetailsHandler> logger) : IRequestHandler<GetPropertyDetailsRequest, ApiResponse>
{
    private readonly MinioSetting _setting = options.Value;

    public async Task<ApiResponse> Handle(GetPropertyDetailsRequest request, CancellationToken cancellationToken)
    {
        var res = new ApiResponse();
        logger.LogInformation("Handling GetPropertyDetailsRequest for PropertyId: {PropertyId}", request.Id);

        try
        {
            logger.LogDebug("Validating GetPropertyDetailsRequest for PropertyId: {PropertyId}", request.Id);
            var validationResult = await validator.ValidateAsync(request, cancellationToken);
            if (!validationResult.IsValid)
            {
                logger.LogWarning("Validation failed for PropertyId: {PropertyId}. Errors: {ValidationErrors}",
                    request.Id, validationResult.Errors);
                var errors = validationResult.Errors.ToDic();
                return res.SetError(nameof(E000), E000, errors);
            }

            var cacheKey = CacheKeys.ForDto<Property, PropertyDetailsDto>(request.Id);
            logger.LogDebug("Checking cache for PropertyId: {PropertyId} with key: {CacheKey}", request.Id, cacheKey);

            var (success, propertyDetailsDto) = await cache.TryGetValueAsync<PropertyDetailsDto>(cacheKey, cancellationToken);
            if (!success || propertyDetailsDto is null)
            {
                logger.LogDebug("Cache miss for PropertyId: {PropertyId}. Querying database.", request.Id);

                propertyDetailsDto = await repository.GetDtoByIdAsync<PropertyDetailsDto>(request.Id, cancellationToken);
                if (propertyDetailsDto is null)
                {
                    logger.LogError("Property not found in database for PropertyId: {PropertyId}", request.Id);
                    return res.SetError(nameof(E000), E000, "Property not found");
                }

                logger.LogDebug("Retrieving owner details via gRPC for UserId: {UserId}", propertyDetailsDto.OwnerId);
                propertyDetailsDto.Owner = await GetOwnerDtoByGrpcAsync(propertyDetailsDto.OwnerId, cancellationToken);

                logger.LogDebug("Processing image URLs for PropertyId: {PropertyId}", request.Id);
                propertyDetailsDto.Images = propertyDetailsDto.Images
                    .Select(image => $"{_setting.Endpoint}/{_setting.BucketName}/{image}")
                    .ToList();

                logger.LogDebug("Caching property details for PropertyId: {PropertyId}", request.Id);
                await cache.SetAsync(cacheKey, propertyDetailsDto, cancellationToken);
            }
            else
            {
                logger.LogDebug("Cache hit for PropertyId: {PropertyId}", request.Id);
            }

            logger.LogInformation("Successfully processed GetPropertyDetailsRequest for PropertyId: {PropertyId}", request.Id);
            return res.SetSuccess(propertyDetailsDto);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error occurred while processing PropertyId: {PropertyId}. Message: {ErrorMessage}",
                request.Id, ex.Message);
            return res.SetError(nameof(E000), E000);
        }
    }

    private async Task<OwnerDto> GetOwnerDtoByGrpcAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        logger.LogInformation("Initiating gRPC call for UserId: {UserId}", userId);
        using var channel = GrpcChannel.ForAddress("https://localhost:5101");
        var client = new UserService.UserServiceClient(channel);

        var request = new GetUserRequest
        {
            Id = userId.ToString()
        };

        try
        {
            var response = await client.GetUserAsync(request, cancellationToken: cancellationToken)
                ?? throw new Exception("User not found");

            logger.LogDebug("Successfully retrieved user details via gRPC for UserId: {UserId}", userId);
            return mapper.Map<OwnerDto>(response);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "gRPC call failed for UserId: {UserId}. Error: {ErrorMessage}",
                userId, ex.Message);
            throw;
        }
    }
}