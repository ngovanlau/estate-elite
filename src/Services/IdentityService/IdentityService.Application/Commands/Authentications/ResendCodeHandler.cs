using Caching.Configuration;
using Caching.Services;
using Common.Application.Responses;
using Common.Application.Settings;
using EventBus.Abstraction.Interfaces;
using EventBus.RabbitMQ.Events;
using FluentValidation;
using IdentityService.Application.Dtos.Authentications;
using IdentityService.Application.Interfaces;
using IdentityService.Application.Requests.Authentications;
using IdentityService.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using static Common.Domain.Constants.ErrorCode;

namespace IdentityService.Application.Commands.Authentications;

public class ResendCodeHandler(
    IValidator<ResendCodeRequest> validator,
    IDistributedCache cache,
    ILogger<ResendCodeHandler> logger,
    IConfirmationCodeGenerator generator,
    IEventBus eventBus,
    IOptions<ConfirmationCodeSetting> options) : IRequestHandler<ResendCodeRequest, ApiResponse>
{
    private readonly ConfirmationCodeSetting confirmationCodeSetting = options.Value;

    public async Task<ApiResponse> Handle(ResendCodeRequest request, CancellationToken cancellationToken)
    {
        var res = new ApiResponse();

        try
        {
            logger.LogInformation("Starting resend confirmation code process for user ID: {UserId}", request.UserId);

            var validationResult = await validator.ValidateAsync(request, cancellationToken);
            if (!validationResult.IsValid)
            {
                var errors = validationResult.Errors;
                logger.LogWarning("Validation failed for user ID {UserId}. Errors: {Errors}", request.UserId, errors);
                return res.SetError(nameof(E000), E000, errors);
            }

            var userId = request.UserId;
            var codeKey = CacheKeys.ForDto<User, UserConfirmationDto>(userId);

            logger.LogDebug("Checking for existing confirmation code for user ID: {UserId}", userId);
            var (success, _) = await cache.TryGetValueAsync<UserConfirmationDto>(codeKey, cancellationToken);
            if (success)
            {
                logger.LogWarning("Active confirmation code already exists for user ID: {UserId}", userId);
                return res.SetError(nameof(E116), E116);
            }

            var expiryTime = TimeSpan.FromMinutes(confirmationCodeSetting.ExpirationTimeInMinutes);
            var confirmationCode = generator.GenerateCode();
            var confirmationDto = new UserConfirmationDto
            {
                UserId = userId,
                ConfirmationCode = confirmationCode,
                AttemptCount = confirmationCodeSetting.MaximumAttempts
            };

            logger.LogDebug("Storing new confirmation code for user ID: {UserId} (Expires in {Minutes} minutes)",
                userId, confirmationCodeSetting.ExpirationTimeInMinutes);
            var options = new DistributedCacheEntryOptions().SetAbsoluteExpiration(expiryTime);
            await cache.SetAsync(codeKey, confirmationDto, options, cancellationToken);

            var userKey = CacheKeys.ForEntity<User>(userId);
            logger.LogDebug("Retrieving user details for ID: {UserId}", userId);
            var (UserSuccess, user) = await cache.TryGetValueAsync<User>(userKey, cancellationToken);
            if (!UserSuccess || user is null || user.IsActive)
            {
                logger.LogError("User is already active or not found in cache for ID: {UserId}", userId);
                return res.SetError(nameof(E115), E115);
            }

            logger.LogInformation("Publishing confirmation code event for user {UserId} (Email: {Email})",
                userId, user.Email);
            var integrationEvent = new SendConfirmationCodeEvent(user.Email, user.FullName, confirmationCode, expiryTime);
            await eventBus.PublishAsync(integrationEvent);

            logger.LogInformation("Successfully processed resend code request for user ID: {UserId}", userId);
            return res.SetSuccess(true);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unexpected error occurred while processing resend code for user ID: {UserId}",
                request.UserId);
            return res.SetError(nameof(E000), E000);
        }
    }
}
