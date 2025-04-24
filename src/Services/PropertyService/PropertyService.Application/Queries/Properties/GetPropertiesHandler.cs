using DistributedCache.Redis;
using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using PropertyService.Application.Dtos.Properties;
using PropertyService.Application.Interfaces;
using PropertyService.Application.Requests.Properties;
using SharedKernel.Responses;
using SharedKernel.Settings;
using static SharedKernel.Constants.ErrorCode;

namespace PropertyService.Application.Queries.Properties;

public class GetPropertiesHandler(
    IPropertyRepository repository,
    IDistributedCache cache,
    IOptions<MinioSetting> options,
    ILogger<GetOwnerPropertiesHandler> logger) : IRequestHandler<GetPropertiesRequest, ApiResponse>
{
    private readonly MinioSetting _setting = options.Value;

    public async Task<ApiResponse> Handle(GetPropertiesRequest request, CancellationToken cancellationToken)
    {
        var res = new ApiResponse();

        try
        {
            var cacheKey = CacheKeys.ForDtoCollection<PropertyDto>();
            var (success, properties) = await cache.TryGetValueAsync<List<PropertyDto>>(cacheKey, cancellationToken);

            if (!success || properties is null || !properties.Any())
            {
                properties = await repository.GetPropertyDtosAsync(cancellationToken);
                if (properties is null)
                {
                    return res.SetError(nameof(E008), string.Format(E008, "Owner properties"));
                }

                foreach (var property in properties)
                {
                    property.ImageUrl = $"{_setting.Endpoint}/{_setting.BucketName}/{property.ObjectName}";
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
