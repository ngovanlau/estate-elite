using System.Reactive.Linq;
using Common.Application.Interfaces;
using Common.Application.Settings;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Minio;
using Minio.DataModel.Args;

namespace Common.Infrastructure.Implements;

public class MinioStorageService : IFileStorageService
{
    private readonly MinioSetting _setting;
    private readonly IMinioClient _client;
    private readonly string _bucketName;
    private readonly ILogger<MinioStorageService> _logger;
    public string BucketName => _bucketName;

    public MinioStorageService(IOptions<MinioSetting> options, ILogger<MinioStorageService> logger)
    {
        _setting = options.Value;
        _bucketName = _setting.BucketName;
        _logger = logger;

        var endpoint = new Uri(_setting.Endpoint);
        _client = new MinioClient()
            .WithEndpoint(endpoint.Host, endpoint.Port)
            .WithCredentials(_setting.AccessKey, _setting.SecretKey)
            .WithSSL(_setting.UseSSL)
            .Build();
    }

    public async Task<bool> BucketExistedAsync(CancellationToken cancellationToken)
    {
        try
        {
            var bucketExistsArgs = new BucketExistsArgs().WithBucket(_bucketName);
            return await _client.BucketExistsAsync(bucketExistsArgs, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error checking if bucket '{BucketName}' exists", _bucketName);
            throw;
        }
    }

    public async Task CreateBucketAsync(CancellationToken cancellationToken)
    {
        try
        {
            var makeBucketArgs = new MakeBucketArgs().WithBucket(_bucketName);
            await _client.MakeBucketAsync(makeBucketArgs, cancellationToken);
            _logger.LogInformation("Bucket '{BucketName}' created successfully", _bucketName);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating bucket '{BucketName}'", _bucketName);
            throw;
        }
    }

    public async Task DeleteFileAsync(string objectName, CancellationToken cancellationToken)
    {
        try
        {
            var removeObjectArgs = new RemoveObjectArgs()
                .WithBucket(_bucketName)
                .WithObject(objectName);

            await _client.RemoveObjectAsync(removeObjectArgs, cancellationToken);
            _logger.LogInformation("File '{ObjectName}' deleted successfully from bucket '{BucketName}'", objectName, _bucketName);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting file '{ObjectName}' from bucket '{BucketName}'", objectName, _bucketName);
            throw;
        }
    }

    public async Task DeleteFilesByPrefixAsync(string prefix, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(prefix))
        {
            throw new ArgumentException("Prefix cannot be null or empty", nameof(prefix));
        }

        try
        {
            _logger.LogInformation("Starting to delete files with prefix '{Prefix}' in bucket '{BucketName}'", prefix, _bucketName);

            if (!await BucketExistedAsync(cancellationToken))
            {
                _logger.LogInformation("Bucket '{BucketName}' does not exist. Creating new bucket", _bucketName);
                await CreateBucketAsync(cancellationToken);
            }

            var listObjectsArgs = new ListObjectsArgs()
                .WithBucket(_bucketName)
                .WithPrefix(prefix)
                .WithRecursive(true);

            var deleteCount = 0;
            const int batchSize = 1000; // MinIO recommends max 1000 objects per delete request
            var currentBatch = new List<string>(batchSize);

            var observable = _client.ListObjectsEnumAsync(listObjectsArgs, cancellationToken);
            await foreach (var item in observable)
            {
                if (cancellationToken.IsCancellationRequested)
                {
                    break;
                }

                currentBatch.Add(item.Key);

                if (currentBatch.Count >= batchSize)
                {
                    await DeleteBatchAsync(currentBatch, cancellationToken);
                    deleteCount += currentBatch.Count;
                    currentBatch.Clear();
                }
            }

            if (currentBatch.Any())
            {
                await DeleteBatchAsync(currentBatch, cancellationToken);
                deleteCount += currentBatch.Count;
            }

            if (deleteCount == 0)
            {
                _logger.LogInformation("No files found with prefix '{Prefix}' in bucket '{BucketName}'", prefix, _bucketName);
            }
            else
            {
                _logger.LogInformation("Successfully deleted {Count} objects with prefix '{Prefix}'", deleteCount, prefix);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting files with prefix '{Prefix}'", prefix);
            throw;
        }
    }

    private async Task DeleteBatchAsync(List<string> objectsToDelete, CancellationToken cancellationToken)
    {
        if (!objectsToDelete.Any())
        {
            return;
        }

        try
        {
            var removeObjectsArgs = new RemoveObjectsArgs()
                .WithBucket(_bucketName)
                .WithObjects(objectsToDelete);

            var deleteErrors = await _client.RemoveObjectsAsync(removeObjectsArgs, cancellationToken);

            int errorCount = 0;
            foreach (var deleteError in deleteErrors)
            {
                if (deleteError != null)
                {
                    errorCount++;
                    _logger.LogError("Failed to delete object '{ObjectName}': {ErrorMessage}",
                        deleteError.Key, deleteError.Message);
                }
            }

            if (errorCount > 0)
            {
                _logger.LogWarning("Completed batch deletion with {ErrorCount} errors out of {TotalCount} objects",
                    errorCount, objectsToDelete.Count);
            }
            else
            {
                _logger.LogDebug("Successfully deleted batch of {Count} objects", objectsToDelete.Count);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during batch deletion of {Count} objects", objectsToDelete.Count);
            throw;
        }
    }


    public async Task<Stream> GetFileAsync(string objectName, CancellationToken cancellationToken)
    {
        try
        {
            var memoryStream = new MemoryStream();

            var getObjectArgs = new GetObjectArgs()
                .WithBucket(_bucketName)
                .WithObject(objectName)
                .WithCallbackStream(async stream => await stream.CopyToAsync(memoryStream));

            await _client.GetObjectAsync(getObjectArgs, cancellationToken);

            memoryStream.Position = 0;
            _logger.LogInformation("File '{ObjectName}' retrieved successfully from bucket '{BucketName}'", objectName, _bucketName);
            return memoryStream;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving file '{ObjectName}' from bucket '{BucketName}'", objectName, _bucketName);
            throw;
        }
    }

    public async Task<string> UploadFileAsync(string objectName, Stream data, long size, string contentType, CancellationToken cancellationToken)
    {
        try
        {
            if (!await BucketExistedAsync(cancellationToken))
            {
                _logger.LogInformation("Bucket '{BucketName}' does not exist. Creating new bucket", _bucketName);
                await CreateBucketAsync(cancellationToken);
            }

            var putObjectArgs = new PutObjectArgs()
                .WithBucket(_bucketName)
                .WithObject(objectName)
                .WithStreamData(data)
                .WithObjectSize(size)
                .WithContentType(contentType);

            var response = await _client.PutObjectAsync(putObjectArgs, cancellationToken);
            _logger.LogInformation("File '{ObjectName}' uploaded successfully to bucket '{BucketName}'", objectName, _bucketName);

            return $"{_setting.Endpoint}/{_setting.BucketName}/{response.ObjectName}";
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error uploading file '{ObjectName}' to bucket '{BucketName}'", objectName, _bucketName);
            throw;
        }
    }
}
