using MediatR;
using Microsoft.Extensions.Logging;
using System.Transactions;
using Microsoft.Extensions.Caching.Distributed;

namespace IdentityService.Application.Commands.Authentications;

using DistributedCache.Redis;
using Domain.Entities;
using Dtos.Authentications;
using Interfaces;
using Requests.Authentications;
using SharedKernel.Commons;
using SharedKernel.Extensions;
using Validates.Authentications;
using static SharedKernel.Constants.ErrorCode;

public class ConfirmHandler(
    IUserRepository userRepository,
    IDistributedCache cache,
    ILogger<ConfirmHandler> logger) : IRequestHandler<ConfirmRequest, ApiResponse>
{
    public async Task<ApiResponse> Handle(ConfirmRequest request, CancellationToken cancellationToken)
    {
        var res = new ApiResponse();

        try
        {
            logger.LogInformation("Processing confirmation request for user ID: {UserId}", request.UserId);

            // Validate input
            var validate = new ConfirmValidate().Validate(request);
            if (!validate.IsValid)
            {
                var errors = validate.Errors.ToDic();
                logger.LogWarning("Confirmation validation failed for user ID: {UserId}. Errors: {@Errors}",
                    request.UserId, errors);
                return res.SetError(nameof(E000), E000, errors);
            }

            // Get user from cache
            var userKey = CacheKeys.ForEntity<User>(request.UserId);
            var userResult = await cache.TryGetValueAsync<User>(userKey);
            if (!userResult.Success || userResult.Value == null)
            {
                logger.LogWarning("User not found in cache: {UserId}", request.UserId);
                return res.SetError(nameof(E103), E103);
            }

            var user = userResult.Value;

            // Check if user is already active
            if (user.IsActive)
            {
                logger.LogWarning("User is already active: {UserId}", request.UserId);
                return res.SetError(nameof(E106), E106);
            }

            // Check if username already exists
            if (await userRepository.IsUsernameExist(user.Username).ConfigureAwait(false))
            {
                logger.LogWarning("Username already exists in database: {Username}", user.Username);
                return res.SetError(nameof(E109), E109);
            }

            // Get confirmation code from cache
            var codeKey = CacheKeys.ForDto<UserConfirmationDto>(request.UserId);
            var codeResult = await cache.TryGetValueAsync<UserConfirmationDto>(codeKey);
            if (!codeResult.Success || codeResult.Value == null)
            {
                logger.LogWarning("Confirmation code not found for user: {UserId}", request.UserId);
                return res.SetError(nameof(E104), E104);
            }

            var confirmationDto = codeResult.Value;

            // Check if code is expired
            if (confirmationDto.ExpiryDate < DateTime.UtcNow)
            {
                logger.LogWarning("Confirmation code expired for user: {UserId}, Expiry: {ExpiryDate}",
                    request.UserId, confirmationDto.ExpiryDate);
                return res.SetError(nameof(E104), E104);
            }

            // Check if maximum attempts reached
            if (confirmationDto.AttemptCount == 0)
            {
                logger.LogWarning("Maximum attempts reached for user: {UserId}", request.UserId);
                return res.SetError(nameof(E108), E108);
            }

            // Validate confirmation code
            if (confirmationDto.ConfirmationCode != request.Code)
            {
                // Decrement attempt count
                confirmationDto.AttemptCount--;
                await cache.SetAsync<UserConfirmationDto>(codeKey, confirmationDto, cancellationToken);

                logger.LogWarning("Invalid confirmation code for user: {UserId}. Attempts remaining: {Attempts}",
                    request.UserId, confirmationDto.AttemptCount);
                return res.SetError(nameof(E105), string.Format(E105, confirmationDto.AttemptCount), confirmationDto.AttemptCount);
            }

            // Activate user
            user.IsActive = true;

            // Save user to database using transaction
            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                if (!await userRepository.Add(user).ConfigureAwait(false))
                {
                    logger.LogError("Failed to save user to database: {UserId}", request.UserId);
                    return res.SetError(nameof(E107), E107);
                }

                scope.Complete();
            }

            // Update cache
            logger.LogInformation("Updating cache for confirmed user: {UserId}", request.UserId);
            await cache.RemoveAsync(codeKey, cancellationToken).ConfigureAwait(false);
            await cache.SetAsync(userKey, user, cancellationToken).ConfigureAwait(false);

            logger.LogInformation("User confirmation successful: {UserId}", request.UserId);
            return res.SetSuccess(true);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while confirming user: {UserId}", request.UserId);
            return res.SetError(nameof(E000), "An unexpected error occurred during confirmation process.");
        }
    }
}