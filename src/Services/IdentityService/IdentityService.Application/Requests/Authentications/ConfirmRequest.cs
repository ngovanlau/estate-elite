using MediatR;
using SharedKernel.Commons;

namespace IdentityService.Application.Requests.Authentications;

public class ConfirmRequest : IRequest<ApiResponse>
{
    public Guid UserId { get; set; }
    public required string Code { get; set; }
}
