namespace Ametrin.Optional.Test;

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

    private static void Success() => throw new SuccessE();
    private static void Success<T>(T v) => throw new SuccessE();
    private static void Success<T, T2>(T v, T2 a) => throw new SuccessE();
    private static void Error() => throw new ErrorE();
    private static void Error<T>(T v) => throw new ErrorE();
    private static void Error<T, T2>(T v, T2 a) => throw new ErrorE();
}

internal sealed class SuccessE: Exception;
internal sealed class ErrorE : Exception;