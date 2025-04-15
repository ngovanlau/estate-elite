using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;

namespace IdentityService.Application.Commands.Users;

using DistributedCache.Redis;
using Domain.Entities;
using Interfaces;
using Requests.Users;
using SharedKernel.Commons;
using SharedKernel.Extensions;
using SharedKernel.Interfaces;
using static SharedKernel.Constants.ErrorCode;

public class UploadHandler(
    ICurrentUserService currentUserService,
    IValidator<UploadRequest> validator,
    IUserRepository userRepository,
    IFileStorageService fileStorageService,
    IDistributedCache cache,
    ILogger<UploadHandler> logger) : IRequestHandler<UploadRequest, ApiResponse>
{
    public async Task<ApiResponse> Handle(UploadRequest request, CancellationToken cancellationToken)
    {
        var res = new ApiResponse();
        logger.LogInformation("Upload process started: User={UserId}, IsAvatar={IsAvatar}",
            currentUserService.Id, request.IsAvatar);

        try
        {
            var validationResult = await validator.ValidateAsync(request, cancellationToken);
            if (!validationResult.IsValid)
            {
                logger.LogWarning("Validation failed: {Errors}", validationResult.Errors.ToDic());
                return res.SetError(nameof(E000), E000, validationResult.Errors.ToDic());
            }

            if (currentUserService.Id is null)
            {
                logger.LogWarning("Missing user ID");
                return res.SetError(nameof(E103), E103);
            }
            var userId = currentUserService.Id.Value;

            var cacheKey = CacheKeys.ForEntity<User>(userId);
            var (success, user) = await cache.TryGetValueAsync<User>(cacheKey, cancellationToken);

            if (!success || user is null || !user.IsActive)
            {
                logger.LogDebug("Cache miss: Fetching user from repository");
                user = await userRepository.GetUserByIdAsync(userId, cancellationToken);
            }

            if (user is null)
            {
                logger.LogWarning("User not found: UserId={UserId}", userId);
                return res.SetError(nameof(E008), string.Format(E008, "User"));
            }

            var targetProperty = request.IsAvatar ? "avatars" : "backgrounds";
            var url = await UploadImageAsync(request.Image, targetProperty, cancellationToken);

            // Update user property
            if (request.IsAvatar) user.Avatar = url;
            else user.Background = url;

            // Save changes and update cache
            if (await userRepository.SaveChangeAsync(cancellationToken))
            {
                await cache.RemoveAsync(cacheKey, cancellationToken);
                await cache.SetAsync(cacheKey, user, cancellationToken);
                logger.LogInformation("Upload successful: Type={Type}, UserId={UserId}",
                    request.IsAvatar ? "avatar" : "background", userId);
            }
            else
            {
                logger.LogWarning("Failed to save changes: UserId={UserId}", userId);
            }

            return res.SetSuccess(url);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Upload process failed: UserId={UserId}", currentUserService.Id);
            return res.SetError(nameof(E000), E000, ex);
        }
    }

    private async Task<string> UploadImageAsync(IFormFile image, string targetProperty, CancellationToken cancellationToken = default)
    {
        var userId = currentUserService.Id;

        logger.LogInformation("Starting image upload: FileName={FileName}, TargetProperty={TargetProperty}, UserId={UserId}",
            image.FileName, targetProperty, userId);

        var fileExtension = Path.GetExtension(image.FileName);
        var objectName = $"{targetProperty}/{userId}{fileExtension}";

        try
        {
            var prefix = $"{targetProperty}/{userId}";
            logger.LogDebug("Attempting to delete existing files with prefix: '{prefix}'", prefix);
            await fileStorageService.DeleteFilesByPrefixAsync(prefix, cancellationToken);

            logger.LogDebug("Uploading: FileName={FileName}, ObjectName={ObjectName}, Size={Size}KB, ContentType={ContentType}",
                image.FileName, objectName, image.Length / 1024, image.ContentType);

            using var stream = image.OpenReadStream();
            var url = await fileStorageService.UploadFileAsync(
                objectName,
                stream,
                image.Length,
                image.ContentType,
                cancellationToken
            );

            logger.LogInformation("Upload completed successfully: FileName={FileName}, ObjectName={ObjectName}, Url={Url}",
                image.FileName, objectName, url);

            return url;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "File upload failed: FileName={FileName}, ObjectName={ObjectName}, Size={Size}KB, Error={Error}",
                image.FileName, objectName, image.Length / 1024, ex.Message);
            throw;
        }
    }
}
