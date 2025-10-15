namespace Ametrin.Optional.Test;

public sealed class TryMapAsyncTests
{
#pragma warning disable CS1998 // lacks await operators
    [Test]
    public async Task TryMapAsync_Success_Test()
    {
        await Assert.That(await Option.Success(1).TryMapAsync(async i => i + 1)).IsSuccess(2);
        await Assert.That(await Result.Success(1).TryMapAsync(async i => i + 1)).IsSuccess(2);
        await Assert.That(await Result.Success<int, string>(1).TryMapAsync(async i => i + 1, e => e.Message)).IsSuccess(2);
    }

    [Test]
    public async Task Task_TryMapAsync_Success_Test()
    {
        var optionTask = Task.FromResult(Option.Success(1));
        var resultTask = Task.FromResult(Result.Success(1));
        var result2Task = Task.FromResult(Result.Success<int, string>(1));
        await Assert.That(await optionTask.TryMapAsync(async i => i + 1)).IsSuccess(2);
        await Assert.That(await resultTask.TryMapAsync(async i => i + 1)).IsSuccess(2);
        await Assert.That(await result2Task.TryMapAsync(async i => i + 1, e => e.Message)).IsSuccess(2);


        await Assert.That(await optionTask.TryMapAsync(i => i + 1)).IsSuccess(2);
        await Assert.That(await optionTask.TryMapAsync(i => Option.Success(i))).IsSuccess(1);
        await Assert.That(await resultTask.TryMapAsync(i => i + 1)).IsSuccess(2);
        await Assert.That(await resultTask.TryMapAsync(i => Result.Success(i))).IsSuccess(1);
        await Assert.That(await result2Task.TryMapAsync(i => i + 1, e => e.Message)).IsSuccess(2);
        await Assert.That(await result2Task.TryMapAsync(i => Result.Success<int, string>(i), e => e.Message)).IsSuccess(1);
    }

    [Test]
    public async Task TryMapAsync_Success_Throws_Test()
    {
        await Assert.That(await Option.Success(1).TryMapAsync(async i => i / 0)).IsError();
        await Assert.That(await Result.Success(1).TryMapAsync(async i => i / 0)).IsErrorOfType<int, DivideByZeroException>();
        await Assert.That(await Result.Success<int, string>(1).TryMapAsync(async i => i / 0, e => e.Message)).IsError("Attempted to divide by zero.");
    }

    [Test]
    public async Task MapAsnyc_Error_Test()
    {
        await Assert.That(await Option.Error<int>().TryMapAsync(async i => i + 1)).IsError();
        await Assert.That(await Result.Error<int>().TryMapAsync(async i => i + 1)).IsError();
        await Assert.That(await Result.Error<int, string>("").TryMapAsync(async i => i + 1, e => e.Message)).IsError("");
    }
}
