namespace Ametrin.Optional.Test;

public sealed class SelectTests
{
    [Test]
    public async Task Select_Success_Test()
    {
        await Assert.That(Option.Success(1).Select(i => i + 1)).IsSuccess(2);
        await Assert.That(Result.Success(1).Select(i => i + 1)).IsSuccess(2);
        await Assert.That(Result.Success<int, string>(1).Select(i => i + 1)).IsSuccess(2);
        await Assert.That(Result.Success<int, int>(1).Select(i => i + 1, error => error.ToString())).IsSuccess(2);
    }

    [Test]
    public async Task Select_Error_Test()
    {
        await Assert.That(Option.Error<int>().Select(i => i + 1)).IsError();
        await Assert.That(Result.Error<int>().Select(i => i + 1)).IsError();
        await Assert.That(Result.Error<int>(new InvalidOperationException()).Select(i => i + 1)).IsErrorOfType<int, InvalidOperationException>();
        await Assert.That(Result.Error<int, string>("").Select(i => i + 1)).IsError("");
        await Assert.That(Result.Error<int, int>(5).Select(i => i + 1, error => error.ToString())).IsError("5");
    }
}
