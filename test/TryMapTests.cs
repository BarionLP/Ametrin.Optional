namespace Ametrin.Optional.Test;

public sealed class TryMapTests
{
    [Test]
    public async Task TryMap_Success_Test()
    {
        await Assert.That(Option.Success("1").TryMap(int.Parse)).IsSuccess(1);
        await Assert.That(Result.Success("1").TryMap(int.Parse)).IsSuccess(1);
        await Assert.That(Result.Success("1").TryMap(int.Parse, e => e.Message)).IsSuccess(1);
        await Assert.That(Result.Success<string, string>("1").TryMap(int.Parse, e => e.Message)).IsSuccess(1);
        await Assert.That(OptionsMarshall.IsSuccess(RefOption.Success<ReadOnlySpan<char>>("1").TryMap(static s => int.Parse(s)))).IsTrue();
        await Assert.That(OptionsMarshall.IsSuccess(Option.Success("1").TryMap(s => s.AsSpan()))).IsTrue();
        await Assert.That(RefOption.Success<ReadOnlySpan<char>>("1").TryMap(s => s.ToString())).IsSuccess("1");

        await Assert.That(new int?(1).TryMap(v => v * 2)).IsEqualTo(2);
        await Assert.That(new int?(1).TryMap(v => v.ToString())).IsEqualTo("1");
        await Assert.That(((string?)"").TryMap(v => v + "a")).IsEqualTo("a");
        await Assert.That(((string?)"1").TryMap(int.Parse)).IsEqualTo(1);
    }

    [Test]
    public async Task TryMap_Success_Exception_Test()
    {
        await Assert.That(Option.Success("z").TryMap(int.Parse)).IsError();
        await Assert.That(Result.Success("z").TryMap(int.Parse)).IsErrorOfType<int, FormatException>();
        await Assert.That(Result.Success("z").TryMap(int.Parse, e => e.Message)).IsError("The input string 'z' was not in a correct format.");
        await Assert.That(Result.Success<string, string>("z").TryMap(int.Parse, e => e.Message)).IsError("The input string 'z' was not in a correct format.");
        await Assert.That(OptionsMarshall.IsSuccess(RefOption.Success<ReadOnlySpan<char>>("z").TryMap(static s => int.Parse(s)))).IsFalse();
        await Assert.That(OptionsMarshall.IsSuccess(Option.Success("z").TryMap<string, ReadOnlySpan<char>>(s => throw new Exception()))).IsFalse();
        await Assert.That(RefOption.Success<ReadOnlySpan<char>>("z").TryMap<ReadOnlySpan<char>, string>(s => throw new Exception())).IsError();

        await Assert.That(new int?(1).TryMap(v => v / 0)).IsNull();
        await Assert.That(new int?(1).TryMap<int, string>(v => throw new Exception())).IsNull();
        await Assert.That(((string?)"").TryMap<string, string>(v => throw new Exception())).IsNull();
        await Assert.That(((string?)"a").TryMap(int.Parse)).IsNull();
    }

    [Test]
    public async Task TryMap_Error_Test()
    {
        await Assert.That(Option.Error<string>().TryMap(int.Parse)).IsError();
        await Assert.That(Result.Error<string>(new NullReferenceException()).TryMap(int.Parse)).IsErrorOfType<int, NullReferenceException>();
        await Assert.That(Result.Error<string>(new Exception("error")).TryMap(int.Parse, e => e.Message)).IsError("error");
        await Assert.That(Result.Error<string, string>("error").TryMap(int.Parse, e => e.Message)).IsError("error");
        await Assert.That(OptionsMarshall.IsSuccess(RefOption.Error<ReadOnlySpan<char>>().TryMap(static s => int.Parse(s)))).IsFalse();
        await Assert.That(OptionsMarshall.IsSuccess(Option.Error<string>().TryMap(s => s.AsSpan()))).IsFalse();
        await Assert.That(RefOption.Error<ReadOnlySpan<char>>().TryMap(s => s.ToString())).IsError();

        await Assert.That(new int?().Map(v => v * 2)).IsNull();
        await Assert.That(new int?().Map(v => v.ToString())).IsNull();
        await Assert.That(((string?)null).Map(v => v + "a")).IsNull();
        await Assert.That(((string?)null).Map(int.Parse)).IsNull();
    }
}
