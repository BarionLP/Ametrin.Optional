namespace Ametrin.Optional.Test;

public sealed class RequireTests
{
    [Test]
    public async Task Success_Require_True_Test()
    {
        await Assert.That(Option.Success(1).Require(i => i != 2)).IsSuccess(1);
        await Assert.That(Result.Success(1).Require(i => i != 2)).IsSuccess(1);
        await Assert.That(Result.Success(1).Require(i => i != 2, static i => new ArgumentException($"{i} was 2"))).IsSuccess(1);
        await Assert.That(Result.Success<int, string>(1).Require(i => i != 2, "was 2")).IsSuccess(1);
        await Assert.That(Result.Success<int, string>(1).Require(i => i != 2, static i => $"{i} was 2")).IsSuccess(1);
        await Assert.That(OptionsMarshall.IsSuccess(RefOption.Success<Span<char>>([]).Require(s => s.IsEmpty))).IsTrue();

        await Assert.That(new int?(1).Require(v => v == 1)).IsEqualTo(1);
        await Assert.That("".Require(string.IsNullOrEmpty)).IsEqualTo("");
    }

    [Test]
    public async Task Success_Require_False_Test()
    {
        await Assert.That(Option.Success(1).Require(static i => i == 2)).IsError();
        await Assert.That(Result.Success(1).Require(static i => i == 2)).IsError();
        await Assert.That(Result.Success(1).Require(static i => i == 2, static i => new ArgumentException($"{i} was not 2"))).IsErrorOfType<int, ArgumentException>();
        await Assert.That(Result.Success<int, string>(1).Require(static i => i == 2, "was not 2")).IsError("was not 2");
        await Assert.That(Result.Success<int, string>(1).Require(static i => i == 2, static i => $"{i} was not 2")).IsError("1 was not 2");
        await Assert.That(OptionsMarshall.IsSuccess(RefOption.Success<Span<char>>(['a']).Require(s => s.IsEmpty))).IsFalse();


        await Assert.That(new int?(1).Require(v => v != 1)).IsNull();
        await Assert.That("1".Require(string.IsNullOrEmpty)).IsNull();
    }

    [Test]
    public async Task Error_Require_Test()
    {
        await Assert.That(Option.Error<int>().Require(static i => i != 2)).IsEqualTo(default);
        await Assert.That(Result.Error<int>(new InvalidOperationException()).Require(static i => i != 2)).IsErrorOfType<int, InvalidOperationException>();
        await Assert.That(Result.Error<int>(new InvalidOperationException()).Require(static i => i != 2, static i => new ArgumentException($"{i} was 2"))).IsErrorOfType<int, InvalidOperationException>();
        await Assert.That(Result.Error<int, string>("error").Require(static i => i != 2, "was 2")).IsError("error");
        await Assert.That(Result.Error<int, string>("error").Require(static i => i != 2, static i => $"{i} was 2")).IsError("error");
        await Assert.That(OptionsMarshall.IsSuccess(RefOption.Error<Span<char>>().Require(s => s.IsEmpty))).IsFalse();


        await Assert.That(new int?().Require(v => v != 1)).IsNull();
        await Assert.That(((string?)null).Require(string.IsNullOrEmpty)).IsNull();
    }
    
    [Test]
    public async Task Success_Require_Arg_True_Test()
    {
        await Assert.That(Option.Success(1).Require(2, static (i, arg) => i != arg)).IsSuccess(1);
        await Assert.That(Result.Success(1).Require(2, static (i, arg) => i != arg)).IsSuccess(1);
        await Assert.That(Result.Success(1).Require(2, static (i, arg) => i != arg, static (i, arg) => new ArgumentException($"{i} was {arg}"))).IsSuccess(1);
        await Assert.That(Result.Success<int, string>(1).Require(2, static (i, arg) => i != arg, "was 2")).IsSuccess(1);
        await Assert.That(Result.Success<int, string>(1).Require(2, static (i, arg) => i != arg, static (i, arg) => $"{i} was {arg}")).IsSuccess(1);
        await Assert.That(OptionsMarshall.IsSuccess(RefOption.Success<Span<char>>([]).Require(0, static (s, arg) => s.Length == arg))).IsTrue();

        await Assert.That(new int?(1).Require(1, static (v, arg) => v == arg)).IsEqualTo(1);
        await Assert.That("".Require("", static (v, arg) => v == arg)).IsEqualTo("");
    }

