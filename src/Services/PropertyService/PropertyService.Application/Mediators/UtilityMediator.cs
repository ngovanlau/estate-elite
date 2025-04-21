using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace PropertyService.Application.Mediators;

using Queries.Utilities;
using Requests.Utilities;
using SharedKernel.Commons;

public static class UtilityMediator
{
    public static void AddUtilityMediator(this MediatRServiceConfiguration configuration, ServiceLifetime life = ServiceLifetime.Scoped)
    {
        configuration.AddBehavior<IRequestHandler<GetUtilityRequest, ApiResponse>, GetUtilityHandler>(life);
    }
}
