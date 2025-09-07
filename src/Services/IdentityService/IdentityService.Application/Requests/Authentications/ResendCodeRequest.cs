using Common.Application.Responses;
using MediatR;

namespace IdentityService.Application.Requests.Authentications;

public class ResendCodeRequest : IRequest<ApiResponse>
{
    public Guid UserId { get; set; }
}
