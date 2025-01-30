using TUnit.Assertions.AssertConditions.Throws;

namespace Ametrin.Optional.Test;

public sealed class ConstructionTests
{
    // some tests also test the equality operators. Thats why they don't use IsEqualTo
    [Test]
    public async Task Option_Test()
    {
        await Assert.That(new Option()).IsError();
        await Assert.That(Option.Success() == true).IsTrue();
        await Assert.That(Option.Success()).IsSuccess();
        await Assert.That(Option.Error()).IsError();

        var originalSuccess = Option.Success();
        await Assert.That(new Option(originalSuccess)).IsEqualTo(originalSuccess);

        var originalError = Option.Error();
        await Assert.That(new Option(originalError)).IsEqualTo(originalError);
    }

    [Test]
    public async Task Option_Generic_Test()
    {
        await Assert.That(new Option<string>()).IsError();
        await Assert.That(Option.Of<string>(null) == Option.Error<string>()).IsTrue();
        await Assert.That(Option.Of<int>(null) == Option.Error<int>()).IsTrue();
        await Assert.That(Option.Of(1)).IsSuccess(1);
        await Assert.That(Option.Error<int>() == default(Option<int>)).IsTrue();
        await Assert.That(() => Option.Success<string>(null!)).Throws<ArgumentNullException>();

        var originalSuccess = Option.Success("hello");
        await Assert.That(new Option<string>(originalSuccess)).IsEqualTo(originalSuccess);

        var originalError = Option.Error<string>();
        await Assert.That(new Option<string>(originalError)).IsEqualTo(originalError);
    }

    [Test]
    public async Task Result_Test()
    {
        await Assert.That(new Result<string>()).IsError();
        await Assert.That(Result.Of("hello")).IsSuccess("hello");
        await Assert.That(Result.Of<string>(null)).IsError();
        await Assert.That(Result.Of(1)).IsSuccess(1);
        await Assert.That(Result.Of<int>(null)).IsError();
        await Assert.That(Result.Of("hello", () => new Exception())).IsSuccess();
        await Assert.That(Result.Of<string>(null, () => new Exception())).IsError();
        await Assert.That(() => Result.Success<string>(null!)).Throws<ArgumentNullException>();

        var originalSuccess = Result.Success("hello");
        await Assert.That(new Result<string>(originalSuccess)).IsEqualTo(originalSuccess);

        var originalError = Result.Error<string>();
        await Assert.That(new Result<string>(originalError)).IsEqualTo(originalError);

        await Assert.That(Result.Try(() => 1)).IsSuccess(1);
        await Assert.That(Result.Try<int>(() => throw new Exception())).IsError();
    }

    [Test]
    public async Task Result_Generic_Test()
    {
        await Assert.That(new Result<string, int>()).IsError();
        await Assert.That(() => Result.Success<string, int>(null!)).Throws<ArgumentNullException>();
        await Assert.That(() => Result.Error<int, string>(null!)).Throws<ArgumentNullException>();

        var originalSuccess = Result.Success<string, int>("hello");
        await Assert.That(new Result<string, int>(originalSuccess)).IsEqualTo(originalSuccess);

        var originalError = Result.Error<string, int>(1);
        await Assert.That(new Result<string, int>(originalError)).IsEqualTo(originalError);
    }

    [Test]
    public async Task ErrorState_Test()
    {
        await Assert.That(ErrorState.Success()).IsSuccess();
        await Assert.That(ErrorState.Error()).IsErrorOfType<Exception>();
        await Assert.That(ErrorState.Error(new ArgumentException())).IsErrorOfType<ArgumentException>();

        var originalSuccess = ErrorState.Success<string>();
        await Assert.That(new ErrorState<string>(originalSuccess)).IsEqualTo(originalSuccess);

        var originalError = ErrorState.Error("ups");
        await Assert.That(new ErrorState<string>(originalError)).IsEqualTo(originalError);
    }

    [Test]
    public async Task ErrorState_Generic_Test()
    {
        await Assert.That(ErrorState.Success<int>()).IsSuccess();
        await Assert.That(ErrorState.Error(1)).IsError(1);

        var originalSuccess = ErrorState.Success();
        await Assert.That(new ErrorState(originalSuccess)).IsEqualTo(originalSuccess);

        var originalError = ErrorState.Error();
        await Assert.That(new ErrorState(originalError)).IsEqualTo(originalError);
    }

    [Test]
    public async Task RefOption_Generic_Test()
    {
        await Assert.That(OptionsMarshall.IsSuccess(new RefOption<ReadOnlySpan<char>>())).IsFalse();
        await Assert.That(RefOption.Success("".AsSpan()).OrThrow() == "").IsTrue();

        var value = "hello";
        var alt = "world";
        await Assert.That(new RefOption<ReadOnlySpan<char>>(RefOption.Success(value.AsSpan())).Or(alt) == value).IsTrue();
        await Assert.That(new RefOption<ReadOnlySpan<char>>(RefOption.Error<ReadOnlySpan<char>>()).Or(alt) == alt).IsTrue();
    }
}
