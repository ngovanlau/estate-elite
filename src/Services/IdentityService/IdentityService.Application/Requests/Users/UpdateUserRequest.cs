using MediatR;

namespace IdentityService.Application.Requests.Users;

using Common.Application.Responses;

public class UpdateUserRequest : IRequest<ApiResponse>
{
    public string? Email { get; set; }
    public string? FullName { get; set; }
    public string? Phone { get; set; }
    public string? Address { get; set; }
}