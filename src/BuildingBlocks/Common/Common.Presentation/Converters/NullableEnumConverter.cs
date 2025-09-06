using System.Text.Json;
using System.Text.Json.Serialization;

namespace Common.Presentation.Converters;

public class NullableEnumConverter<T> : JsonConverter<T?> where T : struct, Enum
{
    private readonly JsonConverter<T> _enumConverter;

    public NullableEnumConverter(JsonStringEnumConverter stringEnumConverter)
    {
        // Tạo converter cho kiểu enum không nullable (T)
        _enumConverter = (JsonConverter<T>)stringEnumConverter.CreateConverter(typeof(T), new JsonSerializerOptions());
    }

    public override T? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType == JsonTokenType.Null)
            return null;

        return _enumConverter.Read(ref reader, typeof(T), options);
    }

    public override void Write(Utf8JsonWriter writer, T? value, JsonSerializerOptions options)
    {
        if (value == null)
            writer.WriteNullValue();
        else
            _enumConverter.Write(writer, value.Value, options);
    }
}