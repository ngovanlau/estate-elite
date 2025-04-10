using MediatR;

namespace IdentityService.Application.Requests.Authentications;

using SharedKernel.Commons;

public class LoginRequest : IRequest<ApiResponse>
{
    public string? Username;
    public string? Email;
    public required string Password;
}
