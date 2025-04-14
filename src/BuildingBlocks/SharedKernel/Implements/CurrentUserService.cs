using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using SharedKernel.Enums;
using SharedKernel.Interfaces;

namespace SharedKernel.Implements;

public class CurrentUserService(IHttpContextAccessor accessor) : ICurrentUserService
{
    private readonly IHttpContextAccessor _accessor = accessor;

    private ClaimsPrincipal? User => _accessor.HttpContext?.User;

    public Guid? Id
    {
        get
        {
            var userIdClaim = User?.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userIdClaim))
                return null;

            return Guid.TryParse(userIdClaim, out Guid userId) ? userId : null;
        }
    }

    public string? Email => User?.FindFirstValue(ClaimTypes.Email);

    public string? Username => User?.FindFirstValue(ClaimTypes.Name);

    public UserRole? Role
    {
        get
        {
            var roleClaim = User?.FindFirstValue(ClaimTypes.Role);
            if (string.IsNullOrEmpty(roleClaim))
                return null;

            return Enum.TryParse<UserRole>(roleClaim, out var role) ? role : null;
        }
    }

    public string GetClaimValue(string claimType)
    {
        return User?.FindFirstValue(claimType) ?? string.Empty;
    }

    public List<string> GetClaimValues(string claimType)
    {
        return User?.FindAll(claimType).Select(c => c.Value).ToList() ?? new List<string>();
    }
}