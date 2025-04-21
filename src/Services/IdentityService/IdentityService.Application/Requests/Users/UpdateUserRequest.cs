using MediatR;

namespace IdentityService.Application.Requests.Users;

using SharedKernel.Commons;

public class UpdateUserRequest : IRequest<ApiResponse>
{
    public string? Email { get; set; }
    public string? FullName { get; set; }
    public string? Phone { get; set; }
    public string? Address { get; set; }
}