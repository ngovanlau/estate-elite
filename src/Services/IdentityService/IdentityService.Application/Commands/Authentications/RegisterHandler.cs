using DistributedCache.Redis;
using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace IdentityService.Application.Commands.Authentications;

using Domain.Entities;
using Dtos.Authentications;
using EventBus.Infrastructures.Interfaces;
using EventBus.RabbitMQ.Events;
using Interfaces;
using Requests.Authentications;
using SharedKernel.Commons;
using SharedKernel.Constants;
using SharedKernel.Extensions;
using Validates.Authentications;
using static SharedKernel.Constants.ErrorCode;

public class RegisterHandler(
    IUserRepository repository,
    IPasswordHasher hasher,
    IDistributedCache cache,
    IConfirmationCodeGenerator generator,
    IEventBus eventBus,
    ILogger<RegisterHandler> logger,
    IOptions<ConfirmationCodeSetting> options) : IRequestHandler<RegisterRequest, ApiResponse>
{
    private ConfirmationCodeSetting confirmationCodeSetting = options.Value;

    public async Task<ApiResponse> Handle(RegisterRequest request, CancellationToken cancellationToken)
    {
        var res = new ApiResponse();

        try
        {
            logger.LogInformation("Starting registration process for username: {Username}", request.Username);

            // Validate
            var validate = new RegisterValidate().Validate(request);
            if (!validate.IsValid)
            {
                var errors = validate.Errors.ToDic();
                logger.LogWarning("Registration validation failed for username: {Username}. Errors: {@Errors}",
                    request.Username, errors);
                return res.SetError(nameof(E000), E000, errors);
            }

            var username = request.Username;
            var email = request.Email;
            var fullname = request.Fullname;
            var password = hasher.Hash(request.Password);

            logger.LogDebug("Checking username existence: {Username}", username);
            if (await repository.IsUsernameExist(username))
            {
                logger.LogWarning("Username {Username} already exists", username);
                return res.SetError(nameof(E101), E101);
            }

            logger.LogDebug("Checking email existence: {Email}", email);
            if (await repository.IsEmailExist(email))
            {
                logger.LogWarning("Email {Email} is already registered", email);
                return res.SetError(nameof(E102), E102);
            }

            var user = User.Create(username, email, fullname, password, request.Role);
            logger.LogInformation("User created with ID: {UserId}", user.Id);

            logger.LogDebug("Caching user {UserId} temporarily", user.Id);
            await cache.SetAsync(CacheKeys.ForEntity<User>(user.Id), user, cancellationToken);

            var expiryTime = TimeSpan.FromMinutes(confirmationCodeSetting.ExpirationTimeInMinutes);
            var expiryDate = DateTime.UtcNow.Add(expiryTime);
            var confirmationCode = generator.GenerateCode();
            var userConfirmationDto = new UserConfirmationDto(
                user.Id,
                confirmationCode,
                expiryDate,
                confirmationCodeSetting.MaximumAttempts);

            logger.LogDebug("Caching confirmation code for user {UserId} (Expiry: {ExpiryTime})",
                user.Id, expiryTime);
            await cache.SetAsync(CacheKeys.ForDto<UserConfirmationDto>(
                userConfirmationDto.UserId),
                userConfirmationDto,
                cancellationToken);

            logger.LogInformation("Publishing confirmation event for user {UserId}", user.Id);
            var integrationEvent = new SendConfirmationCodeEvent(email, fullname, confirmationCode, expiryTime);
            await eventBus.PublishAsync(integrationEvent);

            logger.LogInformation("Registration completed successfully for user {UserId}", user.Id);
            return res.SetSuccess(user.Id);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while processing registration for username: {Username}",
                request.Username);
            return res.SetError(nameof(E000), "An unexpected error occurred during registration.");
        }
    }
}