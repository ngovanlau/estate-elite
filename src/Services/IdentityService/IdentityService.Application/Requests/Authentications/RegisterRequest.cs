using MediatR;
using System.Text.Json.Serialization;

namespace IdentityService.Application.Requests.Authentications;

using SharedKernel.Commons;
using SharedKernel.Enums;

public class RegisterRequest : IRequest<ApiResponse>
{
    public required string Username { get; set; }
    public required string Email { get; set; }
    public required string FullName { get; set; }
    public required string Password { get; set; }
    public required string ConfirmationPassword { get; set; }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public UserRole Role { get; set; }
}
