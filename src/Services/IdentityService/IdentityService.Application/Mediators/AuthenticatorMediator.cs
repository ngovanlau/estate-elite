using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace IdentityService.Application.Mediators;

using SharedKernel.Commons;
using Commands.Authentications;
using Requests.Authentications;

public static class AuthenticatorMediator
{
    public static void AddAuthenticatorMediator(this MediatRServiceConfiguration configuration, ServiceLifetime life = ServiceLifetime.Scoped)
    {
        configuration.AddBehavior<IRequestHandler<RegisterRequest, ApiResponse>, RegisterHandler>(life);
        configuration.AddBehavior<IRequestHandler<ConfirmRequest, ApiResponse>, ConfirmHandler>(life);
        configuration.AddBehavior<IRequestHandler<LoginRequest, ApiResponse>, LoginHandler>(life);
        configuration.AddBehavior<IRequestHandler<ResendCodeRequest, ApiResponse>, ResendCodeHandler>(life);
        configuration.AddBehavior<IRequestHandler<RefreshTokenRequest, ApiResponse>, RefreshTokenHandler>(life);
    }
}
