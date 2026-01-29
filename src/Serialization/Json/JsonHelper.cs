using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Ametrin.Optional.Serialization.Json;

public static class OptionJsonHelper
{
    public const string SUCCESS_PROPERTY_NAME = "$success";
    public const string ERROR_PROPERTY_NAME = "$error";
    public const string TYPE_PROPERTY_NAME = "$type";

    [RequiresDynamicCode("Uses runtime generic instantiation. For NativeAOT, register closed converters or use a source-generated JsonSerializerContext.")]
    public static JsonSerializerOptions AddOptionConvertes(this JsonSerializerOptions options)
    {
        options.Converters.Add(new OptionJsonConverterFactory());
        options.Converters.Add(new ResultJsonConverterFactory());
        options.Converters.Add(new ErrorStateJsonConverter());
        options.Converters.Add(new ErrorStateJsonConverterFactory());
        options.Converters.Add(new SimpleExceptionJsonConverter());
        return options;
    }

    internal static string GetPropertyName(this JsonSerializerOptions options, string logicalName)
        => options.PropertyNamingPolicy?.ConvertName(logicalName) ?? logicalName;

    internal static StringComparison StringComparison(this JsonSerializerOptions options) => options.PropertyNameCaseInsensitive ? System.StringComparison.OrdinalIgnoreCase : System.StringComparison.Ordinal;

    [StackTraceHidden]
    internal static void HandleUnmappedMember(this ref Utf8JsonReader reader, JsonSerializerOptions options)
    {
        switch (options.UnmappedMemberHandling)
        {
            case JsonUnmappedMemberHandling.Skip:
                reader.Skip();
                break;

            case JsonUnmappedMemberHandling.Disallow:
                throw new JsonException();

            default:
                throw new UnreachableException();
        }
    }
}
