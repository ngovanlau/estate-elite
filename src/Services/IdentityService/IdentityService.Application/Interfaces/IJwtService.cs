using System.Security.Claims;
using IdentityService.Domain.Entities;

namespace IdentityService.Application.Interfaces;

public interface ITokenService
{
    string GenerateAccessToken(User user);
    Task<string> GenerateRefreshTokenAsync(Guid userId);
    Task<bool> ValidateRefreshTokenAsync(Guid userId, string token);
    Task RevokeRefreshTokenAsync(Guid userId);
    ClaimsPrincipal GetClaimsPrincipal(string token);
}
