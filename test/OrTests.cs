using TUnit.Assertions.AssertConditions.Throws;

namespace Ametrin.Optional.Test;

public sealed class OrTests
{
    [Test]
    public async Task Or_Success_Test()
    {
        await Assert.That(Option.Success(1).Or(0)).IsEqualTo(1);
        await Assert.That(Result.Success(1).Or(0)).IsEqualTo(1);
        await Assert.That(Result.Success<int, string>(1).Or(0)).IsEqualTo(1);
        await Assert.That(RefOption.Success(1).Or(0)).IsEqualTo(1);

        await Assert.That(Option.Success(1).Or(static () => 0)).IsEqualTo(1);
        await Assert.That(Result.Success(1).Or(static e => 0)).IsEqualTo(1);
        await Assert.That(Result.Success<int, string>(1).Or(static e => 0)).IsEqualTo(1);
        await Assert.That(RefOption.Success(1).Or(static () => 0)).IsEqualTo(1);


        await Assert.That(Option.Success(1).OrThrow()).IsEqualTo(1);
        await Assert.That(Result.Success(1).OrThrow()).IsEqualTo(1);
        await Assert.That(Result.Success<int, string>(1).OrThrow()).IsEqualTo(1);
        await Assert.That(RefOption.Success(1).OrThrow()).IsEqualTo(1);

        await Assert.That(Option.Success(1).OrNull()).IsEqualTo(1);
        await Assert.That(Result.Success(1).OrNull()).IsEqualTo(1);
        await Assert.That(Result.Success<int, string>(1).OrNull()).IsEqualTo(1);

        await Assert.That(Option.Success("").OrNull()).IsEqualTo("");
        await Assert.That(Result.Success("").OrNull()).IsEqualTo("");
        await Assert.That(Result.Success<string, int>("").OrNull()).IsEqualTo("");
    }

    [Test]
    public async Task Or_Error_Test()
    {
        await Assert.That(Option.Error<int>().Or(0)).IsEqualTo(0);
        await Assert.That(Result.Error<int>().Or(0)).IsEqualTo(0);
        await Assert.That(Result.Error<int, string>("").Or(0)).IsEqualTo(0);
        await Assert.That(RefOption.Error<int>().Or(0)).IsEqualTo(0);

        await Assert.That(Option.Error<int>().Or(static () => 0)).IsEqualTo(0);
        await Assert.That(Result.Error<int>().Or(static e => 0)).IsEqualTo(0);
        await Assert.That(Result.Error<int, string>("").Or(static e => 0)).IsEqualTo(0);
        await Assert.That(RefOption.Error<int>().Or(static () => 0)).IsEqualTo(0);

        await Assert.That(() => Option.Error<int>().OrThrow()).Throws<NullReferenceException>();
        await Assert.That(() => Result.Error<int>().OrThrow()).Throws<NullReferenceException>();
        await Assert.That(() => Result.Error<int, string>("").OrThrow()).Throws<NullReferenceException>();
        await Assert.That(() => RefOption.Error<int>().OrThrow()).Throws<NullReferenceException>();


        await Assert.That(Option.Error<int>().OrNull()).IsEqualTo(null);
        await Assert.That(Result.Error<int>().OrNull()).IsEqualTo(null);
        await Assert.That(Result.Error<int, string>("").OrNull()).IsEqualTo(null);

        await Assert.That(Option.Error<string>().OrNull()).IsEqualTo(null);
        await Assert.That(Result.Error<string>().OrNull()).IsEqualTo(null);
        await Assert.That(Result.Error<string, int>(-2).OrNull()).IsEqualTo(null);
    }
}
