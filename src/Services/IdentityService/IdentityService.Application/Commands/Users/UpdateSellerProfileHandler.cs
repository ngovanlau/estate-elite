using AutoMapper;
using DistributedCache.Redis;
using FluentValidation;
using IdentityService.Application.Dtos.Users;
using IdentityService.Application.Interfaces;
using IdentityService.Application.Requests.Users;
using IdentityService.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Common.Infrastructure.Extensions;
using Common.Application.Interfaces;
using Common.Application.Responses;
using static SharedKernel.Constants.ErrorCode;

namespace IdentityService.Application.Commands.Users;

public class UpdateSellerProfileHandler(
    IValidator<UpdateSellerProfileRequest> validator,
    IUserRepository userRepository,
    ICurrentUserService currentUserService,
    IDistributedCache cache,
    IMapper mapper,
    ILogger<UpdateSellerProfileHandler> logger) : IRequestHandler<UpdateSellerProfileRequest, ApiResponse>
{
    public async Task<ApiResponse> Handle(UpdateSellerProfileRequest request, CancellationToken cancellationToken)
    {
        var res = new ApiResponse();

        try
        {
            // Validate request
            var validationResult = await validator.ValidateAsync(request, cancellationToken);
            if (!validationResult.IsValid)
            {
                logger.LogWarning("Request validation failed with {ErrorCount} errors", validationResult.Errors.Count);
                return res.SetError(nameof(E000), E000, validationResult.Errors.ToDic());
            }

            if (currentUserService.Id is null)
            {
                logger.LogWarning("Current user context missing ID");
                return res.SetError(nameof(E103), E103);
            }
            var userId = currentUserService.Id.Value;

            // Cache lookup
            var cacheKey = CacheKeys.ForEntity<User>(userId);
            var (success, user) = await cache.TryGetValueAsync<User>(cacheKey, cancellationToken);

            if (!success || user is null)
            {
                logger.LogDebug("User {UserId} not found in cache, querying database", userId);
                user = await userRepository.GetByIdWithIncludeAsync(userId,
                    query => query.Include(p => p.SellerProfile), cancellationToken);

                if (user == null)
                {
                    logger.LogWarning("User {UserId} not found in database", userId);
                    return res.SetError(nameof(E008), string.Format(E008, "User"));
                }

                if (!user.IsActive)
                {
                    logger.LogWarning("Attempt to update inactive user {UserId}", userId);
                    return res.SetError(nameof(E008), string.Format(E008, "User"));
                }
            }
            else
            {
                logger.LogDebug("Cache hit for user {UserId}", userId);
                user = userRepository.Attach(user);
            }

            // Update user profile
            SellerProfile sellerProfile;

            if (user.SellerProfile == null)
            {
                // Create new seller profile
                sellerProfile = mapper.Map<SellerProfile>(request);
                sellerProfile.UserId = userId;
                sellerProfile.CreatedBy = userId;
                sellerProfile.CreatedOn = DateTime.UtcNow;
                logger.LogDebug("Creating new seller profile for user {UserId}", userId);
            }
            else
            {
                // Update existing seller profile
                sellerProfile = user.SellerProfile;
                mapper.Map(request, sellerProfile);
                logger.LogDebug("Updating existing seller profile for user {UserId}", userId);
            }

            // Always update modification info
            sellerProfile.ModifiedBy = userId;
            sellerProfile.ModifiedOn = DateTime.UtcNow;

            user.SellerProfile = sellerProfile;

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

            logger.LogInformation("Successfully updated seller profile for user {UserId}", userId);
            return res.SetSuccess(true);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unexpected error updating seller profile for user {UserId}", currentUserService.Id);
            return res.SetError(nameof(E000), E000);
        }
    }
}