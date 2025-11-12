namespace Ametrin.Optional.Test.Operations;

public sealed class MapAsyncTests
{
#pragma warning disable CS1998 // lacks await operators
    [Test]
    public async Task MapAsync_Success_Test()
    {
        await Assert.That(await Option.Success(1).MapAsync(async i => i + 1)).IsSuccess(2);
        await Assert.That(await Option.Success(1).MapAsync(async i => Option.Success(i))).IsSuccess(1);
        await Assert.That(await Result.Success(1).MapAsync(async i => i + 1)).IsSuccess(2);
        await Assert.That(await Result.Success(1).MapAsync(async i => Result.Success(i))).IsSuccess(1);
        await Assert.That(await Result.Success<int, string>(1).MapAsync(async i => i + 1)).IsSuccess(2);
        await Assert.That(await Result.Success<int, string>(1).MapAsync(async i => Result.Success<int, string>(i))).IsSuccess(1);
    }

    [Test]
    public async Task Task_MapAsync_Success_Test()
    {
        var optionTask = Task.FromResult(Option.Success(1));
        var resultTask = Task.FromResult(Result.Success(1));
        var result2Task = Task.FromResult(Result.Success<int, string>(1));
        await Assert.That(await optionTask.MapAsync(async i => i + 1)).IsSuccess(2);
        await Assert.That(await optionTask.MapAsync(async i => Option.Success(i))).IsSuccess(1);
        await Assert.That(await resultTask.MapAsync(async i => i + 1)).IsSuccess(2);
        await Assert.That(await resultTask.MapAsync(async i => Result.Success(i))).IsSuccess(1);
        await Assert.That(await result2Task.MapAsync(async i => i + 1)).IsSuccess(2);
        await Assert.That(await result2Task.MapAsync(async i => Result.Success<int, string>(i))).IsSuccess(1);


        await Assert.That(await optionTask.MapAsync(i => i + 1)).IsSuccess(2);
        await Assert.That(await optionTask.MapAsync(i => Option.Success(i))).IsSuccess(1);
        await Assert.That(await resultTask.MapAsync(i => i + 1)).IsSuccess(2);
        await Assert.That(await resultTask.MapAsync(i => Result.Success(i))).IsSuccess(1);
        await Assert.That(await result2Task.MapAsync(i => i + 1)).IsSuccess(2);
        await Assert.That(await result2Task.MapAsync(i => Result.Success<int, string>(i))).IsSuccess(1);
    }

    [Test]
    public async Task MapAsnyc_Error_Test()
    {
        await Assert.That(await Option.Error<int>().MapAsync(async i => i + 1)).IsError();
        await Assert.That(await Option.Error<int>().MapAsync(async i => Option.Success(i))).IsError();
        await Assert.That(await Option.Success(1).MapAsync(async i => Option.Error<int>())).IsError();
        await Assert.That(await Result.Error<int>().MapAsync(async i => i + 1)).IsError();
        await Assert.That(await Result.Error<int>(new FormatException()).MapAsync(async i => Result.Success(i))).IsErrorOfType<int, FormatException>();
        await Assert.That(await Result.Success(1).MapAsync(async i => Result.Error<int>(new ArgumentException()))).IsErrorOfType<int, ArgumentException>();
        await Assert.That(await Result.Error<int, string>("").MapAsync(async i => i + 1)).IsError("");
        await Assert.That(await Result.Error<int, string>("nay").MapAsync(async i => Result.Success<int, string>(i))).IsError("nay");
        await Assert.That(await Result.Success<int, string>(1).MapAsync(async i => Result.Error<int, string>("nay"))).IsError("nay");
    }
}
