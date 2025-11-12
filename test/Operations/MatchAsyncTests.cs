namespace Ametrin.Optional.Test.Operations;

public sealed class MatchAsyncTests
{
    [Test]
    public async Task MatchAsync_Success_Test()
    {
        await Assert.That(Option.Success().MatchAsync(() => Task.FromResult("yay"), () => Task.FromResult("nay"))).IsEqualTo("yay");
        await Assert.That(Option.Success("yay").MatchAsync(v => Task.FromResult(v), () => Task.FromResult("nay"))).IsEqualTo("yay");
        await Assert.That(Result.Success("yay").MatchAsync(v => Task.FromResult(v), e => Task.FromResult("nay"))).IsEqualTo("yay");
        await Assert.That(Result.Success<string, int>("yay").MatchAsync(v => Task.FromResult(v), e => Task.FromResult("nay"))).IsEqualTo("yay");
        await Assert.That(ErrorState.Success().MatchAsync(() => Task.FromResult("yay"), e => Task.FromResult("nay"))).IsEqualTo("yay");
        await Assert.That(ErrorState.Success<int>().MatchAsync(() => Task.FromResult("yay"), e => Task.FromResult("nay"))).IsEqualTo("yay");

        await Assert.That(Option.Success().MatchAsync(() => Task.FromResult("yay"), () => "nay")).IsEqualTo("yay");
        await Assert.That(Option.Success("yay").MatchAsync(v => Task.FromResult(v), () => "nay")).IsEqualTo("yay");
        await Assert.That(Result.Success("yay").MatchAsync(v => Task.FromResult(v), e => "nay")).IsEqualTo("yay");
        await Assert.That(Result.Success<string, int>("yay").MatchAsync(v => Task.FromResult(v), e => "nay")).IsEqualTo("yay");
        await Assert.That(ErrorState.Success().MatchAsync(() => Task.FromResult("yay"), e => "nay")).IsEqualTo("yay");
        await Assert.That(ErrorState.Success<int>().MatchAsync(() => Task.FromResult("yay"), e => "nay")).IsEqualTo("yay");
    }

    [Test]
    public async Task MatchAsync_Error_Test()
    {
        await Assert.That(Option.Error().MatchAsync(() => Task.FromResult("yay"), () => Task.FromResult("nay"))).IsEqualTo("nay");
        await Assert.That(Option.Error<string>().MatchAsync(v => Task.FromResult(v), () => Task.FromResult("nay"))).IsEqualTo("nay");
        await Assert.That(Result.Error<string>().MatchAsync(v => Task.FromResult(v), e => Task.FromResult("nay"))).IsEqualTo("nay");
        await Assert.That(Result.Error<string, int>(0).MatchAsync(v => Task.FromResult(v), e => Task.FromResult("nay"))).IsEqualTo("nay");
        await Assert.That(ErrorState.Error().MatchAsync(() => Task.FromResult("yay"), e => Task.FromResult("nay"))).IsEqualTo("nay");
        await Assert.That(ErrorState.Error(0).MatchAsync(() => Task.FromResult("yay"), e => Task.FromResult("nay"))).IsEqualTo("nay");

        await Assert.That(Option.Error().MatchAsync(() => Task.FromResult("yay"), () => "nay")).IsEqualTo("nay");
        await Assert.That(Option.Error<string>().MatchAsync(v => Task.FromResult(v), () => "nay")).IsEqualTo("nay");
        await Assert.That(Result.Error<string>().MatchAsync(v => Task.FromResult(v), e => "nay")).IsEqualTo("nay");
        await Assert.That(Result.Error<string, int>(0).MatchAsync(v => Task.FromResult(v), e => "nay")).IsEqualTo("nay");
        await Assert.That(ErrorState.Error().MatchAsync(() => Task.FromResult("yay"), e => "nay")).IsEqualTo("nay");
        await Assert.That(ErrorState.Error(0).MatchAsync(() => Task.FromResult("yay"), e => "nay")).IsEqualTo("nay");
    }
}
