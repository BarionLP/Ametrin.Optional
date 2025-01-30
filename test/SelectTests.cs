namespace Ametrin.Optional.Test;

public sealed class SelectTests
{
    [Test]
    public async Task Select_Success_Test()
    {
        await Assert.That(Option.Success(1).Select(i => i + 1)).IsSuccess(2);
        await Assert.That(Option.Success(1).Select(i => Option.Success(i))).IsSuccess(1);
        await Assert.That(Result.Success(1).Select(i => i + 1)).IsSuccess(2);
        await Assert.That(Result.Success(1).Select(i => i + 1, e => e.Message)).IsSuccess(2);
        await Assert.That(Result.Success(1).Select(i => Result.Success(i))).IsSuccess(1);
        await Assert.That(Result.Success<int, string>(1).Select(i => i + 1)).IsSuccess(2);
        await Assert.That(Result.Success<int, int>(1).Select(i => i + 1, error => error.ToString())).IsSuccess(2);
        await Assert.That(Result.Success<int, string>(5).Select(i => i + 1, error => new Exception(error))).IsSuccess();
        await Assert.That(Result.Success<int, string>(1).Select(i => Result.Success<int, string>(i))).IsSuccess(1);
        await Assert.That(RefOption.Success<Span<char>>([]).Select(s => new string(s))).IsSuccess("");
        await Assert.That(OptionsMarshall.IsSuccess(RefOption.Success<Span<char>>([]).Select(s => s))).IsTrue();
        await Assert.That(OptionsMarshall.IsSuccess(Option.Success("").SelectAsSpan())).IsTrue();

        await Assert.That(Option.Success().Select(() => "yay", () => "nay")).IsEqualTo("yay");
        await Assert.That(ErrorState.Success().Select(() => "yay", e => "nay")).IsEqualTo("yay");
        await Assert.That(ErrorState.Success<int>().Select(() => "yay", e => "nay")).IsEqualTo("yay");
    }

    [Test]
    public async Task Select_Error_Test()
    {
        await Assert.That(Option.Error<int>().Select(i => i + 1)).IsError();
        await Assert.That(Option.Error<int>().Select(i => Option.Success(i))).IsError();
        await Assert.That(Option.Success(1).Select(i => Option.Error<int>())).IsError();
        await Assert.That(Result.Error<int>().Select(i => i + 1)).IsError();
        await Assert.That(Result.Error<int>().Select(i => i + 1, e => e.Message)).IsError();
        await Assert.That(Result.Error<int>(new FormatException()).Select(i => Result.Success(i))).IsErrorOfType<int, FormatException>();
        await Assert.That(Result.Success(1).Select(i => Result.Error<int>(new ArgumentException()))).IsErrorOfType<int, ArgumentException>();
        await Assert.That(Result.Error<int, string>("").Select(i => i + 1)).IsError("");
        await Assert.That(Result.Error<int, int>(5).Select(i => i + 1, error => error.ToString())).IsError("5");
        await Assert.That(Result.Error<int, string>("5").Select(i => i + 1, error => new Exception(error))).IsError();
        await Assert.That(Result.Error<int, string>("nay").Select(i => Result.Success<int, string>(i))).IsError("nay");
        await Assert.That(Result.Success<int, string>(1).Select(i => Result.Error<int, string>("nay"))).IsError("nay");
        await Assert.That(RefOption.Error<Span<char>>().Select(s => new string(s))).IsError();
        await Assert.That(OptionsMarshall.IsSuccess(RefOption.Error<Span<char>>().Select(s => s))).IsFalse();
        await Assert.That(OptionsMarshall.IsSuccess(Option.Error<string>().SelectAsSpan())).IsFalse();


        await Assert.That(Option.Error().Select(() => "yay", () => "nay")).IsEqualTo("nay");
        await Assert.That(ErrorState.Error().Select(() => "yay", e => "nay")).IsEqualTo("nay");
        await Assert.That(ErrorState.Error(1).Select(() => "yay", e => "nay")).IsEqualTo("nay");
    }
}
