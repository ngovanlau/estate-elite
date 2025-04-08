using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Security.Cryptography;
using Microsoft.Extensions.Caching.Distributed;

namespace IdentityService.Infrastructure.Utilities;

using Application.Interfaces;
using DistributedCache.Redis;
using Domain.Entities;
using Domain.Models;
using Microsoft.Extensions.Logging;
using SharedKernel.Constants;

public class TokenService : ITokenService
{
    private readonly JwtSetting _setting;
    private readonly SymmetricSecurityKey _securityKey;
    private readonly SigningCredentials _credentials;
    private readonly JwtSecurityTokenHandler _tokenHandler;
    private readonly IDistributedCache _cache;
    private readonly SHA512 _sha512;
    private readonly ILogger<TokenService> _logger;

    public TokenService(IOptions<JwtSetting> options, IDistributedCache cache, ILogger<TokenService> logger)
    {
        _setting = options.Value;
        _securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_setting.SecretKey));
        _credentials = new SigningCredentials(_securityKey, SecurityAlgorithms.HmacSha512Signature);
        _tokenHandler = new JwtSecurityTokenHandler();
        _cache = cache;
        _sha512 = SHA512.Create();
        _logger = logger;
    }

    public string GenerateAccessToken(User user)
    {
        ArgumentNullException.ThrowIfNull(user);

        var now = DateTime.UtcNow;
        var expires = now.AddMinutes(_setting.ExpirationInMinutes);

        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(JwtRegisteredClaimNames.Email, user.Email),
            new Claim(JwtRegisteredClaimNames.Sub, user.Username),
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.Username),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Role, user.Role.ToString()),
            new Claim("FullName", user.Fullname)
        };

        var token = new JwtSecurityToken(
            issuer: _setting.Issuer,
            audience: _setting.Audience,
            claims: claims,
            notBefore: now,
            expires: expires,
            signingCredentials: _credentials
        );

        return _tokenHandler.WriteToken(token);
    }

    public async Task<string> GenerateRefreshTokenAsync(Guid userId)
    {
        try
        {
            var pepper = RandomNumberGenerator.GetBytes(64);
            var hash = _sha512.ComputeHash(pepper);

            var refreshToken = new RefreshToken
            {
                UserId = userId,
                Token = Convert.ToBase64String(hash)
            };

            var cacheKey = CacheKeys.ForRefreshToken(userId);
            var options = new DistributedCacheEntryOptions()
                .SetSlidingExpiration(TimeSpan.FromMinutes(_setting.RefreshTokenSlidingExpirationInMinutes))
                .SetAbsoluteExpiration(TimeSpan.FromDays(_setting.RefreshTokenExpirationInDays));
            await _cache.SetAsync(cacheKey, refreshToken, options);

            return refreshToken.Token;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception occurred while creating refresh token for user {UserId}", userId);
            throw;
        }
    }

    public async Task<bool> ValidateRefreshTokenAsync(Guid userId, string token)
    {
        try
        {
            var cacheKey = CacheKeys.ForRefreshToken(userId);
            var (Success, Value) = await _cache.TryGetValueAsync<RefreshToken>(cacheKey);

            if (!Success || Value is null)
            {
                return false;
            }

            return string.Equals(token, Value.Token, StringComparison.Ordinal);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception occurred while validating refresh token for user {UserId}", userId);
            throw;
        }
    }

    public async Task RevokeRefreshTokenAsync(Guid userId)
    {
        try
        {
            var cacheKey = CacheKeys.ForRefreshToken(userId);
            await _cache.RemoveAsync(cacheKey);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error revoking refresh token for user {UserId}", userId);
            throw;
        }
    }

    public ClaimsPrincipal GetClaimsPrincipal(string token)
    {
        var tokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = _setting.Issuer,
            ValidAudience = _setting.Audience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_setting.SecretKey)),
            ValidateLifetime = false
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken securityToken);

        if (securityToken is not JwtSecurityToken jwtSecurityToken ||
            !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha512Signature, StringComparison.InvariantCultureIgnoreCase))
        {
            throw new SecurityTokenException("Invalid token");
        }

        return principal;
    }
}