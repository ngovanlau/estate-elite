using System;

namespace SharedKernel.Settings;

public class MinioSetting
{
    public required string Endpoint { get; set; }
    public required string Port { get; set; }
    public required string AccessKey { get; set; }
    public required string SecretKey { get; set; }
    public bool UseSSL { get; set; }
    public required string BucketName { get; set; }
}
