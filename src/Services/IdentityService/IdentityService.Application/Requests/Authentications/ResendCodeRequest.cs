using MediatR;
using SharedKernel.Responses;

namespace IdentityService.Application.Requests.Authentications;

public class ResendCodeRequest : IRequest<ApiResponse>
{
    public Guid UserId { get; set; }
}
