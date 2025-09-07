using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;

namespace PropertyService.Application.Queries.Utilities;

using DistributedCache.Redis;
using Interfaces;
using PropertyService.Application.Dtos.Utilities;
using PropertyService.Application.Requests.Utilities;
using PropertyService.Domain.Entities;
using Common.Application.Responses;
using static Common.Domain.Constants.ErrorCode;

public class GetUtilityHandler(
    IUtilityRepository repository,
    IDistributedCache cache,
    ILogger<GetUtilityHandler> logger) : IRequestHandler<GetUtilityRequest, ApiResponse>
{
    public async Task<ApiResponse> Handle(GetUtilityRequest request, CancellationToken cancellationToken)
    {
        var res = new ApiResponse();
        logger.LogInformation("Starting to handle GetUtilityRequest");

        try
        {
            var cacheKey = CacheKeys.ForDtoCollection<Utility, UtilityDto>();
            var (success, list) = await cache.TryGetValueAsync<List<UtilityDto>>(cacheKey, cancellationToken);
            if (!success || list is null || list.Count == 0)
            {
                logger.LogDebug("Cache miss for key: {CacheKey}", cacheKey);
                logger.LogInformation("Loading property types from repository");
                list = await repository.GetAllUtilityDtoAsync(cancellationToken);
                await cache.SetAsync(cacheKey, list, cancellationToken);
            }

            if (list is null || list.Count == 0)
            {
                logger.LogWarning("No utilities found");
                return res.SetError(nameof(E008), string.Format(E008, "utility list"));
            }

            logger.LogInformation("Successfully retrieved {Count} utilities", list.Count);
            return res.SetSuccess(list);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error occurred while retrieving utilities: {ErrorMessage}", ex.Message);
            return res.SetError(nameof(E000), E000);
        }
    }
}
