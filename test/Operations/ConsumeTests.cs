namespace Ametrin.Optional.Test.Operations;

public sealed class ConsumeTests
{
    [Test]
    public void Consume_Success_Test()
    {
        Assert.Throws<SuccessE>(static () => Option.Success().Consume(Success, Error));
        Assert.Throws<SuccessE>(static () => Option.Success(1).Consume(Success, Error));
        Assert.Throws<SuccessE>(static () => Result.Success(1).Consume(Success, Error));
        Assert.Throws<SuccessE>(static () => Result.Success<int, string>(1).Consume(Success, Error));
        Assert.Throws<SuccessE>(static () => ErrorState.Success().Consume(Success, Error));
        Assert.Throws<SuccessE>(static () => ErrorState.Success<string>().Consume(Success, Error));
        Assert.Throws<SuccessE>(static () => RefOption.Success(1).Consume(Success, Error));
    }

    [Test]
    public void Consume_Error_Test()
    {
        Assert.Throws<ErrorE>(static () => Option.Error().Consume(Success, Error));
        Assert.Throws<ErrorE>(static () => Option.Error<int>().Consume(Success, Error));
        Assert.Throws<ErrorE>(static () => Result.Error<int>().Consume(Success, Error));
        Assert.Throws<ErrorE>(static () => Result.Error<int, string>("err").Consume(Success, Error));
        Assert.Throws<ErrorE>(static () => ErrorState.Error().Consume(Success, Error));
        Assert.Throws<ErrorE>(static () => ErrorState.Error("err").Consume(Success, Error));
        Assert.Throws<ErrorE>(static () => RefOption.Error<int>().Consume(Success, Error));
    }

    [Test]
    public void Consume_Arg_Success_Test()
    {
        Assert.Throws<SuccessE>(static () => Option.Success().Consume(1, Success, Error));
        Assert.Throws<SuccessE>(static () => Option.Success(1).Consume(1, Success, Error));
        Assert.Throws<SuccessE>(static () => Result.Success(1).Consume(1, Success, Error));
        Assert.Throws<SuccessE>(static () => Result.Success<int, string>(1).Consume(1, Success, Error));
        Assert.Throws<SuccessE>(static () => ErrorState.Success().Consume(1, Success, Error));
        Assert.Throws<SuccessE>(static () => ErrorState.Success<string>().Consume(1, Success, Error));
        Assert.Throws<SuccessE>(static () => RefOption.Success(1).Consume(1, Success, Error));
    }

    [Test]
    public void Consume_Arg_Error_Test()
    {
        Assert.Throws<ErrorE>(static () => Option.Error().Consume(1, Success, Error));
        Assert.Throws<ErrorE>(static () => Option.Error<int>().Consume(1, Success, Error));
        Assert.Throws<ErrorE>(static () => Result.Error<int>().Consume(1, Success, Error));
        Assert.Throws<ErrorE>(static () => Result.Error<int, string>("err").Consume(1, Success, Error));
        Assert.Throws<ErrorE>(static () => ErrorState.Error().Consume(1, Success, Error));
        Assert.Throws<ErrorE>(static () => ErrorState.Error("err").Consume(1, Success, Error));
        Assert.Throws<ErrorE>(static () => RefOption.Error<int>().Consume(1, Success, Error));
    }

    [Test]
    public void Consume_Success_Success_Test()
    {
        Assert.Throws<SuccessE>(static () => (Option.Success(1), Option.Success(1)).Consume(Success, Error));
        Assert.Throws<SuccessE>(static () => (Result.Success(1), Result.Success(1)).Consume(Success, Error));
    }

    [Test]
    public void Consume_Success_Error_Test()
    {
        Assert.Throws<ErrorE>(static () => (Option.Success(1), Option.Error<int>()).Consume(Success, Error));
        Assert.Throws<ErrorE>(static () => (Result.Success(1), Result.Error<int>()).Consume(Success, Error));
    }

    [Test]
    public void Consume_Error_Success_Test()
    {
        Assert.Throws<ErrorE>(static () => (Option.Error<int>(), Option.Success(1)).Consume(Success, Error));
        Assert.Throws<ErrorE>(static () => (Result.Error<int>(), Result.Success(1)).Consume(Success, Error));
    }

    [Test]
    public void Consume_Error_Error_Test()
    {
        Assert.Throws<ErrorE>(static () => (Option.Error<int>(), Option.Error<int>()).Consume(Success, Error));
        Assert.Throws<ErrorE>(static () => (Result.Error<int>(), Result.Error<int>()).Consume(Success, Error));
    }

    [Test]
    public void Consume_Arg_Success_Success_Test()
    {
        Assert.Throws<SuccessE>(static () => (Option.Success(1), Option.Success(1)).Consume(true, Success, Error));
        Assert.Throws<SuccessE>(static () => (Result.Success(1), Result.Success(1)).Consume(true, Success, Error));
    }

    [Test]
    public void Consume_Arg_Success_Error_Test()
    {
        Assert.Throws<ErrorE>(static () => (Option.Success(1), Option.Error<int>()).Consume(true, Success, Error));
        Assert.Throws<ErrorE>(static () => (Result.Success(1), Result.Error<int>()).Consume(true, Success, Error));
    }

    [Test]
    public void Consume_Arg_Error_Success_Test()
    {
        Assert.Throws<ErrorE>(static () => (Option.Error<int>(), Option.Success(1)).Consume(true, Success, Error));
        Assert.Throws<ErrorE>(static () => (Result.Error<int>(), Result.Success(1)).Consume(true, Success, Error));
    }

    [Test]
    public void Consume_Arg_Error_Error_Test()
    {
        Assert.Throws<ErrorE>(static () => (Option.Error<int>(), Option.Error<int>()).Consume(true, Success, Error));
        Assert.Throws<ErrorE>(static () => (Result.Error<int>(), Result.Error<int>()).Consume(true, Success, Error));
    }

    private static void Success() => throw new SuccessE();
    private static void Success<T1>(T1 v) => throw new SuccessE();
    private static void Success<T1, T2>(T1 v, T2 a) => throw new SuccessE();
    private static void Success<T1, T2, T3>(T1 v, T2 a, T3 c) => throw new SuccessE();

    private static void Error() => throw new ErrorE();
    private static void Error<T1>(T1 v) => throw new ErrorE();
    private static void Error<T1, T2>(T1 v, T2 a) => throw new ErrorE();
}

file sealed class SuccessE : Exception;
file sealed class ErrorE : Exception;