using Common.Application.Interfaces;
using Common.Infrastructure.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using PropertyService.Domain.Entities;

namespace PropertyService.Application.Extensions;

public static class PropertyExtension
{
    public static async Task<Property> UploadImagesAsync(this Property property,
        List<IFormFile> files,
        IFileStorageService fileStorageService,
        ILogger logger,
        CancellationToken cancellationToken = default)
    {
        logger.LogInformation("Starting image upload for property {PropertyId}", property.Id);

        try
        {
            if (files == null || !files.Any())
            {
                logger.LogWarning("No images provided for upload for property {PropertyId}", property.Id);
                return property;
            }

            logger.LogDebug("Processing {ImageCount} images for property {PropertyId}", files.Count, property.Id);

            var prefix = $"properties/{property.Id}";
            logger.LogInformation("Deleting existing images with prefix {Prefix}", prefix);
            await fileStorageService.DeleteFilesByPrefixAsync(prefix, cancellationToken);

            foreach (var file in files)
            {
                var originFileName = file.FileName;
                var fileExtension = Path.GetExtension(originFileName);
                var fileName = StringExtension.GenerateHashId(32);
                var objectName = $"properties/{property.Id}/{fileName}{fileExtension}";

                logger.LogDebug("Processing image: {OriginalFileName} -> {ObjectName}",
                    originFileName, objectName);

                using var stream = file.OpenReadStream();
                logger.LogDebug("Uploading file {ObjectName} with size {FileSize} bytes",
                    objectName, file.Length);

                var url = await fileStorageService.UploadFileAsync(
                    objectName,
                    stream,
                    file.Length,
                    file.ContentType,
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

                var image = new Image
                {
                    HashId = fileName,
                    OriginalFilename = originFileName,
                    BucketName = fileStorageService.BucketName,
                    ObjectName = objectName,
                    ContentType = file.ContentType,
                    FileSize = file.Length,
                    PropertyId = property.Id,
                };
                property.Images.Add(image);
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
