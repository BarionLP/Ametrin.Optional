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
        await Assert.That(Result.Success<int, string>(1).Map(i => Result.Success<int, string>(i))).IsSuccess(1);
        await Assert.That(RefOption.Success<Span<char>>([]).Map(s => new string(s))).IsSuccess("");
        await Assert.That(RefOption.Success<Span<char>>([]).Map(s => Option.Success(new string(s)))).IsSuccess("");
        await Assert.That(OptionsMarshall.IsSuccess(RefOption.Success<Span<char>>([]).Map(s => s))).IsTrue();
        await Assert.That(OptionsMarshall.IsSuccess(RefOption.Success<Span<char>>([]).Map(s => RefOption.Success(s)))).IsTrue();
        await Assert.That(OptionsMarshall.IsSuccess(Option.Success("").MapAsSpan())).IsTrue();
        await Assert.That(new int?(1).Map(v => v * 2)).IsEqualTo(2);
        await Assert.That(new int?(1).Map(v => v.ToString())).IsEqualTo("1");
        await Assert.That(((string?)"").Map(v => v + "a")).IsEqualTo("a");
        await Assert.That(((string?)"1").Map(int.Parse)).IsEqualTo(1);
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
        await Assert.That(Result.Error<int, string>("nay").Map(i => Result.Success<int, string>(i))).IsError("nay");
        await Assert.That(Result.Success<int, string>(1).Map(i => Result.Error<int, string>("nay"))).IsError("nay");
        await Assert.That(RefOption.Error<Span<char>>().Map(s => new string(s))).IsError();
        await Assert.That(RefOption.Error<Span<char>>().Map(s => Option.Success(new string(s)))).IsError();
        await Assert.That(OptionsMarshall.IsSuccess(RefOption.Error<Span<char>>().Map(s => s))).IsFalse();
        await Assert.That(OptionsMarshall.IsSuccess(RefOption.Error<Span<char>>().Map(s => RefOption.Success(s)))).IsFalse();
        await Assert.That(OptionsMarshall.IsSuccess(Option.Error<string>().MapAsSpan())).IsFalse();
        await Assert.That(new int?().Map(v => v * 2)).IsNull();
        await Assert.That(new int?().Map(v => v.ToString())).IsNull();
        await Assert.That(((string?)null).Map(v => v + "a")).IsNull();
        await Assert.That(((string?)null).Map(int.Parse)).IsNull();
    }

    [Test]
    public async Task Map_Arg_Success_Test()
    {
        await Assert.That(Option.Success(1).Map(true, (i, a) => a)).IsSuccess(true);
        await Assert.That(Option.Success(1).Map(true, (i, a) => Option.Success(a))).IsSuccess(true);
        await Assert.That(Result.Success(1).Map(true, (i, a) => a)).IsSuccess(true);
        await Assert.That(Result.Success(1).Map(true, (i, a) => Result.Success(a))).IsSuccess(true);
        await Assert.That(Result.Success<int, string>(1).Map(true, (i, a) => a)).IsSuccess(true);
        await Assert.That(Result.Success<int, string>(1).Map(true, (i, a) => Result.Success<bool, string>(a))).IsSuccess(true);
        await Assert.That(RefOption.Success<Span<char>>([]).Map("notempty", (s, a) => a)).IsSuccess("notempty");
        await Assert.That(RefOption.Success<Span<char>>([]).Map("notempty", (s, a) => Option.Success(a))).IsSuccess("notempty");
        await Assert.That(OptionsMarshall.IsSuccess(RefOption.Success<Span<char>>([]).Map(true, (s, a) => s))).IsTrue();
        await Assert.That(OptionsMarshall.IsSuccess(RefOption.Success<Span<char>>([]).Map(true, (s, a) => RefOption.Success(s)))).IsTrue();
        await Assert.That(OptionsMarshall.IsSuccess(Option.Success("").Map(true, (v, a) => v.AsSpan()))).IsTrue();
        await Assert.That(new int?(1).Map(true, (v, a) => a)).IsEqualTo(true);
        await Assert.That(new int?(1).Map(true, (v, a) => a.ToString())).IsEqualTo("True");
        await Assert.That(((string?)"").Map(true, (v, a) => a.ToString())).IsEqualTo("True");
        await Assert.That(((string?)"1").Map(true, (v, a) => a)).IsEqualTo(true);
    }

    [Test]
    public async Task Map_Arg_Error_Test()
    {
        await Assert.That(Option.Error<int>().Map(true, (i, a) => i + 1)).IsError();
        await Assert.That(Option.Error<int>().Map(true, (i, a) => Option.Success(i))).IsError();
        await Assert.That(Option.Success(1).Map(true, (i, a) => Option.Error<int>())).IsError();
        await Assert.That(Result.Error<int>().Map(true, (i, a) => i + 1)).IsError();
        await Assert.That(Result.Error<int>(new FormatException()).Map(true, (i, a) => Result.Success(i))).IsErrorOfType<int, FormatException>();
        await Assert.That(Result.Success(1).Map(true, (i, a) => Result.Error<int>(new ArgumentException()))).IsErrorOfType<int, ArgumentException>();
        await Assert.That(Result.Error<int, string>("").Map(true, (i, a) => i + 1)).IsError("");
        await Assert.That(Result.Error<int, string>("nay").Map(true, (i, a) => Result.Success<int, string>(i))).IsError("nay");
        await Assert.That(Result.Success<int, string>(1).Map(true, (i, a) => Result.Error<int, string>("nay"))).IsError("nay");
        await Assert.That(RefOption.Error<Span<char>>().Map(true, (s, a) => new string(s))).IsError();
        await Assert.That(RefOption.Error<Span<char>>().Map(true, (s, a) => Option.Success(new string(s)))).IsError();
        await Assert.That(OptionsMarshall.IsSuccess(RefOption.Error<Span<char>>().Map(true, (s, a) => s))).IsFalse();
        await Assert.That(OptionsMarshall.IsSuccess(RefOption.Error<Span<char>>().Map(true, (s, a) => RefOption.Success(s)))).IsFalse();
        await Assert.That(OptionsMarshall.IsSuccess(Option.Error<string>().Map(true, (v, a) => v.AsSpan()))).IsFalse();
        await Assert.That(new int?().Map(true, (v, a) => v * 2)).IsNull();
        await Assert.That(new int?().Map(true, (v, a) => v.ToString())).IsNull();
        await Assert.That(((string?)null).Map(true, (v, a) => v + "a")).IsNull();
        await Assert.That(((string?)null).Map(true, (v, a) => int.Parse(v))).IsNull();
    }
}
