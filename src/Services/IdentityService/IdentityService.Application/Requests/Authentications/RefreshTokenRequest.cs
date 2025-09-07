using Common.Application.Responses;
using MediatR;

namespace IdentityService.Application.Requests.Authentications;

public class RefreshTokenRequest : IRequest<ApiResponse>
{
    public required string AccessToken { get; set; }
    public required string RefreshToken { get; set; }
}
