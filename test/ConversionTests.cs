namespace Ametrin.Optional.Test;

public sealed class ConversionTests
{
    [Test]
    public async Task Option_Test()
    {
        var success = Option.Success("hello");
        var error = Option.Error<string>();

        await Assert.That(success.ToResult()).IsSuccess("hello");
        await Assert.That(success.ToResult(static () => new InvalidCastException())).IsSuccess("hello");
        await Assert.That(error.ToResult()).IsError();
        await Assert.That(error.ToResult(static () => new InvalidCastException())).IsErrorOfType<string, InvalidCastException>();

        await Assert.That(success.ToResult(static () => -1)).IsSuccess("hello");
        await Assert.That(success.ToResult(-1)).IsSuccess("hello");
        await Assert.That(error.ToResult(static () => -1)).IsError(-1);
        await Assert.That(error.ToResult(-1)).IsError(-1);
    }

    [Test]
    public async Task Option_Test_Async()
    {
        var success = Task.FromResult(Option.Success("hello"));
        var error = Task.FromResult(Option.Error<string>());

        await Assert.That(success.ToResultAsync()).IsSuccess("hello");
        await Assert.That(success.ToResultAsync(static () => new InvalidCastException())).IsSuccess("hello");
        await Assert.That(error.ToResultAsync()).IsError();
        await Assert.That(error.ToResultAsync(static () => new InvalidCastException())).IsErrorOfType<string, InvalidCastException>();

        await Assert.That(success.ToResultAsync(static () => -1)).IsSuccess("hello");
        await Assert.That(success.ToResultAsync(-1)).IsSuccess("hello");
        await Assert.That(error.ToResultAsync(static () => -1)).IsError(-1);
        await Assert.That(error.ToResultAsync(-1)).IsError(-1);
    }

    [Test]
    public async Task Result_Test()
    {
        var success = Result.Success("hello");
        var error = Result.Error<string>(new InvalidTimeZoneException());

        await Assert.That(success.ToOption()).IsSuccess("hello");
        await Assert.That(error.ToOption()).IsError();

        await Assert.That(success.ToErrorState()).IsSuccess();
        await Assert.That(error.ToErrorState()).IsErrorOfType<InvalidTimeZoneException>();
    }

    [Test]
    public async Task Result_Test_Async()
    {
        var success = Task.FromResult(Result.Success("hello"));
        var error = Task.FromResult(Result.Error<string>(new InvalidTimeZoneException()));

        await Assert.That(success.ToOptionAsync()).IsSuccess("hello");
        await Assert.That(error.ToOptionAsync()).IsError();

        await Assert.That(success.ToErrorStateAsync()).IsSuccess();
        await Assert.That(error.ToErrorStateAsync()).IsErrorOfType<InvalidTimeZoneException>();
    }


    [Test]
    public async Task Result_Generic_Test()
    {
        var success = Result.Success<string, int>("hello");
        var error = Result.Error<string, int>(-1);

        await Assert.That(success.ToOption()).IsSuccess("hello");
        await Assert.That(error.ToOption()).IsError();

        await Assert.That(success.ToErrorState()).IsSuccess();
        await Assert.That(error.ToErrorState()).IsError();
    }

    [Test]
    public async Task Result_Generic_Test_Async()
    {
        var success = Task.FromResult(Result.Success<string, int>("hello"));
        var error = Task.FromResult(Result.Error<string, int>(-1));

        await Assert.That(success.ToOptionAsync()).IsSuccess("hello");
        await Assert.That(error.ToOptionAsync()).IsError();

        await Assert.That(success.ToErrorStateAsync()).IsSuccess();
        await Assert.That(error.ToErrorStateAsync()).IsError(-1);
    }

    [Test]
    public async Task ErrorState_Test()
    {
        var success = ErrorState.Success();
        var error = ErrorState.Error(new InvalidOperationException());

        await Assert.That(success.ToResult(1)).IsSuccess(1);
        await Assert.That(success.ToResult(static () => 1)).IsSuccess(1);
        await Assert.That(success.ToResultAsync(static () => Task.FromResult(1))).IsSuccess(1);
        await Assert.That(error.ToResult(1)).IsErrorOfType<int, InvalidOperationException>();
        await Assert.That(error.ToResult(static () => 1)).IsErrorOfType<int, InvalidOperationException>();
        await Assert.That(error.ToResultAsync(static () => Task.FromResult(1))).IsErrorOfType<int, InvalidOperationException>();
    }

    [Test]
    public async Task ErrorState_Test_Async()
    {
        var success = Task.FromResult(ErrorState.Success());
        var error = Task.FromResult(ErrorState.Error(new InvalidOperationException()));

        await Assert.That(success.ToResultAsync(1)).IsSuccess(1);
        await Assert.That(success.ToResultAsync(static () => 1)).IsSuccess(1);
        await Assert.That(success.ToResultAsync(static () => Task.FromResult(1))).IsSuccess(1);
        await Assert.That(error.ToResultAsync(1)).IsErrorOfType<int, InvalidOperationException>();
        await Assert.That(error.ToResultAsync(static () => 1)).IsErrorOfType<int, InvalidOperationException>();
        await Assert.That(error.ToResultAsync(static () => Task.FromResult(1))).IsErrorOfType<int, InvalidOperationException>();
    }

    [Test]
    public async Task ErrorState_Generic_Test()
    {
        var success = ErrorState.Success<string>();
        var error = ErrorState.Error("nay");

        await Assert.That(success.ToResult(1)).IsSuccess(1);
        await Assert.That(success.ToResult(static () => 1)).IsSuccess(1);
        await Assert.That(success.ToResultAsync(static () => Task.FromResult(1))).IsSuccess(1);
        await Assert.That(error.ToResult(1)).IsError("nay");
        await Assert.That(error.ToResult(static () => 1)).IsError("nay");
        await Assert.That(error.ToResultAsync(static () => Task.FromResult(1))).IsError("nay");
    }

    [Test]
    public async Task ErrorState_Generic_Test_Async()
    {
        var success = Task.FromResult(ErrorState.Success<string>());
        var error = Task.FromResult(ErrorState.Error("nay"));

        await Assert.That(success.ToResultAsync(1)).IsSuccess(1);
        await Assert.That(success.ToResultAsync(static () => 1)).IsSuccess(1);
        await Assert.That(success.ToResultAsync(static () => Task.FromResult(1))).IsSuccess(1);
        await Assert.That(error.ToResultAsync(1)).IsError("nay");
        await Assert.That(error.ToResultAsync(static () => 1)).IsError("nay");
        await Assert.That(error.ToResultAsync(static () => Task.FromResult(1))).IsError("nay");
    }
}
