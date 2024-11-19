namespace Ametrin.Optional.Test;

public sealed class ConstructionTests
{
    [Test]
    public async Task Option_Test()
    {
        await Assert.That(Option.Of<string>(null!) == Option.Error<string>()).IsTrue();
        await Assert.That(Option.Of<int>(null) == Option.Error<int>()).IsTrue();
        await Assert.That(Option.Of(1)).IsSuccess(1);
        await Assert.That(Option.Error<int>() == default(Option<int>)).IsTrue();
        await Assert.That(() => Option.Success<string>(null!)).Throws<ArgumentNullException>();
    }

    [Test]
    public async Task Result_Test()
    {
        await Assert.That(Result.Of<string>(null)).IsError();
        await Assert.That(() => Result.Success<string>(null!)).Throws<ArgumentNullException>();

        await Assert.That(() => Result.Success<string, int>(null!)).Throws<ArgumentNullException>();
        await Assert.That(() => Result.Error<int, string>(null!)).Throws<ArgumentNullException>();
    }

    [Test]
    public async Task ErrorState_Test()
    {
        await Assert.That(ErrorState.Success()).IsSuccess();
        await Assert.That(ErrorState.Error()).IsErrorOfType<Exception>();
        await Assert.That(ErrorState.Error(new ArgumentException() as Exception)).IsErrorOfType<ArgumentException>();

        await Assert.That(ErrorState.Success<int>()).IsSuccess();
        await Assert.That(ErrorState.Error(1)).IsError(1);
    }
}
