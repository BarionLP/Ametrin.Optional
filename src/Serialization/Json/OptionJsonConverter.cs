
using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Ametrin.Optional.Serialization.Json;

// TODO: make AOT compatible or provide AOT alternative
[RequiresDynamicCode("JSON serialization and deserialization might require types that cannot be statically analyzed")]
[RequiresUnreferencedCode("JSON serialization and deserialization might require types that cannot be statically analyzed")]
public sealed class OptionJsonConverter<T> : JsonConverter<Option<T>>
{
    public override Option<T> Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType is JsonTokenType.Null)
        {
            return default;
        }
        else
        {
            return JsonSerializer.Deserialize<T>(ref reader, options);
        }
    }

    public override void Write(Utf8JsonWriter writer, Option<T> value, JsonSerializerOptions options)
    {
        if (value.Branch(out var inner))
        {
            JsonSerializer.Serialize(writer, inner, options);
        }
        else
        {
            writer.WriteNullValue();
        }
    }
}