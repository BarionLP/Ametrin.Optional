namespace Ametrin.Optional.Test;

public sealed class MapErrorTests
{
    [Test]
    public async Task MapError_Success_Test()
    {
        await Assert.That(Result.Success(1).MapError(e => e.Message)).IsSuccess(1);
        await Assert.That(Result.Success(1).MapError(e => e.InnerException!)).IsSuccess(1);
        await Assert.That(Result.Success<int, string>(1).MapError(e => new Exception(e))).IsSuccess(1);
        await Assert.That(Result.Success<int, string>(1).MapError(e => e.GetHashCode())).IsSuccess(1);

        await Assert.That(ErrorState.Success().MapError(e => e.Message)).IsSuccess();
        await Assert.That(ErrorState.Success().MapError(e => e.InnerException!)).IsSuccess();
        await Assert.That(ErrorState.Success<string>().MapError(e => new Exception(e))).IsSuccess();
        await Assert.That(ErrorState.Success<string>().MapError(e => e.GetHashCode())).IsSuccess();
    }

    [Test]
    public async Task MapError_Error_Test()
    {
        await Assert.That(Result.Error<int>(new Exception("hello")).MapError(e => e.Message)).IsError("hello");
        await Assert.That(Result.Error<int>(new Exception(null, new NotImplementedException())).MapError(e => e.InnerException!)).IsErrorOfType<int, NotImplementedException>();
        await Assert.That(Result.Error<int, string>("nay").MapError(e => new Exception(e))).IsError();
        await Assert.That(Result.Error<int, string>("nay").MapError(e => e.GetHashCode())).IsError("nay".GetHashCode());

        await Assert.That(ErrorState.Error(new Exception("hello")).MapError(e => e.Message)).IsError("hello");
        await Assert.That(ErrorState.Error(new Exception(null, new NotImplementedException())).MapError(e => e.InnerException!)).IsErrorOfType<NotImplementedException>();
        await Assert.That(ErrorState.Error("nay").MapError(e => new Exception(e))).IsError();
        await Assert.That(ErrorState.Error("nay").MapError(e => e.GetHashCode())).IsError("nay".GetHashCode());
    }
}
