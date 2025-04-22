using Microsoft.AspNetCore.Http;
using SharedKernel.Extensions;
using SharedKernel.Interfaces;
using Microsoft.Extensions.Logging;
using PropertyService.Domain.Entities;

namespace PropertyService.Application.Extensions;

public static class PropertyExtension
{
    public static async Task<Property> UploadImagesAsync(this Property property,
        List<IFormFile> images,
        IFileStorageService fileStorageService,
        ILogger logger,
        CancellationToken cancellationToken = default)
    {
        logger.LogInformation("Starting image upload for property {PropertyId}", property.Id);

        try
        {
            if (images == null || !images.Any())
            {
                logger.LogWarning("No images provided for upload for property {PropertyId}", property.Id);
                return property;
            }

            logger.LogDebug("Processing {ImageCount} images for property {PropertyId}", images.Count, property.Id);

            foreach (var image in images)
            {
                var originFileName = image.FileName;
                var fileExtension = Path.GetExtension(originFileName);
                var fileName = StringExtension.GenerateHashId(32);
                var objectName = $"properties/{property.Id}/{fileName}{fileExtension}";

                logger.LogDebug("Processing image: {OriginalFileName} -> {ObjectName}",
                    originFileName, objectName);

                var prefix = $"properties/{property.Id}";
                logger.LogInformation("Deleting existing images with prefix {Prefix}", prefix);
                await fileStorageService.DeleteFilesByPrefixAsync(prefix, cancellationToken);

                using var stream = image.OpenReadStream();
                logger.LogDebug("Uploading file {ObjectName} with size {FileSize} bytes",
                    objectName, image.Length);

                var url = await fileStorageService.UploadFileAsync(
                    objectName,
                    stream,
                    image.Length,
                    image.ContentType,
                    cancellationToken
                );

                if (string.IsNullOrWhiteSpace(url))
                {
                    logger.LogError("Failed to upload image {ObjectName} for property {PropertyId}",
                        objectName, property.Id);
                    throw new Exception($"Failed to upload the image {objectName}");
                }

                logger.LogInformation("Successfully uploaded image {ObjectName} for property {PropertyId}",
                    objectName, property.Id);

                property.Images.Add(new Image
                {
                    HashId = fileName,
                    OriginalFilename = originFileName,
                    BucketName = fileStorageService.BucketName,
                    ObjectName = objectName,
                    ContentType = image.ContentType,
                    FileSize = image.Length,
                });
            }

            logger.LogInformation("Completed image upload for property {PropertyId}. Total images: {ImageCount}",
                property.Id, property.Images.Count);
            return property;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error uploading images for property {PropertyId}", property.Id);
            throw;
        }
    }
}