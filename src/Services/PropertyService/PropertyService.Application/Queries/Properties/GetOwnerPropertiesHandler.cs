using DistributedCache.Redis;
using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using PropertyService.Application.Dtos.Properties;
using PropertyService.Application.Interfaces;
using PropertyService.Application.Requests.Properties;
using SharedKernel.Interfaces;
using SharedKernel.Responses;
using static SharedKernel.Constants.ErrorCode;

namespace PropertyService.Application.Queries.Properties;

public class GetOwnerPropertiesHandler(
    IPropertyRepository repository,
    IDistributedCache cache,
    ICurrentUserService currentUserService,
    ILogger<GetOwnerPropertiesHandler> logger) : IRequestHandler<GetOwnerPropertiesRequest, ApiResponse>
{
    public async Task<ApiResponse> Handle(GetOwnerPropertiesRequest request, CancellationToken cancellationToken)
    {
        var res = new PageApiResponse(request.PageNumber, request.PageSize);

        try
        {
            if (currentUserService.Id is null)
            {
                return res.SetError(nameof(E103), E103);
            }
            var ownerId = currentUserService.Id.Value;

            var cacheKey = CacheKeys.ForDtoCollection<OwnerPropertyDto>(ownerId.ToString());
            var (success, properties) = await cache.TryGetValueAsync<List<OwnerPropertyDto>>(cacheKey, cancellationToken);
            if (!success || properties is null || !properties.Any())
            {
                properties = await repository.GetOwnerPropertyDtosAsync(ownerId, cancellationToken);
                if (properties is null)
                {
                    return res.SetError(nameof(E008), string.Format(E008, "Owner properties"));
                }

                await cache.SetAsync(cacheKey, properties, cancellationToken);
            }

            return res.SetSuccess(properties);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error occurred while getting owner properties");
            return res.SetError(nameof(E000), E000, ex);
        }
    }
}
