using System.Text.Json;
using System.Text.Json.Serialization;

public class NullableEnumConverter<T> : JsonConverter<T?> where T : struct, Enum
{
    private readonly JsonStringEnumConverter _baseConverter = new();

    public override T? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType == JsonTokenType.Null)
            return null;

        return JsonSerializer.Deserialize<T>(ref reader, options);
    }

    public override void Write(Utf8JsonWriter writer, T? value, JsonSerializerOptions options)
    {
        if (value == null)
            writer.WriteNullValue();
        else
            JsonSerializer.Serialize(writer, value.Value, options);
    }
}