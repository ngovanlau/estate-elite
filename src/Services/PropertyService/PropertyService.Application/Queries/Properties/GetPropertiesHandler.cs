using DistributedCache.Redis;
using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using PropertyService.Application.Dtos.Properties;
using PropertyService.Application.Interfaces;
using PropertyService.Application.Requests.Properties;
using PropertyService.Domain.Entities;
using SharedKernel.Commons;
using Common.Application.Responses;
using Common.Application.Settings;
using static Common.Domain.Constants.ErrorCode;

namespace PropertyService.Application.Queries.Properties;

public class GetPropertiesHandler(
    IPropertyRepository repository,
    IDistributedCache cache,
    IOptions<MinioSetting> options,
    ILogger<GetOwnerPropertiesHandler> logger) : IRequestHandler<GetPropertiesRequest, PageApiResponse>
{
    private readonly MinioSetting _setting = options.Value;

    public async Task<PageApiResponse> Handle(GetPropertiesRequest request, CancellationToken cancellationToken)
    {
        var res = new PageApiResponse(request.PageNumber, request.PageSize);

        try
        {
            var prefix = request.LastEntityId.ToString();
            if (!string.IsNullOrWhiteSpace(request.Address)) prefix += request.Address;
            if (request.PropertyTypeId is not null) prefix += request.PropertyTypeId.ToString();

            var cacheKey = CacheKeys.ForDtoCollection<Property, PropertyDto>(prefix);
            var (success, pageResult) = await cache.TryGetValueAsync<PageResult<PropertyDto>>(cacheKey, cancellationToken);

            if (!success || pageResult is null || !pageResult.Items.Any())
            {
                pageResult = await repository.GetPropertyDtosAsync(
                    request.PageSize,
                    request.LastEntityId,
                    p => (string.IsNullOrWhiteSpace(request.Address) || string.IsNullOrWhiteSpace(p.Address.Details) || p.Address.Details.Contains(request.Address))
                        && (request.PropertyTypeId == null || p.PropertyTypeId == request.PropertyTypeId),
                    cancellationToken);
                if (!pageResult.Items.Any())
                {
                    return res.SetError(nameof(E008), string.Format(E008, "Properties"));
                }

                foreach (var property in pageResult.Items)
                {
                    property.ImageUrl = $"{_setting.Endpoint}/{_setting.BucketName}/{property.ObjectName}";
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
