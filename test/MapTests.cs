namespace Ametrin.Optional.Test;

public sealed class MapTests
{
    [Test]
    public async Task Map_Success_Test()
    {
        await Assert.That(Option.Success(1).Map(i => i + 1)).IsSuccess(2);
        await Assert.That(Option.Success(1).Map(i => Option.Success(i))).IsSuccess(1);
        await Assert.That(Result.Success(1).Map(i => i + 1)).IsSuccess(2);
        await Assert.That(Result.Success(1).Map(i => Result.Success(i))).IsSuccess(1);
        await Assert.That(Result.Success<int, string>(1).Map(i => i + 1)).IsSuccess(2);
        await Assert.That(Result.Success<int, string>(5).Map(i => i + 1, error => new Exception(error))).IsSuccess();
        await Assert.That(Result.Success<int, string>(1).Map(i => Result.Success<int, string>(i))).IsSuccess(1);
        await Assert.That(RefOption.Success<Span<char>>([]).Map(s => new string(s))).IsSuccess("");
        await Assert.That(OptionsMarshall.IsSuccess(RefOption.Success<Span<char>>([]).Map(s => s))).IsTrue();
        await Assert.That(OptionsMarshall.IsSuccess(Option.Success("").MapAsSpan())).IsTrue();
        await Assert.That(new int?(1).Map(v => v * 2)).IsEqualTo(2);
        await Assert.That(new int?(1).Map(v => v.ToString())).IsEqualTo("1");
        await Assert.That(((string?)"").Map(v => v + "a")).IsEqualTo("a");
        await Assert.That(((string?)"1").Map(int.Parse)).IsEqualTo(1);

        await Assert.That(Option.Success().Map(() => "yay", () => "nay")).IsEqualTo("yay");
        await Assert.That(ErrorState.Success().Map(() => "yay", e => "nay")).IsEqualTo("yay");
        await Assert.That(ErrorState.Success<int>().Map(() => "yay", e => "nay")).IsEqualTo("yay");
    }

    [Test]
    public async Task Map_Error_Test()
    {
        await Assert.That(Option.Error<int>().Map(i => i + 1)).IsError();
        await Assert.That(Option.Error<int>().Map(i => Option.Success(i))).IsError();
        await Assert.That(Option.Success(1).Map(i => Option.Error<int>())).IsError();
        await Assert.That(Result.Error<int>().Map(i => i + 1)).IsError();
        await Assert.That(Result.Error<int>(new FormatException()).Map(i => Result.Success(i))).IsErrorOfType<int, FormatException>();
        await Assert.That(Result.Success(1).Map(i => Result.Error<int>(new ArgumentException()))).IsErrorOfType<int, ArgumentException>();
        await Assert.That(Result.Error<int, string>("").Map(i => i + 1)).IsError("");
        await Assert.That(Result.Error<int, string>("5").Map(i => i + 1, error => new Exception(error))).IsError();
        await Assert.That(Result.Error<int, string>("nay").Map(i => Result.Success<int, string>(i))).IsError("nay");
        await Assert.That(Result.Success<int, string>(1).Map(i => Result.Error<int, string>("nay"))).IsError("nay");
        await Assert.That(RefOption.Error<Span<char>>().Map(s => new string(s))).IsError();
        await Assert.That(OptionsMarshall.IsSuccess(RefOption.Error<Span<char>>().Map(s => s))).IsFalse();
        await Assert.That(OptionsMarshall.IsSuccess(Option.Error<string>().MapAsSpan())).IsFalse();
        await Assert.That(new int?().Map(v => v * 2)).IsNull();
        await Assert.That(new int?().Map(v => v.ToString())).IsNull();
        await Assert.That(((string?)null).Map(v => v + "a")).IsNull();
        await Assert.That(((string?)null).Map(int.Parse)).IsNull();

        await Assert.That(Option.Error().Map(() => "yay", () => "nay")).IsEqualTo("nay");
        await Assert.That(ErrorState.Error().Map(() => "yay", e => "nay")).IsEqualTo("nay");
        await Assert.That(ErrorState.Error(1).Map(() => "yay", e => "nay")).IsEqualTo("nay");
    }
}
