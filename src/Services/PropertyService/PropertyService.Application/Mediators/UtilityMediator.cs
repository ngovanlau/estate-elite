using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace PropertyService.Application.Mediators;

using Common.Application.Responses;
using Queries.Utilities;
using Requests.Utilities;

public static class UtilityMediator
{
    public static void AddUtilityMediator(this MediatRServiceConfiguration configuration, ServiceLifetime life = ServiceLifetime.Scoped)
    {
        configuration.AddBehavior<IRequestHandler<GetUtilityRequest, ApiResponse>, GetUtilityHandler>(life);
    }
}
