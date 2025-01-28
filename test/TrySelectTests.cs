namespace Ametrin.Optional.Test;

public sealed class TrySelectTests
{
    [Test]
    public async Task TrySelect_Success_Test()
    {
        await Assert.That(Option.Success("1").TrySelect(int.Parse)).IsSuccess(1);
        await Assert.That(Result.Success("1").TrySelect(int.Parse)).IsSuccess(1);
        await Assert.That(Result.Success("1").TrySelect(int.Parse, e => e.Message)).IsSuccess(1);
        await Assert.That(Result.Success<string, string>("1").TrySelect(int.Parse, e => e.Message)).IsSuccess(1);
        await Assert.That(OptionsMarshall.IsSuccess(RefOption.Success<ReadOnlySpan<char>>("1").TrySelect(static s => int.Parse(s)))).IsTrue();
        await Assert.That(OptionsMarshall.IsSuccess(Option.Success("1").TrySelect(s => s.AsSpan()))).IsTrue();
        await Assert.That(RefOption.Success<ReadOnlySpan<char>>("1").TrySelect(s => s.ToString())).IsSuccess("1");
    }

    [Test]
    public async Task TrySelect_Success_Exception_Test()
    {
        await Assert.That(Option.Success("z").TrySelect(int.Parse)).IsError();
        await Assert.That(Result.Success("z").TrySelect(int.Parse)).IsErrorOfType<int, FormatException>();
        await Assert.That(Result.Success("z").TrySelect(int.Parse, e => e.Message)).IsError("The input string 'z' was not in a correct format.");
        await Assert.That(Result.Success<string, string>("z").TrySelect(int.Parse, e => e.Message)).IsError("The input string 'z' was not in a correct format.");
        await Assert.That(OptionsMarshall.IsSuccess(RefOption.Success<ReadOnlySpan<char>>("z").TrySelect(static s => int.Parse(s)))).IsFalse();
        await Assert.That(OptionsMarshall.IsSuccess(Option.Success("z").TrySelect<string, ReadOnlySpan<char>>(s => throw new Exception()))).IsFalse();
        await Assert.That(RefOption.Success<ReadOnlySpan<char>>("z").TrySelect<ReadOnlySpan<char>, string>(s => throw new Exception())).IsError();
    }

    [Test]
    public async Task TrySelect_Error_Test()
    {
        await Assert.That(Option.Error<string>().TrySelect(int.Parse)).IsError();
        await Assert.That(Result.Error<string>(new NullReferenceException()).TrySelect(int.Parse)).IsErrorOfType<int, NullReferenceException>();
        await Assert.That(Result.Error<string>(new Exception("error")).TrySelect(int.Parse, e => e.Message)).IsError("error");
        await Assert.That(Result.Error<string, string>("error").TrySelect(int.Parse, e => e.Message)).IsError("error");
        await Assert.That(OptionsMarshall.IsSuccess(RefOption.Error<ReadOnlySpan<char>>().TrySelect(static s => int.Parse(s)))).IsFalse();
        await Assert.That(OptionsMarshall.IsSuccess(Option.Error<string>().TrySelect(s => s.AsSpan()))).IsFalse();
        await Assert.That(RefOption.Error<ReadOnlySpan<char>>().TrySelect(s => s.ToString())).IsError();
    }
}
