namespace Ametrin.Optional.Test;

public sealed class WhereNotTests
{
    [Test]
    public async Task Success_WhereNot_True_Test()
    {
        await Assert.That(Option.Success(1).WhereNot(i => i == 2)).IsEqualTo(1);
        await Assert.That(Result.Success(1).WhereNot(i => i == 2)).IsEqualTo(1);
        await Assert.That(Result.Success(1).WhereNot(i => i == 2, static i => new ArgumentException($"{i} was 2"))).IsEqualTo(1);
        await Assert.That(Result.Success<int, string>(1).WhereNot(i => i == 2, "was 2")).IsEqualTo(1);
        await Assert.That(Result.Success<int, string>(1).WhereNot(i => i == 2, static i => $"{i} was 2")).IsEqualTo(1);
    }

    [Test]
    public async Task Success_WhereNot_False_Test()
    {
        await Assert.That(Option.Success(1).WhereNot(i => i != 2)).IsEqualTo(default);
        await Assert.That(Result.Success(1).WhereNot(i => i != 2).Error is not null).IsTrue();
        await Assert.That(Result.Success(1).WhereNot(i => i != 2, static i => new ArgumentException($"{i} was not 2")).Error is ArgumentException { Message: "1 was not 2" }).IsTrue();
        await Assert.That(Result.Success<int, string>(1).WhereNot(i => i != 2, "was not 2")).IsEqualTo("was not 2");
        await Assert.That(Result.Success<int, string>(1).WhereNot(i => i != 2, static i => $"{i} was not 2")).IsEqualTo("1 was not 2");
    }

    [Test]
    public async Task Error_WhereNot_Test()
    {
        await Assert.That(Option.Error<int>().WhereNot(i => i == 2)).IsEqualTo(default);
        await Assert.That(Result.Error<int>(new InvalidOperationException()).WhereNot(i => i == 2).Error is InvalidOperationException).IsTrue();
        await Assert.That(Result.Error<int>(new InvalidOperationException()).WhereNot(i => i == 2, static i => new ArgumentException($"{i} was 2")).Error is InvalidOperationException).IsTrue();
        await Assert.That(Result.Error<int, string>("error").WhereNot(i => i == 2, "was 2")).IsEqualTo("error");
        await Assert.That(Result.Error<int, string>("error").WhereNot(i => i == 2, static i => $"{i} was 2")).IsEqualTo("error");
    }
}
