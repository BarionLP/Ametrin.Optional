using System.Text.Json;
using Ametrin.Optional.Serialization.Json;

namespace Ametrin.Optional.Test;

public sealed class JsonSerializationTests
{
    private static readonly JsonSerializerOptions options = new JsonSerializerOptions(JsonSerializerOptions.Default).AddOptionConvertes();

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


    [Test]
    public async Task Result_Tests()
    {
        var success = JsonSerializer.Serialize(Result.Success(42), options);
        var error = JsonSerializer.Serialize(Result.Error<int>(new InvalidOperationException("custom error message")), options);
        await Assert.That(success).EqualTo("""{"$success":42}""");
        await Assert.That(error).EqualTo("""{"$error":{"$type":"System.InvalidOperationException","Message":"custom error message"}}""");
        await Assert.That(JsonSerializer.Deserialize<Result<int>>(success, options)).IsSuccess(42);
        await Assert.That(JsonSerializer.Deserialize<Result<int>>(error, options)).IsError(e => e is InvalidOperationException { Message: "custom error message" });
    }


    [Test]
    public async Task Result2_Tests()
    {
        var success = JsonSerializer.Serialize(Result.Success<int, string>(42), options);
        var error = JsonSerializer.Serialize(Result.Error<int, string>("custom error message"), options);
        await Assert.That(success).EqualTo("""{"$success":42}""");
        await Assert.That(error).EqualTo("""{"$error":"custom error message"}""");
        await Assert.That(JsonSerializer.Deserialize<Result<int, string>>(success, options)).IsSuccess(42);
        await Assert.That(JsonSerializer.Deserialize<Result<int, string>>(error, options)).IsError("custom error message");
    }


    [Test]
    public async Task ErrorState_Tests()
    {
        var success = JsonSerializer.Serialize(ErrorState.Success(), options);
        var error = JsonSerializer.Serialize(ErrorState.Error(new InvalidOperationException("custom error message")), options);
        await Assert.That(success).EqualTo("null");
        await Assert.That(error).EqualTo("""{"$type":"System.InvalidOperationException","Message":"custom error message"}""");
        await Assert.That(JsonSerializer.Deserialize<ErrorState>(success, options)).IsSuccess();
        await Assert.That(JsonSerializer.Deserialize<ErrorState>(error, options)).IsError(e => e is InvalidOperationException { Message: "custom error message" });
    }


    [Test]
    public async Task ErrorState2_Tests()
    {
        var success = JsonSerializer.Serialize(ErrorState.Success<string>(), options);
        var error = JsonSerializer.Serialize(ErrorState.Error("custom error message"), options);
        await Assert.That(success).EqualTo("null");
        await Assert.That(error).EqualTo("\"custom error message\"");
        await Assert.That(JsonSerializer.Deserialize<ErrorState<string>>(success, options)).IsSuccess();
        await Assert.That(JsonSerializer.Deserialize<ErrorState<string>>(error, options)).IsError("custom error message");
    }
}
