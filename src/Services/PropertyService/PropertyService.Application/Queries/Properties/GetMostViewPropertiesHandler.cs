using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using PropertyService.Application.Interfaces;
using PropertyService.Application.Requests.Properties;
using SharedKernel.Responses;
using SharedKernel.Settings;
using static SharedKernel.Constants.ErrorCode;

namespace PropertyService.Application.Queries.Properties;

public class GetMostViewPropertiesHandler(
    IPropertyRepository repository,
    IOptions<MinioSetting> options,
    ILogger<GetMostViewPropertiesHandler> logger) : IRequestHandler<GetMostViewPropertiesRequest, ApiResponse>
{
    private readonly MinioSetting _setting = options.Value;

    public async Task<ApiResponse> Handle(GetMostViewPropertiesRequest request, CancellationToken cancellationToken)
    {
        var res = new ApiResponse();

        try
        {
            var propertyDtos = await repository.GetMostViewPropertyDtosAsync(request.Quantity, cancellationToken);

            if (!propertyDtos.Any())
            {
                return res.SetError(nameof(E008), string.Format(E008, "Properties"));
            }

            foreach (var property in propertyDtos)
            {
                property.ImageUrl = $"{_setting.Endpoint}/{_setting.BucketName}/{property.ObjectName}";
            }

            return res.SetSuccess(propertyDtos);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error occurred while getting owner properties");
            return res.SetError(nameof(E000), E000);
        }
    }
}
