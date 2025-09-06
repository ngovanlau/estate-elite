using MediatR;

namespace IdentityService.Application.Requests.Authentications;

using Common.Domain.Enums;
using Common.Application.Responses;

public class RegisterRequest : IRequest<ApiResponse>
{
    public required string Username { get; set; }
    public required string Email { get; set; }
    public required string FullName { get; set; }
    public required string Password { get; set; }
    public required string ConfirmationPassword { get; set; }
    public UserRole Role { get; set; }
}
