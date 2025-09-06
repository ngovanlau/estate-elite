using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;

namespace IdentityService.Application.Commands.Authentications;

using Dtos.Authentications;
using Interfaces;
using Requests.Authentications;
using Common.Infrastructure.Extensions;
using Common.Application.Responses;
using static SharedKernel.Constants.ErrorCode;

public class LoginHandler(
    IValidator<LoginRequest> validator,
    IUserRepository userRepository,
    ITokenService tokenService,
    IPasswordHasher passwordHasher,
    ILogger<LoginHandler> logger) : IRequestHandler<LoginRequest, ApiResponse>
{
    public async Task<ApiResponse> Handle(LoginRequest request, CancellationToken cancellationToken)
    {
        var res = new ApiResponse();

        //TODO: Rate limit for lock when send most login request 

        try
        {
            var validationResult = await validator.ValidateAsync(request, cancellationToken);
            if (!validationResult.IsValid)
            {
                var errors = validationResult.Errors.ToDic();
                logger.LogWarning("Login validation failed for {Username}. Errors: {@Errors}", request.Username, errors);
                return res.SetError(nameof(E000), E000, errors);
            }

            var userDto = await userRepository.GetUserDtoByUsernameOrEmailAsync(
                request.Username,
                request.Email,
                cancellationToken);

            if (userDto == null)
            {
                logger.LogWarning("User not found - Username: {Username}, Email: {Email}",
                    request.Username, request.Email);
                return res.SetError(nameof(E008), string.Format(E008, "User"));
            }

            if (!passwordHasher.Verify(request.Password, userDto.PasswordHash))
            {
                logger.LogWarning("Invalid password attempt for UserId: {UserId}", userDto.Id);
                return res.SetError(nameof(E114), E114);
            }

            logger.LogInformation("Generating tokens for user {UserId}", userDto.Id);
            var accessToken = tokenService.GenerateAccessToken(userDto);
            await tokenService.RevokeRefreshTokenAsync(userDto.Id, cancellationToken);
            var refreshToken = await tokenService.GenerateRefreshTokenAsync(userDto.Id, cancellationToken);

            logger.LogInformation("Login successful for user {UserId}", userDto.Id);
            return res.SetSuccess(new TokenDto { AccessToken = accessToken, RefreshToken = refreshToken });
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Login failed for {Username}", request.Username);
            return res.SetError(nameof(E000), E000);
        }
    }
}