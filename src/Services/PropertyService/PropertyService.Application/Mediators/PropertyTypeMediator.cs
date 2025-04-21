using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace PropertyService.Application.Mediators;

using Queries.PropertyTypes;
using Requests.PropertyTypes;
using SharedKernel.Commons;

public static class PropertyTypeMediator
{
    public static void AddPropertyTypeMediator(this MediatRServiceConfiguration configuration, ServiceLifetime life = ServiceLifetime.Scoped)
    {
        configuration.AddBehavior<IRequestHandler<GetPropertyTypeRequest, ApiResponse>, GetPropertyTypeHandler>(life);
    }
}
