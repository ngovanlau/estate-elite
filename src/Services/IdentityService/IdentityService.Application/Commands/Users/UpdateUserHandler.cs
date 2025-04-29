using AutoMapper;
using DistributedCache.Redis;
using FluentValidation;
using IdentityService.Application.Dtos.Users;
using IdentityService.Application.Interfaces;
using IdentityService.Application.Requests.Users;
using IdentityService.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using SharedKernel.Extensions;
using SharedKernel.Interfaces;
using SharedKernel.Responses;
using static SharedKernel.Constants.ErrorCode;

namespace IdentityService.Application.Commands.Users;

public class UpdateUserHandler(
    ICurrentUserService currentUserService,
    IValidator<UpdateUserRequest> validator,
    IUserRepository userRepository,
    IDistributedCache cache,
    IMapper mapper,
    ILogger<UpdateUserHandler> logger) : IRequestHandler<UpdateUserRequest, ApiResponse>
{
    public async Task<ApiResponse> Handle(UpdateUserRequest request, CancellationToken cancellationToken)
    {
        var res = new ApiResponse();

        logger.LogInformation("Starting user update operation for user ID: {UserId}", currentUserService.Id);

        try
        {
            var validationResult = await validator.ValidateAsync(request, cancellationToken);
            if (!validationResult.IsValid)
            {
                logger.LogWarning("Validation failed for user {UserId} with {ErrorCount} errors: {Errors}",
                    currentUserService.Id, validationResult.Errors.Count, validationResult.Errors.ToDic());
                return res.SetError(nameof(E000), E000, validationResult.Errors.ToDic());
            }

            if (currentUserService.Id is null)
            {
                logger.LogWarning("Missing user ID in current user context");
                return res.SetError(nameof(E103), E103);
            }
            var userId = currentUserService.Id.Value;

            // Log cache lookup attempt
            logger.LogDebug("Attempting to retrieve user {UserId} from cache", userId);
            var cacheKey = CacheKeys.ForEntity<User>(userId);
            var (success, user) = await cache.TryGetValueAsync<User>(cacheKey, cancellationToken);

            if (!success || user is null || !user.IsActive)
            {
                logger.LogInformation("Cache miss for user {UserId}, fetching from repository", userId);
                user = await userRepository.GetByIdAsync(userId, cancellationToken);

                if (user == null)
                {
                    logger.LogWarning("User not found in repository for ID: {UserId}", userId);
                }
            }
            else
            {
                logger.LogDebug("Cache hit for user {UserId}", userId);
                user = userRepository.Attach(user);
            }

            if (user is null)
            {
                logger.LogWarning("User not found: UserId={UserId}", userId);
                return res.SetError(nameof(E008), string.Format(E008, "User"));
            }

            // Log the fields being updated
            logger.LogInformation("Updating user {UserId} with changes: FullName={FullName}, Email={Email}, Phone={Phone}, Address={Address}",
                userId,
                request.FullName ?? "[unchanged]",
                request.Email ?? "[unchanged]",
                request.Phone ?? "[unchanged]",
                request.Address ?? "[unchanged]");

            user.FullName = request.FullName ?? user.FullName;
            user.Email = request.Email ?? user.Email;
            user.Phone = request.Phone ?? user.Phone;
            user.Address = request.Address ?? user.Address;

            // Persist changes
            if (!await userRepository.SaveChangeAsync(cancellationToken))
            {
                logger.LogError("Failed to save changes for user {UserId}", userId);
                return res.SetError(nameof(E000), E000);
            }

            // Update cache
            logger.LogDebug("Updating cache for user {UserId}", userId);
            await cache.RemoveAsync(cacheKey, cancellationToken);
            await cache.SetAsync(cacheKey, user, cancellationToken);

            var dtoKey = CacheKeys.ForDto<User, CurrentUserDto>(userId);
            await cache.RemoveAsync(dtoKey, cancellationToken);
            await cache.SetAsync(dtoKey, mapper.Map<CurrentUserDto>(user), cancellationToken);

            logger.LogInformation("Successfully updated user profile for user {UserId}", userId);
            return res.SetSuccess(true);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Critical error during user update operation for user {UserId}", currentUserService.Id);
            return res.SetError(nameof(E000), E000);
        }
    }
}
