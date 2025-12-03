
using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Json.Serialization.Metadata;

namespace Ametrin.Optional.Serialization.Json;

// [RequiresDynamicCode("JSON serialization and deserialization might require types that cannot be statically analyzed")]
// [RequiresUnreferencedCode("JSON serialization and deserialization might require types that cannot be statically analyzed")]
public sealed class OptionJsonConverter<T> : JsonConverter<Option<T>>
{
    public override Option<T> Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType is JsonTokenType.Null)
        {
            reader.Read();
            return default;
        }
        else
        {
            var typeInfo = (JsonTypeInfo<T>)options.GetTypeInfo(typeof(T));
            return JsonSerializer.Deserialize(ref reader, typeInfo);
        }
    }

    public override void Write(Utf8JsonWriter writer, Option<T> value, JsonSerializerOptions options)
    {
        if (value.Branch(out var inner))
        {
            var typeInfo = (JsonTypeInfo<T>)options.GetTypeInfo(typeof(T));
            JsonSerializer.Serialize(writer, inner, typeInfo);
        }
        else
        {
            writer.WriteNullValue();
        }
    }
}
