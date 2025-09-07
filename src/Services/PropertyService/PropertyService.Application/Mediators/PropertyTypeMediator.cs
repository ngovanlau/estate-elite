using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace PropertyService.Application.Mediators;

using Common.Application.Responses;
using Queries.PropertyTypes;
using Requests.PropertyTypes;

public static class PropertyTypeMediator
{
    public static void AddPropertyTypeMediator(this MediatRServiceConfiguration configuration, ServiceLifetime life = ServiceLifetime.Scoped)
    {
        configuration.AddBehavior<IRequestHandler<GetPropertyTypeRequest, ApiResponse>, GetPropertyTypeHandler>(life);
    }
}
