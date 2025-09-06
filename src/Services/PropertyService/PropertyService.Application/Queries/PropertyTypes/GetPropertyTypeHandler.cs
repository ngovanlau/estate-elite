using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;

namespace PropertyService.Application.Queries.PropertyTypes;

using DistributedCache.Redis;
using Dtos.PropertyTypes;
using Interfaces;
using PropertyService.Domain.Entities;
using Requests.PropertyTypes;
using Common.Application.Responses;
using static SharedKernel.Constants.ErrorCode;

public class GetPropertyTypeHandler(
    IPropertyTypeRepository repository,
    IDistributedCache cache,
    ILogger<GetPropertyTypeHandler> logger) : IRequestHandler<GetPropertyTypeRequest, ApiResponse>
{
    public async Task<ApiResponse> Handle(GetPropertyTypeRequest request, CancellationToken cancellationToken)
    {
        var res = new ApiResponse();
        logger.LogInformation("Starting to handle GetPropertyTypeRequest");

        try
        {
            var cacheKey = CacheKeys.ForDtoCollection<PropertyType, PropertyTypeDto>();
            var (success, list) = await cache.TryGetValueAsync<List<PropertyTypeDto>>(cacheKey, cancellationToken);
            if (!success || list is null || list.Count == 0)
            {
                logger.LogDebug("Cache miss for key: {CacheKey}", cacheKey);
                logger.LogInformation("Loading property types from repository");
                list = await repository.GetAllPropertyTypeDtoAsync(cancellationToken);
                await cache.SetAsync(cacheKey, list, cancellationToken);
            }

            if (list is null || list.Count == 0)
            {
                logger.LogWarning("No property types found");
                return res.SetError(nameof(E008), string.Format(E008, "Property type list"));
            }

            logger.LogInformation("Successfully retrieved {Count} property types", list.Count);
            return res.SetSuccess(list);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error occurred while retrieving property types: {ErrorMessage}", ex.Message);
            return res.SetError(nameof(E000), E000);
        }
    }
}
