using MediatR;
using Microsoft.Extensions.DependencyInjection;
using PropertyService.Application.Commands.Properties;
using PropertyService.Application.Queries.Properties;
using PropertyService.Application.Requests.Properties;
using Common.Application.Responses;

namespace PropertyService.Application.Mediators;

public static class PropertyMediator
{
    public static void AddPropertyMediator(this MediatRServiceConfiguration configuration, ServiceLifetime life = ServiceLifetime.Scoped)
    {
        configuration.AddBehavior<IRequestHandler<CreatePropertyRequest, ApiResponse>, CreatePropertyHandler>(life);
        configuration.AddBehavior<IRequestHandler<GetOwnerPropertiesRequest, PageApiResponse>, GetOwnerPropertiesHandler>(life);
        configuration.AddBehavior<IRequestHandler<GetPropertiesRequest, PageApiResponse>, GetPropertiesHandler>(life);
        configuration.AddBehavior<IRequestHandler<GetPropertyDetailsRequest, ApiResponse>, GetPropertyDetailsHandler>(life);
        configuration.AddBehavior<IRequestHandler<TrackViewRequest>, TrackViewHandler>(life);
        configuration.AddBehavior<IRequestHandler<GetMostViewPropertiesRequest, ApiResponse>, GetMostViewPropertiesHandler>(life);
    }
}
