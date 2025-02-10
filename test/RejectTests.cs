namespace Ametrin.Optional.Test;

public sealed class RejectTests
{
    [Test]
    public async Task Success_Reject_True_Test()
    {
        await Assert.That(Option.Success(1).Reject(i => i == 2)).IsSuccess(1);
        await Assert.That(Result.Success(1).Reject(i => i == 2)).IsSuccess(1);
        await Assert.That(Result.Success(1).Reject(i => i == 2, static i => new ArgumentException($"{i} was 2"))).IsSuccess(1);
        await Assert.That(Result.Success<int, string>(1).Reject(i => i == 2, "was 2")).IsSuccess(1);
        await Assert.That(Result.Success<int, string>(1).Reject(i => i == 2, static i => $"{i} was 2")).IsSuccess(1);
        await Assert.That(OptionsMarshall.IsSuccess(RefOption.Success<Span<char>>(['a']).Reject(s => s.IsEmpty))).IsTrue();


        await Assert.That(new int?(1).Reject(v => v != 1)).IsEqualTo(1);
        await Assert.That("1".Reject(string.IsNullOrEmpty)).IsEqualTo("1");
    }

    [Test]
    public async Task Success_Reject_False_Test()
    {
        await Assert.That(Option.Success(1).Reject(i => i != 2)).IsError();
        await Assert.That(Result.Success(1).Reject(i => i != 2).Map<Exception>(i => null!).Or(e => e)).IsNotNull();
        await Assert.That(Result.Success(1).Reject(i => i != 2, static i => new ArgumentException($"{i} was not 2"))).IsErrorOfType<int, ArgumentException>();
        await Assert.That(Result.Success<int, string>(1).Reject(i => i != 2, "was not 2")).IsEqualTo("was not 2");
        await Assert.That(Result.Success<int, string>(1).Reject(i => i != 2, static i => $"{i} was not 2")).IsEqualTo("1 was not 2");
        await Assert.That(OptionsMarshall.IsSuccess(RefOption.Success<Span<char>>([]).Reject(s => s.IsEmpty))).IsFalse();


        await Assert.That(new int?(1).Reject(v => v == 1)).IsNull();
        await Assert.That("".Reject(string.IsNullOrEmpty)).IsNull();
    }

    [Test]
    public async Task Error_Reject_Test()
    {
        await Assert.That(Option.Error<int>().Reject(i => i == 2)).IsEqualTo(default);
        await Assert.That(Result.Error<int>(new InvalidOperationException()).Reject(i => i == 2)).IsErrorOfType<int, InvalidOperationException>();
        await Assert.That(Result.Error<int>(new InvalidOperationException()).Reject(i => i == 2, static i => new ArgumentException($"{i} was 2"))).IsErrorOfType<int, InvalidOperationException>();
        await Assert.That(Result.Error<int, string>("error").Reject(i => i == 2, "was 2")).IsEqualTo("error");
        await Assert.That(Result.Error<int, string>("error").Reject(i => i == 2, static i => $"{i} was 2")).IsEqualTo("error");
        await Assert.That(OptionsMarshall.IsSuccess(RefOption.Error<Span<char>>().Reject(s => s.IsEmpty))).IsFalse();


        await Assert.That(new int?().Reject(v => v != 1)).IsNull();
        await Assert.That(((string?)null).Reject(string.IsNullOrEmpty)).IsNull();
    }
}
