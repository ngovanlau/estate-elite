using Common.Application.Responses;
using MediatR;

namespace IdentityService.Application.Requests.Authentications;

public class ConfirmRequest : IRequest<ApiResponse>
{
    public Guid UserId { get; set; }
    public required string Code { get; set; }
}
