using System.Text.Json;
using System.Text.Json.Serialization;

namespace SharedKernel.Converters;

public class UtcDateTimeConverter : JsonConverter<DateTime>
{
    public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var str = reader.GetString();
        var dateTime = DateTime.Parse(str ?? string.Empty, null, System.Globalization.DateTimeStyles.AdjustToUniversal);
        return DateTime.SpecifyKind(dateTime, DateTimeKind.Utc);
    }

    public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.ToUniversalTime().ToString("o")); // ISO 8601 format
    }
}
