using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Security.Claims;

namespace IdentityService.Application.Commands.Authentications;

using Dtos.Authentications;
using Interfaces;
using Requests.Authentications;
using Common.Infrastructure.Extensions;
using Common.Application.Responses;
using static SharedKernel.Constants.ErrorCode;

public class RefreshTokenHandler(
    IValidator<RefreshTokenRequest> validator,
    IUserRepository userRepository,
    ITokenService tokenService,
    ILogger<RefreshTokenHandler> logger) : IRequestHandler<RefreshTokenRequest, ApiResponse>
{
    public async Task<ApiResponse> Handle(RefreshTokenRequest request, CancellationToken cancellationToken)
    {
        var res = new ApiResponse();
        logger.LogInformation("Processing refresh token request");

        try
        {
            var validationResult = await validator.ValidateAsync(request, cancellationToken);
            if (!validationResult.IsValid)
            {
                var errors = validationResult.Errors.ToDic();
                logger.LogWarning("Refresh token request validation failed: {ValidationErrors}", errors);
                return res.SetError(nameof(E000), E000, errors);
            }

            var principal = tokenService.GetClaimsPrincipal(request.AccessToken);
            if (principal is null)
            {
                logger.LogWarning("Invalid access token: Unable to extract claims principal");
                return res.SetError(nameof(E117), E117);
            }

            var userIdClaim = principal.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!Guid.TryParse(userIdClaim, out var userId))
            {
                logger.LogWarning("Invalid access token: User ID claim missing or malformed");
                return res.SetError(nameof(E117), E117);
            }

            logger.LogDebug("Validating refresh token for user {UserId}", userId);
            if (!await tokenService.ValidateRefreshTokenAsync(userId, request.RefreshToken, cancellationToken))
            {
                logger.LogWarning("Invalid or expired refresh token for user {UserId}", userId);
                return res.SetError(nameof(E118), E118);
            }

            logger.LogInformation("Revoking old refresh token for user {UserId}", userId);
            await tokenService.RevokeRefreshTokenAsync(userId, cancellationToken);

            var username = principal.FindFirstValue(ClaimTypes.Name);
            var email = principal.FindFirstValue(ClaimTypes.Email);
            logger.LogDebug("Retrieving user details for username: {Username}, email: {Email}", username, email);

            var userDto = await userRepository.GetUserDtoByUsernameOrEmailAsync(username, email, cancellationToken);
            if (userDto is null)
            {
                logger.LogWarning("User not found for username: {Username}, email: {Email}", username, email);
                return res.SetError(nameof(E008), string.Format(E008, "Username or email"));
            }

            logger.LogInformation("Generating new token pair for user {UserId}", userId);
            var data = new TokenDto
            {
                AccessToken = tokenService.GenerateAccessToken(userDto),
                RefreshToken = await tokenService.GenerateRefreshTokenAsync(userId, cancellationToken)
            };

            logger.LogInformation("Token refresh completed successfully for user {UserId}", userId);
            return res.SetSuccess(data);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unhandled exception occurred during token refresh");
            return res.SetError(nameof(E000), E000);
        }
    }
}
