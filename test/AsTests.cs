namespace Ametrin.Optional.Test;

public sealed class AsTests
{
    [Test]
    public async Task As_Success_Test()
    {
        await Assert.That(Option.Success("yay").As<object>()).IsSuccess("yay");
        await Assert.That(Result.Success("yay").As<object>()).IsSuccess("yay");
        await Assert.That(Result.Success(1).As<object>()).IsSuccess(1);
        await Assert.That(Result.Success<string, int>("yay").As<object>()).IsSuccess("yay");
    }
    
    [Test]
    public async Task As_Error_Test()
    {
        await Assert.That(Option.Error<string>().As<object>()).IsError();
        await Assert.That(Result.Error<string>().As<object>()).IsError();
        await Assert.That(Result.Error<string, int>(-1).As<object>()).IsError(-1);
    }
}
