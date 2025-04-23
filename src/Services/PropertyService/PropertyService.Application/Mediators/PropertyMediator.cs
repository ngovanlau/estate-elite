using MediatR;
using Microsoft.Extensions.DependencyInjection;
using PropertyService.Application.Commands.Properties;
using PropertyService.Application.Queries.Properties;
using PropertyService.Application.Requests.Properties;
using SharedKernel.Responses;

namespace PropertyService.Application.Mediators;

public static class PropertyMediator
{
    public static void AddPropertyMediator(this MediatRServiceConfiguration configuration, ServiceLifetime life = ServiceLifetime.Scoped)
    {
        configuration.AddBehavior<IRequestHandler<CreatePropertyRequest, ApiResponse>, CreatePropertyHandler>(life);
        configuration.AddBehavior<IRequestHandler<GetOwnerPropertiesRequest, ApiResponse>, GetOwnerPropertiesHandler>(life);
        configuration.AddBehavior<IRequestHandler<GetPropertiesRequest, ApiResponse>, GetPropertiesHandler>(life);
    }
}
