using MediatR;
using SharedKernel.Responses;

namespace IdentityService.Application.Requests.Authentications;

public class RefreshTokenRequest : IRequest<ApiResponse>
{
    public required string AccessToken { get; set; }
    public required string RefreshToken { get; set; }
}
