using Microsoft.Extensions.Options;
using Microsoft.Extensions.Logging;
using Minio;
using Minio.DataModel.Args;

namespace SharedKernel.Implements;

using Interfaces;
using Settings;

public class MinioStorageService : IFileStorageService
{
    private readonly MinioSetting _setting;
    private readonly IMinioClient _client;
    private readonly string bucketName;
    private readonly ILogger<MinioStorageService> _logger;

    public MinioStorageService(IOptions<MinioSetting> options, ILogger<MinioStorageService> logger)
    {
        _setting = options.Value;
        bucketName = _setting.BucketName;
        _logger = logger;

        _client = new MinioClient()
            .WithEndpoint(_setting.Endpoint)
            .WithCredentials(_setting.AccessKey, _setting.SecretKey)
            .WithSSL(_setting.UseSSL)
            .Build();
    }

    public async Task<bool> BucketExistedAsync(CancellationToken cancellationToken)
    {
        try
        {
            var bucketExistsArgs = new BucketExistsArgs().WithBucket(bucketName);
            return await _client.BucketExistsAsync(bucketExistsArgs, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error checking if bucket '{BucketName}' exists", bucketName);
            throw;
        }
    }

    public async Task CreateBucketAsync(CancellationToken cancellationToken)
    {
        try
        {
            var makeBucketArgs = new MakeBucketArgs().WithBucket(bucketName);
            await _client.MakeBucketAsync(makeBucketArgs, cancellationToken);
            _logger.LogInformation("Bucket '{BucketName}' created successfully", bucketName);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating bucket '{BucketName}'", bucketName);
            throw;
        }
    }

    public async Task DeleteFileAsync(string objectName, CancellationToken cancellationToken)
    {
        try
        {
            var removeObjectArgs = new RemoveObjectArgs()
                .WithBucket(bucketName)
                .WithObject(objectName);

            await _client.RemoveObjectAsync(removeObjectArgs, cancellationToken);
            _logger.LogInformation("File '{ObjectName}' deleted successfully from bucket '{BucketName}'", objectName, bucketName);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting file '{ObjectName}' from bucket '{BucketName}'", objectName, bucketName);
            throw;
        }
    }

    public async Task<Stream> GetFileAsync(string objectName, CancellationToken cancellationToken)
    {
        try
        {
            var memoryStream = new MemoryStream();

            var getObjectArgs = new GetObjectArgs()
                .WithBucket(bucketName)
                .WithObject(objectName)
                .WithCallbackStream(async stream => await stream.CopyToAsync(memoryStream));

            await _client.GetObjectAsync(getObjectArgs, cancellationToken);

            memoryStream.Position = 0;
            _logger.LogInformation("File '{ObjectName}' retrieved successfully from bucket '{BucketName}'", objectName, bucketName);
            return memoryStream;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving file '{ObjectName}' from bucket '{BucketName}'", objectName, bucketName);
            throw;
        }
    }

    public async Task<string> UploadFileAsync(string objectName, Stream data, long size, string contentType, CancellationToken cancellationToken)
    {
        try
        {
            if (!await BucketExistedAsync(cancellationToken))
            {
                _logger.LogInformation("Bucket '{BucketName}' does not exist. Creating new bucket", bucketName);
                await CreateBucketAsync(cancellationToken);
            }

            var putObjectArgs = new PutObjectArgs()
                .WithBucket(bucketName)
                .WithObject(objectName)
                .WithStreamData(data)
                .WithObjectSize(size)
                .WithContentType(contentType);

            var response = await _client.PutObjectAsync(putObjectArgs, cancellationToken);
            _logger.LogInformation("File '{ObjectName}' uploaded successfully to bucket '{BucketName}'", objectName, bucketName);

            return response.ObjectName;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error uploading file '{ObjectName}' to bucket '{BucketName}'", objectName, bucketName);
            throw;
        }
    }
}