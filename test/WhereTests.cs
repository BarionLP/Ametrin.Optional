namespace Ametrin.Optional.Test;

public sealed class WhereTests
{
    [Test]
    public async Task Success_Where_True_Test()
    {
        await Assert.That(Option.Success(1).Where(i => i != 2)).IsSuccess(1);
        await Assert.That(Result.Success(1).Where(i => i != 2)).IsSuccess(1);
        await Assert.That(Result.Success(1).Where(i => i != 2, static i => new ArgumentException($"{i} was 2"))).IsSuccess(1);
        await Assert.That(Result.Success<int, string>(1).Where(i => i != 2, "was 2")).IsSuccess(1);
        await Assert.That(Result.Success<int, string>(1).Where(i => i != 2, static i => $"{i} was 2")).IsSuccess(1);
        await Assert.That(OptionsMarshall.IsSuccess(RefOption.Success<Span<char>>([]).Where(s => s.IsEmpty))).IsTrue();

        await Assert.That(new int?(1).Where(v => v == 1)).IsEqualTo(1);
        await Assert.That("".Where(string.IsNullOrEmpty)).IsEqualTo("");
    }

    [Test]
    public async Task Success_Where_False_Test()
    {
        await Assert.That(Option.Success(1).Where(i => i == 2)).IsError();
        await Assert.That(Result.Success(1).Where(i => i == 2)).IsError();
        await Assert.That(Result.Success(1).Where(i => i == 2, static i => new ArgumentException($"{i} was not 2"))).IsErrorOfType<int, ArgumentException>();
        await Assert.That(Result.Success<int, string>(1).Where(i => i == 2, "was not 2")).IsError("was not 2");
        await Assert.That(Result.Success<int, string>(1).Where(i => i == 2, static i => $"{i} was not 2")).IsError("1 was not 2");
        await Assert.That(OptionsMarshall.IsSuccess(RefOption.Success<Span<char>>(['a']).Where(s => s.IsEmpty))).IsFalse();


        await Assert.That(new int?(1).Where(v => v != 1)).IsNull();
        await Assert.That("1".Where(string.IsNullOrEmpty)).IsNull();
    }

    [Test]
    public async Task Error_Where_Test()
    {
        await Assert.That(Option.Error<int>().Where(i => i != 2)).IsEqualTo(default);
        await Assert.That(Result.Error<int>(new InvalidOperationException()).Where(i => i != 2)).IsErrorOfType<int, InvalidOperationException>();
        await Assert.That(Result.Error<int>(new InvalidOperationException()).Where(i => i != 2, static i => new ArgumentException($"{i} was 2"))).IsErrorOfType<int, InvalidOperationException>();
        await Assert.That(Result.Error<int, string>("error").Where(i => i != 2, "was 2")).IsError("error");
        await Assert.That(Result.Error<int, string>("error").Where(i => i != 2, static i => $"{i} was 2")).IsError("error");
        await Assert.That(OptionsMarshall.IsSuccess(RefOption.Error<Span<char>>().Where(s => s.IsEmpty))).IsFalse();


        await Assert.That(new int?().Where(v => v != 1)).IsNull();
        await Assert.That(((string?)null).Where(string.IsNullOrEmpty)).IsNull();
    }

    [Test]
    public async Task Success_Where_Cast_True_Test()
    {
        await Assert.That(Option.Success<object>(string.Empty).Where<string>()).IsSuccess(string.Empty);
        await Assert.That(Result.Success<object>(string.Empty).Where<string>()).IsSuccess(string.Empty);
        await Assert.That(Result.Success<object>(string.Empty).Where<string>(static val => new InvalidOperationException())).IsSuccess(string.Empty);
        await Assert.That(Result.Success<object, int>(string.Empty).Where<string>(error: -1)).IsSuccess(string.Empty);
        await Assert.That(Result.Success<object, int>(string.Empty).Where<string>(error: static val => -2)).IsSuccess(string.Empty);
    }

    [Test]
    public async Task Success_Where_Cast_False_Test()
    {
        await Assert.That(Option.Success<object>(1).Where<string>()).IsError();
        await Assert.That(Result.Success<object>(1).Where<string>()).IsErrorOfType<string, InvalidCastException>();
        await Assert.That(Result.Success<object>(1).Where<string>(static val => new InvalidOperationException())).IsErrorOfType<string, InvalidOperationException>();
        await Assert.That(Result.Success<object, int>(1).Where<string>(error: -1)).IsError(-1);
        await Assert.That(Result.Success<object, int>(1).Where<string>(error: static val => -2)).IsError(-2);
    }

    [Test]
    public async Task Error_Where_Cast_Test()
    {
        await Assert.That(Option.Error<object>().Where<string>()).IsError();
        await Assert.That(Result.Error<object>().Where<string>()).IsErrorNotOfType<string, InvalidOperationException>();
        await Assert.That(Result.Error<object>().Where<string>(static val => new InvalidOperationException())).IsErrorNotOfType<string, InvalidOperationException>();
        await Assert.That(Result.Error<object, int>(1).Where<string>(error: -1)).IsError(1);
        await Assert.That(Result.Error<object, int>(1).Where<string>(error: static val => -2)).IsError(1);
    }
}
