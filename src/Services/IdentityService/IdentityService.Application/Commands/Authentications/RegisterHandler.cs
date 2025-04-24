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
using FluentValidation;
using Interfaces;
using Requests.Authentications;
using SharedKernel.Extensions;
using SharedKernel.Responses;
using SharedKernel.Settings;
using static SharedKernel.Constants.ErrorCode;

public class RegisterHandler(
    IValidator<RegisterRequest> validator,
    IUserRepository repository,
    IPasswordHasher hasher,
    IDistributedCache cache,
    IConfirmationCodeGenerator generator,
    IEventBus eventBus,
    ILogger<RegisterHandler> logger,
    IOptions<ConfirmationCodeSetting> options) : IRequestHandler<RegisterRequest, ApiResponse>
{
    private readonly ConfirmationCodeSetting confirmationCodeSetting = options.Value;

    public async Task<ApiResponse> Handle(RegisterRequest request, CancellationToken cancellationToken)
    {
        var res = new ApiResponse();

        try
        {
            logger.LogInformation("Starting registration process for username: {Username}", request.Username);

            // Validate
            var validationResult = await validator.ValidateAsync(request, cancellationToken);
            if (!validationResult.IsValid)
            {
                var errors = validationResult.Errors.ToDic();
                logger.LogWarning("Registration validation failed for username: {Username}. Errors: {@Errors}",
                    request.Username, errors);
                return res.SetError(nameof(E000), E000, errors);
            }

            var username = request.Username;
            var email = request.Email;
            var fullname = request.FullName;
            var password = request.Password;
            var confirmationPassword = request.ConfirmationPassword;

            if (!(password == confirmationPassword))
            {
                logger.LogWarning("Password and confirmation password do not match. User: {Username}", username);
                return res.SetError(nameof(E110), E110);
            }
            password = hasher.Hash(password);

            logger.LogDebug("Checking username existence: {Username}", username);
            if (await repository.IsUsernameExistAsync(username, cancellationToken))
            {
                logger.LogWarning("Username {Username} already exists", username);
                return res.SetError(nameof(E101), E101);
            }

            logger.LogDebug("Checking email existence: {Email}", email);
            if (await repository.IsEmailExistAsync(email))
            {
                logger.LogWarning("Email {Email} is already registered", email);
                return res.SetError(nameof(E102), E102);
            }

            var user = User.Create(username, email, fullname, password, request.Role);
            logger.LogInformation("User created with ID: {UserId}", user.Id);

            logger.LogDebug("Caching user {UserId} temporarily", user.Id);
            await cache.SetAsync(CacheKeys.ForEntity<User>(user.Id), user, cancellationToken);

            var expiryTime = TimeSpan.FromMinutes(confirmationCodeSetting.ExpirationTimeInMinutes);
            var options = new DistributedCacheEntryOptions().SetAbsoluteExpiration(expiryTime);
            var userConfirmationDto = new UserConfirmationDto
            {
                UserId = user.Id,
                ConfirmationCode = generator.GenerateCode(),
                AttemptCount = confirmationCodeSetting.MaximumAttempts
            };

            logger.LogDebug("Caching confirmation code for user {UserId} (Expiry: {ExpiryTime})",
                user.Id, expiryTime);
            var codeKey = CacheKeys.ForDto<User, UserConfirmationDto>(userConfirmationDto.UserId);
            await cache.SetAsync(codeKey, userConfirmationDto, options, cancellationToken);

            logger.LogInformation("Publishing confirmation event for user {UserId}", user.Id);
            var integrationEvent = new SendConfirmationCodeEvent(email, fullname, userConfirmationDto.ConfirmationCode, expiryTime);
            await eventBus.PublishAsync(integrationEvent);

            logger.LogInformation("Registration completed successfully for user {UserId}", user.Id);
            return res.SetSuccess(user.Id);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while processing registration for username: {Username}",
                request.Username);
            return res.SetError(nameof(E000), E000);
        }
    }
}