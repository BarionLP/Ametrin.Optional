using System.Text.Json;
using Ametrin.Optional.Serialization.Json;

namespace Ametrin.Optional.Test;

public sealed class JsonSerializationTests
{
    private static readonly JsonSerializerOptions options = new(JsonSerializerOptions.Default)
    {
        Converters =
        {
            new OptionJsonConverter<int>(),
            new ErrorStateJsonConverter(),
            new ErrorStateJsonConverter<string>(),
        }
    };
    
    [Test]
    public async Task Option_Tests()
    {
        var success = JsonSerializer.Serialize(Option.Success(42), options);
        var error = JsonSerializer.Serialize(Option.Error<int>(), options);
        await Assert.That(success).EqualTo("42");
        await Assert.That(error).EqualTo("null");
        await Assert.That(JsonSerializer.Deserialize<Option<int>>(success, options)).IsSuccess(42);
        await Assert.That(JsonSerializer.Deserialize<Option<int>>(error, options)).IsError();
    }
}
