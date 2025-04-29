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

        try
        {
            var validationResult = await validator.ValidateAsync(request, cancellationToken);
            if (!validationResult.IsValid)
            {
                var errors = validationResult.Errors.ToDic();
                return res.SetError(nameof(E000), E000, errors);
            }

            var cacheKey = CacheKeys.ForDto<Property, PropertyDetailsDto>(request.Id);
            var (success, propertyDetailsDto) = await cache.TryGetValueAsync<PropertyDetailsDto>(cacheKey, cancellationToken);
            if (!success || propertyDetailsDto is null)
            {
                propertyDetailsDto = await repository.GetDtoByIdAsync<PropertyDetailsDto>(request.Id, cancellationToken);
                if (propertyDetailsDto is null)
                {
                    return res.SetError(nameof(E000), E000, "Property not found");
                }

                propertyDetailsDto.Owner = await GetOwnerDtoByGrpcAsync(propertyDetailsDto.OwnerId, cancellationToken);
                propertyDetailsDto.Images = propertyDetailsDto.Images.Select(image => $"{_setting.Endpoint}/{_setting.BucketName}/{image}").ToList();

                await cache.SetAsync(cacheKey, propertyDetailsDto, cancellationToken);
            }

            return res.SetSuccess(propertyDetailsDto);
        }
        catch (Exception ex)
        {
            return res.SetError(nameof(E000), E000);
        }
    }

    private async Task<OwnerDto> GetOwnerDtoByGrpcAsync(Guid userId, CancellationToken cancellationToken = default)
    {
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
            return mapper.Map<OwnerDto>(response);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error occurred while getting owner details");
            throw;
        }
    }
}