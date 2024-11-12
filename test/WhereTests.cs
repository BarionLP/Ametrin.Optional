namespace Ametrin.Optional.Test;

public sealed class WhereTests
{
    [Test]
    public async Task Success_Where_True_Test()
    {
        await Assert.That(Option.Success(1).Where(i => i != 2)).IsEqualTo(1);
        await Assert.That(Result.Success(1).Where(i => i != 2)).IsEqualTo(1);
        await Assert.That(Result.Success(1).Where(i => i != 2, static i => new ArgumentException($"{i} was 2"))).IsEqualTo(1);
        await Assert.That(Result.Success<int, string>(1).Where(i => i != 2, "was 2")).IsEqualTo(1);
        await Assert.That(Result.Success<int, string>(1).Where(i => i != 2, static i => $"{i} was 2")).IsEqualTo(1);
    }

    [Test]
    public async Task Success_Where_False_Test()
    {
        await Assert.That(Option.Success(1).Where(i => i == 2)).IsEqualTo(default);
        await Assert.That(Result.Success(1).Where(i => i == 2).Error is not null).IsTrue();
        await Assert.That(Result.Success(1).Where(i => i == 2, static i => new ArgumentException($"{i} was not 2")).Error is ArgumentException { Message: "1 was not 2" }).IsTrue();
        await Assert.That(Result.Success<int, string>(1).Where(i => i == 2, "was not 2")).IsEqualTo("was not 2");
        await Assert.That(Result.Success<int, string>(1).Where(i => i == 2, static i => $"{i} was not 2")).IsEqualTo("1 was not 2");
    }

    [Test]
    public async Task Error_Where_Test()
    {
        await Assert.That(Option.Error<int>().Where(i => i != 2)).IsEqualTo(default);
        await Assert.That(Result.Error<int>(new InvalidOperationException()).Where(i => i != 2).Error is InvalidOperationException).IsTrue();
        await Assert.That(Result.Error<int>(new InvalidOperationException()).Where(i => i != 2, static i => new ArgumentException($"{i} was 2")).Error is InvalidOperationException).IsTrue();
        await Assert.That(Result.Error<int, string>("error").Where(i => i != 2, "was 2")).IsEqualTo("error");
        await Assert.That(Result.Error<int, string>("error").Where(i => i != 2, static i => $"{i} was 2")).IsEqualTo("error");
    }

    [Test]
    public async Task Success_Where_Cast_True_Test()
    {
        await Assert.That(Option.Success<object>(string.Empty).Where<string>()).IsEqualTo("");
        await Assert.That(Result.Success<object>(string.Empty).Where<string>()).IsEqualTo("");
        await Assert.That(Result.Success<object>(string.Empty).Where<string>(static val => new InvalidOperationException())).IsEqualTo("");
        await Assert.That(Result.Success<object, int>(string.Empty).Where<string>(error: -1)).IsEqualTo("");
        await Assert.That(Result.Success<object, int>(string.Empty).Where<string>(error: static val => -2)).IsEqualTo("");
    }
    
    [Test]
    public async Task Success_Where_Cast_False_Test()
    {
        await Assert.That(Option.Success<object>(1).Where<string>()).IsEqualTo(default);
        await Assert.That(Result.Success<object>(1).Where<string>().Error is InvalidCastException).IsTrue();
        await Assert.That(Result.Success<object>(1).Where<string>(static val => new InvalidOperationException()).Error is InvalidOperationException).IsTrue();
        await Assert.That(Result.Success<object, int>(1).Where<string>(error: -1)).IsEqualTo(-1);
        await Assert.That(Result.Success<object, int>(1).Where<string>(error: static val => -2)).IsEqualTo(-2);
    }
    
    [Test]
    public async Task Error_Where_Cast_Test()
    {
        await Assert.That(Option.Error<object>().Where<string>()).IsEqualTo(default);
        await Assert.That(Result.Error<object>().Where<string>().Error is not InvalidCastException).IsTrue();
        await Assert.That(Result.Error<object>().Where<string>(static val => new InvalidOperationException()).Error is not InvalidOperationException).IsTrue();
        await Assert.That(Result.Error<object, int>(1).Where<string>(error: -1)).IsEqualTo(1);
        await Assert.That(Result.Error<object, int>(1).Where<string>(error: static val => -2)).IsEqualTo(1);
    }
}
