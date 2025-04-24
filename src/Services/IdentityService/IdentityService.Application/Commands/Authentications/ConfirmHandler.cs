using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using System.Transactions;

namespace IdentityService.Application.Commands.Authentications;

using DistributedCache.Redis;
using Domain.Entities;
using Dtos.Authentications;
using FluentValidation;
using Interfaces;
using Requests.Authentications;
using SharedKernel.Extensions;
using SharedKernel.Responses;
using static SharedKernel.Constants.ErrorCode;

public class ConfirmHandler(
    IValidator<ConfirmRequest> validator,
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

            var validationResult = await validator.ValidateAsync(request, cancellationToken);
            if (!validationResult.IsValid)
            {
                var errors = validationResult.Errors.ToDic();
                logger.LogWarning("Confirmation validation failed for user ID: {UserId}. Errors: {@Errors}",
                    request.UserId, errors);
                return res.SetError(nameof(E000), E000, errors);
            }

            var userKey = CacheKeys.ForEntity<User>(request.UserId);
            var (userSuccess, user) = await cache.TryGetValueAsync<User>(userKey, cancellationToken);
            if (!userSuccess || user == null)
            {
                logger.LogWarning("User not found in cache: {UserId}", request.UserId);
                return res.SetError(nameof(E008), string.Format(E008, "User"));
            }

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

            var codeKey = CacheKeys.ForDto<User, UserConfirmationDto>(request.UserId);
            var (dtoSuccess, confirmationDto) = await cache.TryGetValueAsync<UserConfirmationDto>(codeKey, cancellationToken);
            if (!dtoSuccess || confirmationDto == null)
            {
                logger.LogWarning("Confirmation code not found for user: {UserId}", request.UserId);
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