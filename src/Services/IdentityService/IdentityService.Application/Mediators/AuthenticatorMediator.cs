using MediatR;
using Microsoft.Extensions.DependencyInjection;
using IdentityService.Application.Commands.Authentications;
using IdentityService.Application.Requests.Authentications;
using Common.Application.Responses;

namespace IdentityService.Application.Mediators;

public static class AuthenticatorMediator
{
    public static void AddAuthenticatorMediator(this MediatRServiceConfiguration configuration, ServiceLifetime life = ServiceLifetime.Scoped)
    {
        configuration.AddBehavior<IRequestHandler<RegisterRequest, ApiResponse>, RegisterHandler>(life);
        configuration.AddBehavior<IRequestHandler<ConfirmRequest, ApiResponse>, ConfirmHandler>(life);
        configuration.AddBehavior<IRequestHandler<LoginRequest, ApiResponse>, LoginHandler>(life);
        configuration.AddBehavior<IRequestHandler<ResendCodeRequest, ApiResponse>, ResendCodeHandler>(life);
        configuration.AddBehavior<IRequestHandler<RefreshTokenRequest, ApiResponse>, RefreshTokenHandler>(life);
        configuration.AddBehavior<IRequestHandler<GoogleAuthRequest, ApiResponse>, GoogleAuthHandler>(life);
    }
}
