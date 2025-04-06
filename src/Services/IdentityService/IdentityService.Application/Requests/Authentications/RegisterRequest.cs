using MediatR;

namespace IdentityService.Application.Requests.Authentications;

using SharedKernel.Commons;

public class RegisterRequest : IRequest<ApiResponse>
{
    public string? Username { get; set; }
    public string? Email { get; set; }
    public string? Fullname { get; set; }
    public string? Password { get; set; }
}
