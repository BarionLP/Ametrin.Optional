namespace Ametrin.Optional.Test.Operations;

public sealed class MatchTests
{
    [Test]
    public async Task Match_Success_Test()
    {
        await Assert.That(Option.Success().Match(() => "yay", () => "nay")).IsEqualTo("yay");
        await Assert.That(Option.Success("yay").Match(v => v, () => "nay")).IsEqualTo("yay");
        await Assert.That(Result.Success("yay").Match(v => v, e => "nay")).IsEqualTo("yay");
        await Assert.That(Result.Success<string, int>("yay").Match(v => v, e => "nay")).IsEqualTo("yay");
        await Assert.That(ErrorState.Success().Match(() => "yay", e => "nay")).IsEqualTo("yay");
        await Assert.That(ErrorState.Success<int>().Match(() => "yay", e => "nay")).IsEqualTo("yay");
        await Assert.That(RefOption.Success("yay".AsSpan()).Match(v => v, () => "nay").ToString()).IsEqualTo("yay");

        await Assert.That("yay".Match(v => v, () => "nay")).IsEqualTo("yay");
        await Assert.That(new int?(1).Match(v => v, () => -1)).IsEqualTo(1);
    }

    [Test]
    public async Task Match_Error_Test()
    {
        await Assert.That(Option.Error().Match(() => "yay", () => "nay")).IsEqualTo("nay");
        await Assert.That(Option.Error<string>().Match(v => v, () => "nay")).IsEqualTo("nay");
        await Assert.That(Result.Error<string>().Match(v => v, e => "nay")).IsEqualTo("nay");
        await Assert.That(Result.Error<string, int>(0).Match(v => v, e => "nay")).IsEqualTo("nay");
        await Assert.That(ErrorState.Error().Match(() => "yay", e => "nay")).IsEqualTo("nay");
        await Assert.That(ErrorState.Error(0).Match(() => "yay", e => "nay")).IsEqualTo("nay");
        await Assert.That(RefOption.Error<ReadOnlySpan<char>>().Match(v => v.ToString(), () => "nay")).IsEqualTo("nay");

        await Assert.That(((string?)null).Match(v => v, () => "nay")).IsEqualTo("nay");
        await Assert.That(new int?().Match(v => v, () => -1)).IsEqualTo(-1);
    }
}
