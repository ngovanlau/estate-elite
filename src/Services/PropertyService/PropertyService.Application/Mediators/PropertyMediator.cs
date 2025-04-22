using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace PropertyService.Application.Mediators;

using PropertyService.Application.Commands.Properties;
using PropertyService.Application.Requests.Properties;
using SharedKernel.Commons;

public static class PropertyMediator
{
    public static void AddPropertyMediator(this MediatRServiceConfiguration configuration, ServiceLifetime life = ServiceLifetime.Scoped)
    {
        configuration.AddBehavior<IRequestHandler<CreatePropertyRequest, ApiResponse>, CreatePropertyHandler>(life);
    }
}
