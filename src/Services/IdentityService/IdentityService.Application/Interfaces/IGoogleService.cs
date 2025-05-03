using IdentityService.Application.Dtos.Authentications;

namespace IdentityService.Application.Interfaces;

public interface IGoogleService
{
    Task<GoogleUserInfoDTO?> VerifyGoogleTokenAsync(string idToken, CancellationToken cancellationToken = default);
}
