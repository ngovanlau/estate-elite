using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using PropertyService.Application.Interfaces;
using PropertyService.Application.Requests.Properties;
using SharedKernel.Responses;

namespace PropertyService.Application.Queries.Properties;

public class GetOwnerPropertiesHandler(
    IPropertyRepository repository,
    IDistributedCache cache,
    ILogger<GetOwnerPropertiesHandler> logger) : IRequestHandler<GetOwnerPropertiesRequest, ApiResponse>
{
    public Task<ApiResponse> Handle(GetOwnerPropertiesRequest request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
