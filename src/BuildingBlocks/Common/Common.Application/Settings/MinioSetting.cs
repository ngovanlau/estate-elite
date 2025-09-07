namespace Common.Application.Settings;

public sealed record MinioSetting(
    string Endpoint,
    string Port,
    string AccessKey,
    string SecretKey,
    bool UseSSL,
    string BucketName
)
{
    public const string SectionName = "MinioSetting";
}
