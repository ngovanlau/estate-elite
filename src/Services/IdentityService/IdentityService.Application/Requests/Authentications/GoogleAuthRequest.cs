using MediatR;
using Common.Application.Responses;

namespace IdentityService.Application.Requests.Authentications;

public class GoogleAuthRequest : IRequest<ApiResponse>
{
    public required string IdToken { get; set; }
}
