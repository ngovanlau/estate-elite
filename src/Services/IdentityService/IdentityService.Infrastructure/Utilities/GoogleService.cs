using IdentityService.Application.Dtos.Authentications;
using IdentityService.Application.Interfaces;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SharedKernel.Settings;
using Google.Apis.Auth;

namespace IdentityService.Infrastructure.Utilities;

public class GoogleService(
    IOptions<GoogleSetting> options,
    ILogger<GoogleService> logger) : IGoogleService
{
    private readonly GoogleSetting _setting = options.Value;

    public async Task<GoogleUserInfoDTO?> VerifyGoogleTokenAsync(string idToken, CancellationToken cancellationToken = default)
    {
        try
        {
            var payload = await GoogleJsonWebSignature.ValidateAsync(
                idToken,
                new GoogleJsonWebSignature.ValidationSettings
                {
                    Audience = new[] { _setting.ClientId },
                }
            );

            return new GoogleUserInfoDTO
            {
                Id = payload.Subject,
                Email = payload.Email,
                Name = payload.Name,
                Picture = payload.Picture
            };
        }
        catch (InvalidJwtException ex)
        {
            logger.LogWarning("Invalid Google ID token: {Message}", ex.Message);
            return null;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error verifying Google token");
            return null;
        }
    }
}
