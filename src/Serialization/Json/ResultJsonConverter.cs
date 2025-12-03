using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Json.Serialization.Metadata;
using static Ametrin.Optional.Serialization.Json.JsonHelper;

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

        var stringComparison = options.StringComparison;

        if (string.Equals(propertyName, SUCCESS_PROPERTY_NAME, stringComparison))
        {
            var typeInfo = (JsonTypeInfo<T>)options.GetTypeInfo(typeof(T));
            return JsonSerializer.Deserialize(ref reader, typeInfo) ?? throw new JsonException();
        }
        else if (string.Equals(propertyName, ERROR_PROPERTY_NAME, stringComparison))
        {
            var typeInfo = (JsonTypeInfo<Exception>)options.GetTypeInfo(typeof(Exception));
            return JsonSerializer.Deserialize(ref reader, typeInfo) ?? throw new JsonException();
        }

        throw new JsonException();
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

// public sealed class ResultJsonConverterFactory : JsonConverterFactory
// {
//     public override bool CanConvert(Type typeToConvert)
//     {
//         ArgumentNullException.ThrowIfNull(typeToConvert);

//         return typeToConvert.IsGenericType && typeToConvert.GetGenericTypeDefinition() == typeof(Result<>);
//     }

    // [DynamicDependency(DynamicallyAccessedMemberTypes.PublicConstructors, typeof(ResultJsonConverter<>))]
    // public override JsonConverter? CreateConverter(Type typeToConvert, JsonSerializerOptions options)
    // {
    //     ArgumentNullException.ThrowIfNull(typeToConvert);
    //     ArgumentNullException.ThrowIfNull(options);

    //     var innerType = typeToConvert.GetGenericArguments()[0];

    //     var converter = typeof(ResultJsonConverter<>).MakeGenericType(innerType);
    //     return (JsonConverter)Activator.CreateInstance(converter)!;
    // }
// }
