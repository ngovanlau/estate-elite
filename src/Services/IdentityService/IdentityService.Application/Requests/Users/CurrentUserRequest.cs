using MediatR;

namespace IdentityService.Application.Requests.Users;

using Common.Application.Responses;

public class CurrentUserRequest : IRequest<ApiResponse>
{
}