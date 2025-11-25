using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;
using static Ametrin.Optional.Serialization.Json.JsonHelper;

namespace Ametrin.Optional.Serialization.Json;

public sealed class SimpleExceptionJsonConverter : JsonConverter<Exception>
{
    public override Exception? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType is not JsonTokenType.StartObject) throw new JsonException();

        if (!reader.Read()) throw new JsonException();

        string? typeName = null;
        string? message = null;
        List<Exception> inner = [];

        var stringComparison = options.StringComparison;
        var messagePropertyName = options.GetPropertyName(nameof(Exception.Message));
        var innerExceptionsPropertyName = options.GetPropertyName(nameof(AggregateException.InnerExceptions));

        while (reader.Read() && reader.TokenType is not JsonTokenType.EndObject)
        {
            if (reader.TokenType is not JsonTokenType.PropertyName) throw new JsonException();

            var name = reader.GetString();
            if (!reader.Read()) throw new JsonException();

            if (string.Equals(name, TYPE_PROPERTY_NAME, stringComparison))
            {
                typeName = reader.GetString();
            }
            else if (string.Equals(name, messagePropertyName, stringComparison))
            {
                message = reader.GetString();
            }
            else if (string.Equals(name, innerExceptionsPropertyName, stringComparison))
            {
                if (reader.TokenType is not JsonTokenType.StartArray) throw new JsonException();
                while (reader.Read() && reader.TokenType is not JsonTokenType.EndArray)
                {
                    inner.Add(Read(ref reader, null!, options) ?? throw new JsonException());
                }
                reader.Read();
            }
            else
            {
                reader.HandleUnmappedMember(options);
            }
        }

        if (string.IsNullOrWhiteSpace(typeName))
        {
            throw new JsonException();
        }

        if (typeName == typeof(AggregateException).FullName)
        {
            return new AggregateException(message, inner);
        }

#pragma warning disable IL2057 // Unrecognized value passed to the parameter of method. It's not possible to guarantee the availability of the target type.
        var type = Type.GetType(typeName, throwOnError: false);
#pragma warning restore IL2057 // Unrecognized value passed to the parameter of method. It's not possible to guarantee the availability of the target type.

        if (type is null || !type.IsAssignableTo(typeof(Exception))) throw new JsonException();

        var ctor = type.GetConstructor([typeof(string)]);
        if (ctor is not null)
        {
            return (Exception)ctor.Invoke([message]);
        }

        var ctorDefault = type.GetConstructor(Type.EmptyTypes);
        if (ctorDefault is not null)
        {
            return (Exception)ctorDefault.Invoke([]);
        }

        return new Exception(message);
    }

    public override void Write(Utf8JsonWriter writer, Exception value, JsonSerializerOptions options)
    {
        writer.WriteStartObject();

        writer.WriteString(TYPE_PROPERTY_NAME, value.GetType().FullName);
        writer.WriteString(options.GetPropertyName(nameof(Exception.Message)), value.Message);

        if (value is AggregateException aggregate)
        {
            writer.WriteStartArray(options.GetPropertyName(nameof(AggregateException.InnerExceptions)));
            foreach (var inner in aggregate.InnerExceptions)
            {
                Write(writer, inner, options);
            }
            writer.WriteEndArray();
        }

        writer.WriteEndObject();
    }
}