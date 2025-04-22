namespace SharedKernel.Interfaces;

public interface IFileStorageService
{
    string BucketName { get; }
    Task<string> UploadFileAsync(string objectName, Stream data, long size, string contentType, CancellationToken cancellationToken = default);
    Task<Stream> GetFileAsync(string objectName, CancellationToken cancellationToken = default);
    Task DeleteFileAsync(string objectName, CancellationToken cancellationToken = default);
    Task DeleteFilesByPrefixAsync(string prefix, CancellationToken cancellationToken);
    Task<bool> BucketExistedAsync(CancellationToken cancellationToken = default);
    Task CreateBucketAsync(CancellationToken cancellationToken = default);
}
