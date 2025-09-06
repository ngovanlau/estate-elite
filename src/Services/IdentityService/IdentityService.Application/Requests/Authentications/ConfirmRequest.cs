using MediatR;
using Common.Application.Responses;

namespace IdentityService.Application.Requests.Authentications;

public class ConfirmRequest : IRequest<ApiResponse>
{
    public Guid UserId { get; set; }
    public required string Code { get; set; }
}
