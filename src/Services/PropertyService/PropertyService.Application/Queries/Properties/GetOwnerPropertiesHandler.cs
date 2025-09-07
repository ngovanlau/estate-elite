using Common.Application.Interfaces;
using Common.Application.Responses;
using DistributedCache.Redis;
using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using PropertyService.Application.Dtos.Properties;
using PropertyService.Application.Interfaces;
using PropertyService.Application.Requests.Properties;
using PropertyService.Domain.Entities;
using SharedKernel.Commons;
using static Common.Domain.Constants.ErrorCode;

namespace PropertyService.Application.Queries.Properties;

public class GetOwnerPropertiesHandler(
    IPropertyRepository repository,
    IDistributedCache cache,
    ICurrentUserService currentUserService,
    ILogger<GetOwnerPropertiesHandler> logger) : IRequestHandler<GetOwnerPropertiesRequest, PageApiResponse>
{
    public async Task<PageApiResponse> Handle(GetOwnerPropertiesRequest request, CancellationToken cancellationToken)
    {
        var res = new PageApiResponse(request.PageNumber, request.PageSize);

        try
        {
            if (currentUserService.Id is null)
            {
                return res.SetError(nameof(E103), E103);
            }
            var ownerId = currentUserService.Id.Value;

            var cacheKey = CacheKeys.ForDtoCollection<Property, OwnerPropertyDto>(request.PageNumber.ToString());
            var (success, pageResult) = await cache.TryGetValueAsync<PageResult<OwnerPropertyDto>>(cacheKey, cancellationToken);
            if (!success || pageResult is null || !pageResult.Items.Any())
            {
                pageResult = await repository.GetOwnerPropertyDtosAsync(ownerId, request.PageSize, request.PageNumber, cancellationToken);
                if (!pageResult.Items.Any())
                {
                    return res.SetError(nameof(E008), string.Format(E008, "Owner properties"));
                }

                await cache.SetAsync(cacheKey, pageResult, cancellationToken);
            }

            res.TotalRecords = pageResult.TotalRecords;

            return res.SetSuccess(pageResult.Items);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error occurred while getting owner properties");
            return res.SetError(nameof(E000), E000);
        }
    }
}
