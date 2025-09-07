namespace Common.Application.Settings;

public sealed record GrpcEndpointSetting(
    string Identity,
    string Property,
    string Payment
)
{
    public const string SectionName = "GrpcEndpointSetting";
}
