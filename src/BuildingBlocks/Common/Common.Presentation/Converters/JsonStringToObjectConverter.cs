using System.Text.Json;
using System.Text.Json.Serialization;

namespace Common.Presentation.Converters;

public class JsonStringToObjectConverter<T> : JsonConverter<T> where T : class
{
    public override T? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType == JsonTokenType.Null) return null;

        if (reader.TokenType == JsonTokenType.String)
        {
            string jsonString = reader.GetString() + "";
            if (string.IsNullOrWhiteSpace(jsonString)) return null;

            return JsonSerializer.Deserialize<T>(jsonString, options);
        }

        if (reader.TokenType == JsonTokenType.StartObject)
        {
            return JsonSerializer.Deserialize<T>(ref reader, options);
        }

        throw new JsonException($"Unexpected token {reader.TokenType} when parsing {typeof(T).Name}");
    }

    public override void Write(Utf8JsonWriter writer, T value, JsonSerializerOptions options)
    {
        JsonSerializer.Serialize(writer, value, options);
    }
}
