using System.Text.Json;
using System.Text.Json.Serialization;

namespace Ametrin.Optional.Serialization.Json;

internal static class JsonHelper
{
    internal const string SUCCESS_PROPERTY_NAME = "$success";
    internal const string ERROR_PROPERTY_NAME = "$error";
    internal const string TYPE_PROPERTY_NAME = "$type";


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
