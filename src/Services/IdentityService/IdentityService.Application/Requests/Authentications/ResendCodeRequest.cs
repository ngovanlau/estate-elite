using MediatR;
using Common.Application.Responses;

namespace IdentityService.Application.Requests.Authentications;

public class ResendCodeRequest : IRequest<ApiResponse>
{
    public Guid UserId { get; set; }
}
