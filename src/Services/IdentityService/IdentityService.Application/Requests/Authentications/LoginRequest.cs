using MediatR;

namespace IdentityService.Application.Requests.Authentications;

using SharedKernel.Commons;

public class LoginRequest : IRequest<ApiResponse>
{
    public string? Username { get; set; }
    public string? Email { get; set; }
    public required string Password { get; set; }
}
