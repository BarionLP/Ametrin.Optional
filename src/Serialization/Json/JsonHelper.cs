using System.Diagnostics;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Ametrin.Optional.Serialization.Json;

internal static class JsonHelper
{
    internal const string SUCCESS_PROPERTY_NAME = "$success";
    internal const string ERROR_PROPERTY_NAME = "$error";
    internal const string TYPE_PROPERTY_NAME = "$type";


    extension(JsonSerializerOptions options)
    {
        internal string GetPropertyName(string logicalName)
            => options.PropertyNamingPolicy?.ConvertName(logicalName) ?? logicalName;

        internal StringComparison StringComparison => options.PropertyNameCaseInsensitive ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal;
    }
    
    extension(ref Utf8JsonReader reader)
    {
        [StackTraceHidden]
        internal void HandleUnmappedMember(JsonSerializerOptions options)
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
}
