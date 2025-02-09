namespace Ametrin.Optional.Test;

public sealed class WhereNotTests
{
    [Test]
    public async Task Success_WhereNot_True_Test()
    {
        await Assert.That(Option.Success(1).WhereNot(i => i == 2)).IsSuccess(1);
        await Assert.That(Result.Success(1).WhereNot(i => i == 2)).IsSuccess(1);
        await Assert.That(Result.Success(1).WhereNot(i => i == 2, static i => new ArgumentException($"{i} was 2"))).IsSuccess(1);
        await Assert.That(Result.Success<int, string>(1).WhereNot(i => i == 2, "was 2")).IsSuccess(1);
        await Assert.That(Result.Success<int, string>(1).WhereNot(i => i == 2, static i => $"{i} was 2")).IsSuccess(1);
        await Assert.That(OptionsMarshall.IsSuccess(RefOption.Success<Span<char>>(['a']).WhereNot(s => s.IsEmpty))).IsTrue();
    }

    [Test]
    public async Task Success_WhereNot_False_Test()
    {
        await Assert.That(Option.Success(1).WhereNot(i => i != 2)).IsError();
        await Assert.That(Result.Success(1).WhereNot(i => i != 2).Map<Exception>(i => null!).Or(e => e)).IsNotNull();
        await Assert.That(Result.Success(1).WhereNot(i => i != 2, static i => new ArgumentException($"{i} was not 2"))).IsErrorOfType<int, ArgumentException>();
        await Assert.That(Result.Success<int, string>(1).WhereNot(i => i != 2, "was not 2")).IsEqualTo("was not 2");
        await Assert.That(Result.Success<int, string>(1).WhereNot(i => i != 2, static i => $"{i} was not 2")).IsEqualTo("1 was not 2");
        await Assert.That(OptionsMarshall.IsSuccess(RefOption.Success<Span<char>>([]).WhereNot(s => s.IsEmpty))).IsFalse();
    }

    [Test]
    public async Task Error_WhereNot_Test()
    {
        await Assert.That(Option.Error<int>().WhereNot(i => i == 2)).IsEqualTo(default);
        await Assert.That(Result.Error<int>(new InvalidOperationException()).WhereNot(i => i == 2)).IsErrorOfType<int, InvalidOperationException>();
        await Assert.That(Result.Error<int>(new InvalidOperationException()).WhereNot(i => i == 2, static i => new ArgumentException($"{i} was 2"))).IsErrorOfType<int, InvalidOperationException>();
        await Assert.That(Result.Error<int, string>("error").WhereNot(i => i == 2, "was 2")).IsEqualTo("error");
        await Assert.That(Result.Error<int, string>("error").WhereNot(i => i == 2, static i => $"{i} was 2")).IsEqualTo("error");
        await Assert.That(OptionsMarshall.IsSuccess(RefOption.Error<Span<char>>().WhereNot(s => s.IsEmpty))).IsFalse();
    }
}
