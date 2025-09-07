using System.Text.Json;
using System.Text.Json.Serialization;

namespace Common.Presentation.Converters;

public class NullableEnumConverterFactory : JsonConverterFactory
{
    private static readonly JsonStringEnumConverter _stringEnumConverter = new();

    public override bool CanConvert(Type typeToConvert)
    {
        // Kiểm tra nếu là Nullable<Enum> (ví dụ: RentPeriod?)
        return Nullable.GetUnderlyingType(typeToConvert)?.IsEnum == true;
    }

    public override JsonConverter CreateConverter(Type typeToConvert, JsonSerializerOptions options)
    {
        // Lấy kiểu enum cơ bản (ví dụ: RentPeriod? -> RentPeriod)
        Type enumType = Nullable.GetUnderlyingType(typeToConvert)!;

        // Tạo JsonConverter cho Nullable<Enum> bằng reflection
        Type converterType = typeof(NullableEnumConverter<>).MakeGenericType(enumType);
        return (JsonConverter)Activator.CreateInstance(converterType, _stringEnumConverter)!;
    }
}
