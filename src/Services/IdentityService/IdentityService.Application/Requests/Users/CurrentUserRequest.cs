using MediatR;

namespace IdentityService.Application.Requests.Users;

using SharedKernel.Responses;

public class CurrentUserRequest : IRequest<ApiResponse>
{
}