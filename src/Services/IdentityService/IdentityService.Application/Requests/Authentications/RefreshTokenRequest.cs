using MediatR;
using SharedKernel.Commons;

namespace IdentityService.Application.Requests.Authentications;

public class RefreshTokenRequest : IRequest<ApiResponse>
{
    public required string RefreshToken { get; set; }
}
