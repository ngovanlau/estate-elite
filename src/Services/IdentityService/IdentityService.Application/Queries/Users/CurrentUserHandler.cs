using AutoMapper;
using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;

namespace IdentityService.Application.Queries.Users;

using DistributedCache.Redis;
using Domain.Entities;
using Dtos.Users;
using Interfaces;
using Requests.Users;
using SharedKernel.Commons;
using SharedKernel.Interfaces;
using static SharedKernel.Constants.ErrorCode;

public class CurrentUserHandler(
    ICurrentUserService currentUserService,
    IUserRepository userRepository,
    IDistributedCache cache,
    IMapper mapper,
    ILogger<CurrentUserHandler> logger) : IRequestHandler<CurrentUserRequest, ApiResponse>
{
    public async Task<ApiResponse> Handle(CurrentUserRequest request, CancellationToken cancellationToken)
    {
        var response = new ApiResponse();

        try
        {
            if (currentUserService.Id is null)
            {
                logger.LogWarning("Authentication error: Missing user ID");
                return response.SetError(nameof(E103), E103);
            }

            var userId = currentUserService.Id.Value;
            logger.LogInformation("Processing current user request: UserId={UserId}", userId);

            var currentUserDto = await GetCurrentUserDtoAsync(userId, cancellationToken);

            if (currentUserDto is null)
            {
                logger.LogWarning("User not found or inactive: UserId={UserId}", userId);
                return response.SetError(nameof(E008), string.Format(E008, "User"));
            }

            return response.SetSuccess(currentUserDto);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to retrieve current user information");
            return response.SetError(nameof(E000), E000, ex);
        }
    }

    private async Task<CurrentUserDto?> GetCurrentUserDtoAsync(Guid userId, CancellationToken cancellationToken)
    {
        var currentUserDtoKey = CacheKeys.ForDto<CurrentUserDto>(userId);
        var (dtoSuccess, currentUserDto) = await cache.TryGetValueAsync<CurrentUserDto>(currentUserDtoKey, cancellationToken);

        if (dtoSuccess && currentUserDto is not null)
        {
            return currentUserDto;
        }

        logger.LogInformation("Cache miss for current user DTO: UserId={UserId}", userId);
        currentUserDto = await GetUserFromRepositoryOrCacheAsync(userId, cancellationToken);

        if (currentUserDto is not null)
        {
            await cache.SetAsync(currentUserDtoKey, currentUserDto, cancellationToken);
        }

        return currentUserDto;
    }

    private async Task<CurrentUserDto?> GetUserFromRepositoryOrCacheAsync(Guid userId, CancellationToken cancellationToken)
    {
        var userKey = CacheKeys.ForEntity<User>(userId);
        var (userSuccess, user) = await cache.TryGetValueAsync<User>(userKey, cancellationToken);

        if (userSuccess && user is not null && user.IsActive)
        {
            return mapper.Map<CurrentUserDto>(user);
        }

        logger.LogInformation("Retrieving user from repository: UserId={UserId}", userId);
        return await userRepository.GetCurrentUserDtoByIdAsync(userId, cancellationToken);
    }
}