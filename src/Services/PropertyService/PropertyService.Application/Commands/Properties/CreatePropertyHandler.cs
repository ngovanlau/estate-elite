using FluentValidation;
using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using PropertyService.Application.Requests.Properties;
using SharedKernel.Commons;

namespace PropertyService.Application.Commands.Properties;

public class CreatePropertyHandler(
    IValidator<CreatePropertyRequest> validator,
    IDistributedCache cache,
    ILogger<CreatePropertyHandler> logger) : IRequestHandler<CreatePropertyRequest, ApiResponse>
{
    public Task<ApiResponse> Handle(CreatePropertyRequest request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
