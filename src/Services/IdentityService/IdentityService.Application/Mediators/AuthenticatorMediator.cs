using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace IdentityService.Application.Mediators;

using SharedKernel.Commons;
using Commands.Authentications;
using IdentityService.Application.Requests.Authentications;

public static class AuthenticatorMediator
{
    public static void AddAuthenticatorMediator(this MediatRServiceConfiguration configuration, ServiceLifetime life = ServiceLifetime.Scoped)
    {
        configuration.AddBehavior<IRequestHandler<RegisterRequest, ApiResponse>, RegisterHandler>(life);
    }
}
