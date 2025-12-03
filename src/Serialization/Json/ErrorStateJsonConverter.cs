using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Json.Serialization.Metadata;

namespace Ametrin.Optional.Serialization.Json;

public sealed class ErrorStateJsonConverter : JsonConverter<ErrorState>
{
    public override ErrorState Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType is JsonTokenType.Null)
        {
            reader.Read();
            return default;
        }
        else
        {
            var typeInfo = (JsonTypeInfo<Exception>)options.GetTypeInfo(typeof(Exception));
            return JsonSerializer.Deserialize(ref reader, typeInfo) ?? throw new JsonException();
        }
    }

    public override void Write(Utf8JsonWriter writer, ErrorState value, JsonSerializerOptions options)
    {
        if (value.Branch(out var error))
        {
            writer.WriteNullValue();
        }
        else
        {
            var typeInfo = (JsonTypeInfo<Exception>)options.GetTypeInfo(typeof(Exception));
            JsonSerializer.Serialize(writer, error, typeInfo);
        }
    }
}

public sealed class ErrorStateJsonConverter<T> : JsonConverter<ErrorState<T>>
{
    public override ErrorState<T> Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType is JsonTokenType.Null)
        {
            reader.Read();
            return default;
        }
        else
        {
            var typeInfo = (JsonTypeInfo<T>)options.GetTypeInfo(typeof(T));
            return JsonSerializer.Deserialize(ref reader, typeInfo) ?? throw new JsonException();
        }
    }

    public override void Write(Utf8JsonWriter writer, ErrorState<T> value, JsonSerializerOptions options)
    {
        if (value.Branch(out var inner))
        {
            writer.WriteNullValue();
        }
        else
        {
            var typeInfo = (JsonTypeInfo<T>)options.GetTypeInfo(typeof(T));
            JsonSerializer.Serialize(writer, inner, typeInfo);
        }
    }
}