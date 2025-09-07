using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace IdentityService.Application.Mediators;

using Commands.Users;
using Common.Application.Responses;
using Queries.Users;
using Requests.Users;

public static class UserMediator
{
    public static void AddUserMediator(this MediatRServiceConfiguration configuration, ServiceLifetime life = ServiceLifetime.Scoped)
    {
        configuration.AddBehavior<IRequestHandler<UploadRequest, ApiResponse>, UploadHandler>(life);
        configuration.AddBehavior<IRequestHandler<CurrentUserRequest, ApiResponse>, CurrentUserHandler>(life);
        configuration.AddBehavior<IRequestHandler<UpdateUserRequest, ApiResponse>, UpdateUserHandler>(life);
        configuration.AddBehavior<IRequestHandler<UpdateSellerProfileRequest, ApiResponse>, UpdateSellerProfileHandler>(life);
    }
}
