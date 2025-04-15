using MediatR;

namespace IdentityService.Application.Requests.Users;

using SharedKernel.Commons;

public class CurrentUserRequest : IRequest<ApiResponse>
{
}