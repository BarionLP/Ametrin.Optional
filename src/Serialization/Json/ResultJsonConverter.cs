using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Json.Serialization.Metadata;
using static Ametrin.Optional.Serialization.Json.OptionJsonHelper;

namespace Ametrin.Optional.Serialization.Json;

public sealed class ResultJsonConverter<T> : JsonConverter<Result<T>>
{
    public override Result<T> Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType is not JsonTokenType.StartObject) throw new JsonException();

        if (!reader.Read()) throw new JsonException();

        if (reader.TokenType is not JsonTokenType.PropertyName) throw new JsonException();
        var propertyName = reader.GetString();

        if (!reader.Read()) throw new JsonException();

        var stringComparison = options.StringComparison();

        Result<T> result;
        if (string.Equals(propertyName, SUCCESS_PROPERTY_NAME, stringComparison))
        {
            var typeInfo = (JsonTypeInfo<T>)options.GetTypeInfo(typeof(T));
            result = JsonSerializer.Deserialize(ref reader, typeInfo) ?? throw new JsonException();
        }
        else if (string.Equals(propertyName, ERROR_PROPERTY_NAME, stringComparison))
        {
            var typeInfo = (JsonTypeInfo<Exception>)options.GetTypeInfo(typeof(Exception));
            result = JsonSerializer.Deserialize(ref reader, typeInfo) ?? throw new JsonException();
        }
        else
        {
            throw new JsonException();
        }

        if (!reader.Read() || reader.TokenType is not JsonTokenType.EndObject) throw new JsonException();

        return result;
    }

    public override void Write(Utf8JsonWriter writer, Result<T> value, JsonSerializerOptions options)
    {
        writer.WriteStartObject();
        if (value.Branch(out var success, out var error))
        {
            writer.WritePropertyName(SUCCESS_PROPERTY_NAME);
            var typeInfo = (JsonTypeInfo<T>)options.GetTypeInfo(typeof(T));
            JsonSerializer.Serialize(writer, success, typeInfo);
        }
        else
        {
            writer.WritePropertyName(ERROR_PROPERTY_NAME);
            var typeInfo = (JsonTypeInfo<Exception>)options.GetTypeInfo(typeof(Exception));
            JsonSerializer.Serialize(writer, error, typeInfo);
        }
        writer.WriteEndObject();
    }
}
public sealed class ResultJsonConverter<T, E> : JsonConverter<Result<T, E>>
{
    public override Result<T, E> Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType is not JsonTokenType.StartObject) throw new JsonException();

        if (!reader.Read()) throw new JsonException();

        if (reader.TokenType is not JsonTokenType.PropertyName) throw new JsonException();
        var propertyName = reader.GetString();

        if (!reader.Read()) throw new JsonException();

        var stringComparison = options.StringComparison();

        Result<T, E> result;
        if (string.Equals(propertyName, SUCCESS_PROPERTY_NAME, stringComparison))
        {
            var typeInfo = (JsonTypeInfo<T>)options.GetTypeInfo(typeof(T));
            result = JsonSerializer.Deserialize(ref reader, typeInfo) ?? throw new JsonException();
        }
        else if (string.Equals(propertyName, ERROR_PROPERTY_NAME, stringComparison))
        {
            var typeInfo = (JsonTypeInfo<E>)options.GetTypeInfo(typeof(E));
            result = JsonSerializer.Deserialize(ref reader, typeInfo) ?? throw new JsonException();
        }
        else
        {
            throw new JsonException();
        }

        if (!reader.Read() || reader.TokenType is not JsonTokenType.EndObject) throw new JsonException();

        return result;
    }

    public override void Write(Utf8JsonWriter writer, Result<T, E> value, JsonSerializerOptions options)
    {
        writer.WriteStartObject();
        if (value.Branch(out var success, out var error))
        {
            writer.WritePropertyName(SUCCESS_PROPERTY_NAME);
            var typeInfo = (JsonTypeInfo<T>)options.GetTypeInfo(typeof(T));
            JsonSerializer.Serialize(writer, success, typeInfo);
        }
        else
        {
            writer.WritePropertyName(ERROR_PROPERTY_NAME);
            var typeInfo = (JsonTypeInfo<E>)options.GetTypeInfo(typeof(E));
            JsonSerializer.Serialize(writer, error, typeInfo);
        }
        writer.WriteEndObject();
    }
}

[RequiresDynamicCode("Uses runtime generic instantiation. For NativeAOT, register closed converters or use a source-generated JsonSerializerContext.")]
public sealed class ResultJsonConverterFactory : JsonConverterFactory
{
    public override bool CanConvert(Type typeToConvert)
    {
        if (typeToConvert.IsGenericType)
        {
            var genericTypeDefinition = typeToConvert.GetGenericTypeDefinition();
            return genericTypeDefinition == typeof(Result<>) || genericTypeDefinition == typeof(Result<,>);
        }
        return false;
    }

    public override JsonConverter? CreateConverter(Type typeToConvert, JsonSerializerOptions options)
    {
        var innerTypes = typeToConvert.GetGenericArguments();

        return innerTypes switch
        {
            [var innerType] => (JsonConverter)Activator.CreateInstance(typeof(ResultJsonConverter<>).MakeGenericType(innerType))!,
            [var innerType1, var innerType2] => (JsonConverter)Activator.CreateInstance(typeof(ResultJsonConverter<,>).MakeGenericType(innerType1, innerType2))!,
            _ => throw new UnreachableException(),
        };
    }
}