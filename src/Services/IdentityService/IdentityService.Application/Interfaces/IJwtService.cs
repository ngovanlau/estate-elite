using System.Security.Claims;

namespace IdentityService.Application.Interfaces;

using Dtos.Authentications;

public interface ITokenService
{
    string GenerateAccessToken(UserDto userDto);
    Task<string> GenerateRefreshTokenAsync(Guid userId, CancellationToken cancellationToken = default);
    Task<bool> ValidateRefreshTokenAsync(Guid userId, string token, CancellationToken cancellationToken = default);
    Task RevokeRefreshTokenAsync(Guid userId, CancellationToken cancellationToken = default);
    ClaimsPrincipal GetClaimsPrincipal(string token);
}
