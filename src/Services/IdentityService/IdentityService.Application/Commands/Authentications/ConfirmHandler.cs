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

            var validate = new ConfirmValidate().Validate(request);
            if (!validate.IsValid)
            {
                var errors = validate.Errors.ToDic();
                logger.LogWarning("Confirmation validation failed for user ID: {UserId}. Errors: {@Errors}",
                    request.UserId, errors);
                return res.SetError(nameof(E000), E000, errors);
            }

            var userKey = CacheKeys.ForEntity<User>(request.UserId);
            var userResult = await cache.TryGetValueAsync<User>(userKey, cancellationToken);
            if (!userResult.Success || userResult.Value == null)
            {
                logger.LogWarning("User not found in cache: {UserId}", request.UserId);
                return res.SetError(nameof(E103), E103);
            }
            var user = userResult.Value;

            if (user.IsActive)
            {
                logger.LogWarning("User is already active: {UserId}", request.UserId);
                return res.SetError(nameof(E106), E106);
            }

            if (await userRepository.IsUsernameExistAsync(user.Username, cancellationToken))
            {
                logger.LogWarning("Username already exists in database: {Username}", user.Username);
                return res.SetError(nameof(E109), E109);
            }

            var codeKey = CacheKeys.ForDto<UserConfirmationDto>(request.UserId);
            var codeResult = await cache.TryGetValueAsync<UserConfirmationDto>(codeKey, cancellationToken);
            if (!codeResult.Success || codeResult.Value == null)
            {
                logger.LogWarning("Confirmation code not found for user: {UserId}", request.UserId);
                return res.SetError(nameof(E104), E104);
            }
            var confirmationDto = codeResult.Value;

            if (confirmationDto.ExpiryDate < DateTime.UtcNow)
            {
                logger.LogWarning("Confirmation code expired for user: {UserId}, Expiry: {ExpiryDate}",
                    request.UserId, confirmationDto.ExpiryDate);
                return res.SetError(nameof(E104), E104);
            }

            if (confirmationDto.AttemptCount == 0)
            {
                logger.LogWarning("Maximum attempts reached for user: {UserId}", request.UserId);
                return res.SetError(nameof(E108), E108);
            }

            if (confirmationDto.ConfirmationCode != request.Code)
            {
                confirmationDto.AttemptCount--;
                await cache.SetAsync(codeKey, confirmationDto, cancellationToken);

                logger.LogWarning("Invalid confirmation code for user: {UserId}. Attempts remaining: {Attempts}",
                    request.UserId, confirmationDto.AttemptCount);
                return res.SetError(nameof(E105), string.Format(E105, confirmationDto.AttemptCount), confirmationDto.AttemptCount);
            }

            user.IsActive = true;

            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                if (!await userRepository.AddAsync(user, cancellationToken))
                {
                    logger.LogError("Failed to save user to database: {UserId}", request.UserId);
                    return res.SetError(nameof(E107), E107);
                }

                scope.Complete();
            }

            logger.LogInformation("Updating cache for confirmed user: {UserId}", request.UserId);
            await cache.RemoveAsync(codeKey, cancellationToken);
            await cache.SetAsync(userKey, user, cancellationToken);

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