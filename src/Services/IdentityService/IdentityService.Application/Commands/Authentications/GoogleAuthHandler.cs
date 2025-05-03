using AutoMapper;
using FluentValidation;
using IdentityService.Application.Dtos.Authentications;
using IdentityService.Application.Interfaces;
using IdentityService.Application.Requests.Authentications;
using IdentityService.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;
using SharedKernel.Enums;
using SharedKernel.Extensions;
using SharedKernel.Responses;
using static SharedKernel.Constants.ErrorCode;

namespace IdentityService.Application.Commands.Authentications;

public class GoogleAuthHandler(
    IValidator<GoogleAuthRequest> validator,
    IGoogleService googleService,
    ITokenService tokenService,
    IUserRepository userRepository,
    IPasswordHasher passwordHasher,
    IMapper mapper,
    ILogger<GoogleAuthHandler> logger) : IRequestHandler<GoogleAuthRequest, ApiResponse>
{
    public async Task<ApiResponse> Handle(GoogleAuthRequest request, CancellationToken cancellationToken)
    {
        var res = new ApiResponse();

        try
        {
            logger.LogDebug("Validating Google authentication request");
            var validationResult = await validator.ValidateAsync(request, cancellationToken);
            if (!validationResult.IsValid)
            {
                logger.LogWarning("Validation failed for Google auth request - Errors: {Errors}",
                    validationResult.Errors);
                return res.SetError(nameof(E000), E000, validationResult.Errors.ToDic());
            }

            logger.LogDebug("Verifying Google token");
            var googleUser = await googleService.VerifyGoogleTokenAsync(request.IdToken, cancellationToken);
            if (googleUser is null)
            {
                logger.LogWarning("Invalid Google token provided");
                return res.SetError(nameof(E103), E103);
            }

            UserDto? userDto;
            if (!await userRepository.IsEmailExistAsync(googleUser.Email, cancellationToken))
            {
                logger.LogInformation("Creating new user from Google account {GoogleId}", googleUser.Id);
                var user = User.Create(
                    googleUser.Email,
                    googleUser.Email,
                    googleUser.Name,
                    passwordHasher.Hash(StringExtension.GenerateHashId(8)),
                    UserRole.Buyer);

                user.Avatar = googleUser.Picture;
                user.GoogleId = googleUser.Id;
                user.IsActive = true;

                if (!await userRepository.AddEntityAsync(user, cancellationToken))
                {
                    logger.LogError("Failed to create user with Google ID {GoogleId}", googleUser.Id);
                    return res.SetError(nameof(E011), string.Format(E011, "User"));
                }

                userDto = mapper.Map<UserDto>(user);
                logger.LogDebug("Created new user {UserId} from Google account", user.Id);
            }
            else
            {
                logger.LogDebug("Existing Google user found with email {Email}", googleUser.Email);
                userDto = await userRepository.GetUserDtoByUsernameOrEmailAsync(
                    string.Empty,
                    googleUser.Email,
                    cancellationToken);
            }

            if (userDto is null)
            {
                logger.LogWarning("User not found for Google email {Email}", googleUser.Email);
                return res.SetError(nameof(E115), E115);
            }

            logger.LogDebug("Generating tokens for user {UserId}", userDto.Id);
            var accessToken = tokenService.GenerateAccessToken(userDto);
            await tokenService.RevokeRefreshTokenAsync(userDto.Id, cancellationToken);
            var refreshToken = await tokenService.GenerateRefreshTokenAsync(userDto.Id, cancellationToken);

            logger.LogInformation("Google authentication successful for user {UserId}", userDto.Id);
            return res.SetSuccess(new TokenDto
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken
            });
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Google authentication failed - {Message}", ex.Message);
            return res.SetError(nameof(E000), E000);
        }
    }
}