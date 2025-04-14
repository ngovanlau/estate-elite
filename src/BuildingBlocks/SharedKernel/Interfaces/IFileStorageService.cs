namespace SharedKernel.Interfaces;

public interface IFileStorageService
{
    Task<string> UploadFileAsync(string objectName, Stream data, long size, string contentType, CancellationToken cancellationToken = default);
    Task<Stream> GetFileAsync(string objectName, CancellationToken cancellationToken = default);
    Task DeleteFileAsync(string objectName, CancellationToken cancellationToken = default);
    Task<bool> BucketExistedAsync(CancellationToken cancellationToken = default);
    Task CreateBucketAsync(CancellationToken cancellationToken = default);
}