    [Test]
    public async Task Success_Require_Arg_False_Test()
    {
        await Assert.That(Option.Success(1).Require(2, static (i, arg) => i == arg)).IsError();
        await Assert.That(Result.Success(1).Require(2, static (i, arg) => i == arg)).IsError();
        await Assert.That(Result.Success(1).Require(2, static (i, arg) => i == arg, static (i, arg) => new ArgumentException($"{i} was not {arg}"))).IsErrorOfType<int, ArgumentException>();
        await Assert.That(Result.Success<int, string>(1).Require(2, static (i, arg) => i == arg, "was not 2")).IsError("was not 2");
        await Assert.That(Result.Success<int, string>(1).Require(2, static (i, arg) => i == arg, static (i, arg) => $"{i} was not {arg}")).IsError("1 was not 2");
        await Assert.That(OptionsMarshall.IsSuccess(RefOption.Success<Span<char>>(['a']).Require(0, static (s, arg) => s.Length == arg))).IsFalse();


        await Assert.That(new int?(1).Require(1, static (v, arg) => v != arg)).IsNull();
        await Assert.That("1".Require("", static (v, arg) => v == arg)).IsNull();
    }

    [Test]
    public async Task Error_Require_Arg_Test()
    {
        await Assert.That(Option.Error<int>().Require(2, static (i, arg) => i != arg)).IsEqualTo(default);
        await Assert.That(Result.Error<int>(new InvalidOperationException()).Require(2, static (i, arg) => i != arg)).IsErrorOfType<int, InvalidOperationException>();
        await Assert.That(Result.Error<int>(new InvalidOperationException()).Require(2, static (i, arg) => i != arg, static (i, arg) => new ArgumentException($"{i} was {arg}"))).IsErrorOfType<int, InvalidOperationException>();
        await Assert.That(Result.Error<int, string>("error").Require(2, static (i, arg) => i != arg, "was 2")).IsError("error");
        await Assert.That(Result.Error<int, string>("error").Require(2, static (i, arg) => i != arg, static (i, arg) => $"{i} was {arg}")).IsError("error");
        await Assert.That(OptionsMarshall.IsSuccess(RefOption.Error<Span<char>>().Require(0, static (s, arg) => s.Length == arg))).IsFalse();


        await Assert.That(new int?().Require(1, static (v, arg) => v == arg)).IsNull();
        await Assert.That(((string?)null).Require("", static (v, arg) => v == arg)).IsNull();
    }

    [Test]
    public async Task Success_Require_Cast_True_Test()
    {
        await Assert.That(Option.Success<object>(string.Empty).Require<string>()).IsSuccess(string.Empty);
        await Assert.That(Result.Success<object>(string.Empty).Require<string>()).IsSuccess(string.Empty);
        await Assert.That(Result.Success<object>(string.Empty).Require<string>(static val => new InvalidOperationException())).IsSuccess(string.Empty);
        await Assert.That(Result.Success<object, int>(string.Empty).Require<string>(error: -1)).IsSuccess(string.Empty);
        await Assert.That(Result.Success<object, int>(string.Empty).Require<string>(error: static val => -2)).IsSuccess(string.Empty);
    }

    [Test]
    public async Task Success_Require_Cast_False_Test()
    {
        await Assert.That(Option.Success<object>(1).Require<string>()).IsError();
        await Assert.That(Result.Success<object>(1).Require<string>()).IsErrorOfType<string, InvalidCastException>();
        await Assert.That(Result.Success<object>(1).Require<string>(static val => new InvalidOperationException())).IsErrorOfType<string, InvalidOperationException>();
        await Assert.That(Result.Success<object, int>(1).Require<string>(error: -1)).IsError(-1);
        await Assert.That(Result.Success<object, int>(1).Require<string>(error: static val => -2)).IsError(-2);
    }

    [Test]
    public async Task Error_Require_Cast_Test()
    {
        await Assert.That(Option.Error<object>().Require<string>()).IsError();
        await Assert.That(Result.Error<object>().Require<string>()).IsErrorNotOfType<string, InvalidOperationException>();
        await Assert.That(Result.Error<object>().Require<string>(static val => new InvalidOperationException())).IsErrorNotOfType<string, InvalidOperationException>();
        await Assert.That(Result.Error<object, int>(1).Require<string>(error: -1)).IsError(1);
        await Assert.That(Result.Error<object, int>(1).Require<string>(error: static val => -2)).IsError(1);
    }
}
