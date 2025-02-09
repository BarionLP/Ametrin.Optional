namespace Ametrin.Optional.Test;

public sealed class EqualityTests
{
    [Test]
    public async Task Option_Test()
    {
        await Assert.That(Option.Success() == Option.Success()).IsTrue();
        await Assert.That(Option.Error() == Option.Error()).IsTrue();
        await Assert.That(Option.Error() == Option.Success()).IsFalse();
        await Assert.That(Option.Success() != Option.Error()).IsTrue();
        await Assert.That(Option.Error() != Option.Error()).IsFalse();

        await Assert.That(Option.Success().Equals(null)).IsFalse();
        await Assert.That(Option.Success().Equals((object)Option.Success())).IsTrue();
        await Assert.That(Option.Success().Equals((object)Option.Error())).IsFalse();

        await Assert.That(Option.Success().GetHashCode()).IsEqualTo(Option.Success().GetHashCode());
        await Assert.That(Option.Error().GetHashCode()).IsEqualTo(Option.Error().GetHashCode());
    }

    [Test]
    public async Task Option_Generic_Test()
    {
        await Assert.That(Option.Success(string.Empty) == Option.Success(string.Empty)).IsTrue();
        await Assert.That(Option.Success(string.Empty) == string.Empty).IsTrue();
        await Assert.That(Option.Error<string>() == null!).IsTrue();
        await Assert.That(Option.Error<string>() == Option.Success(string.Empty)).IsFalse();
        await Assert.That(Option.Error<string>() == string.Empty).IsFalse();
        await Assert.That(Option.Success(string.Empty) != Option.Error<string>()).IsTrue();
        await Assert.That(Option.Success(string.Empty) != null!).IsTrue();
        await Assert.That(Option.Error<string>() != Option.Error<string>()).IsFalse();

        await Assert.That(Option.Success(string.Empty).Equals(null)).IsFalse();
        await Assert.That(Option.Success(string.Empty).Equals((object)Option.Success(string.Empty))).IsTrue();
        await Assert.That(Option.Success(string.Empty).Equals((object)Option.Error<string>())).IsFalse();
        await Assert.That(Option.Success(string.Empty).Equals((object)string.Empty)).IsTrue();
        await Assert.That(Option.Success(string.Empty).Equals(7)).IsFalse();
        await Assert.That(Option.Success(string.Empty).Equals((object)"hello")).IsFalse();

        await Assert.That(Option.Success(string.Empty).GetHashCode()).IsEqualTo(Option.Success(string.Empty).GetHashCode());
        await Assert.That(Option.Error<string>().GetHashCode()).IsEqualTo(Option.Error<string>().GetHashCode());
    }

    [Test]
    public async Task Result_Test()
    {
        var sharedException = new Exception();

        await Assert.That(Result.Success(string.Empty) == Result.Success(string.Empty)).IsTrue();
        await Assert.That(Result.Error<string>() == Result.Success(string.Empty)).IsFalse();
        await Assert.That(Result.Error<string>() == string.Empty).IsFalse();
        await Assert.That(Result.Success(string.Empty) != Result.Error<string>()).IsTrue();
        await Assert.That(Result.Error<string>(sharedException) != Result.Error<string>(sharedException)).IsFalse();
        await Assert.That(Result.Error<string>(sharedException) != string.Empty).IsTrue();

        await Assert.That(Result.Success(string.Empty).Equals(null)).IsFalse();
        await Assert.That(Result.Error<string>().Equals(null)).IsFalse();
        await Assert.That(Result.Success(string.Empty).Equals((object)string.Empty)).IsTrue();
        await Assert.That(Result.Error<string>().Equals((object)string.Empty)).IsFalse();
        await Assert.That(Result.Success(string.Empty).Equals((object)Result.Success(string.Empty))).IsTrue();
        await Assert.That(Result.Success(string.Empty).Equals((object)Result.Error<string>())).IsFalse();
        await Assert.That(Result.Success(string.Empty).Equals(7)).IsFalse();

        await Assert.That(Result.Success(string.Empty).GetHashCode()).IsEqualTo(Result.Success(string.Empty).GetHashCode());
        await Assert.That(Result.Error<string>(sharedException).GetHashCode()).IsEqualTo(Result.Error<string>(sharedException).GetHashCode());
    }

    [Test]
    public async Task Result_Generic_Test()
    {
        await Assert.That(Result.Success<string, int>(string.Empty) == Result.Success<string, int>(string.Empty)).IsTrue();
        await Assert.That(Result.Error<string, int>(-1) == Result.Success<string, int>(string.Empty)).IsFalse();
        await Assert.That(Result.Error<string, int>(-1) == string.Empty).IsFalse();
        await Assert.That(Result.Success<string, int>(string.Empty) != Result.Error<string, int>(-1)).IsTrue();
        await Assert.That(Result.Error<string, int>(-1) != Result.Error<string, int>(-1)).IsFalse();
        await Assert.That(Result.Error<string, int>(-1) != string.Empty).IsTrue();

        await Assert.That(Result.Success<string, int>(string.Empty).Equals(null)).IsFalse();
        await Assert.That(Result.Error<string, int>(-1).Equals(null)).IsFalse();
        await Assert.That(Result.Success<string, int>(string.Empty).Equals((object)string.Empty)).IsTrue();
        await Assert.That(Result.Error<string, int>(-1).Equals((object)string.Empty)).IsFalse();
        await Assert.That(Result.Success<string, int>(string.Empty).Equals((object)Result.Success<string, int>(string.Empty))).IsTrue();
        await Assert.That(Result.Success<string, int>(string.Empty).Equals((object)Result.Error<string, int>(-1))).IsFalse();
        await Assert.That(Result.Success<string, int>(string.Empty).Equals(new Exception())).IsFalse();

        await Assert.That(Result.Success<string, int>(string.Empty).GetHashCode()).IsEqualTo(Result.Success<string, int>(string.Empty).GetHashCode());
        await Assert.That(Result.Error<string, int>(-1).GetHashCode()).IsEqualTo(Result.Error<string, int>(-1).GetHashCode());
    }
    
    [Test]
    public async Task ErrorState_Test()
    {
        var sharedException = new Exception();

        await Assert.That(ErrorState.Success() == ErrorState.Success()).IsTrue();
        await Assert.That(ErrorState.Error() == ErrorState.Success()).IsFalse();
        await Assert.That(ErrorState.Success() != ErrorState.Error()).IsTrue();
        await Assert.That(ErrorState.Error(sharedException) != ErrorState.Error(sharedException)).IsFalse();

        await Assert.That(ErrorState.Success().Equals(null)).IsTrue();
        await Assert.That(ErrorState.Error().Equals(null)).IsFalse();
        await Assert.That(ErrorState.Success().Equals((object)ErrorState.Success())).IsTrue();
        await Assert.That(ErrorState.Success().Equals((object)ErrorState.Error())).IsFalse();
        await Assert.That(ErrorState.Success().Equals(7)).IsFalse();

        await Assert.That(ErrorState.Success().GetHashCode()).IsEqualTo(ErrorState.Success().GetHashCode());
        await Assert.That(ErrorState.Error(sharedException).GetHashCode()).IsEqualTo(ErrorState.Error(sharedException).GetHashCode());
    }

    [Test]
    public async Task ErrorState_Generic_Test()
    {
        await Assert.That(ErrorState.Success<string>() == ErrorState.Success<string>()).IsTrue();
        await Assert.That(ErrorState.Error(string.Empty) == ErrorState.Success<string>()).IsFalse();
        await Assert.That(ErrorState.Success<string>() != ErrorState.Error(string.Empty)).IsTrue();
        await Assert.That(ErrorState.Error(string.Empty) != ErrorState.Error(string.Empty)).IsFalse();

        await Assert.That(ErrorState.Success<string>().Equals(null)).IsTrue();
        await Assert.That(ErrorState.Success<string>().Equals((object)ErrorState.Success<string>())).IsTrue();
        await Assert.That(ErrorState.Success<string>().Equals((object)ErrorState.Error(string.Empty))).IsFalse();
        await Assert.That(ErrorState.Success<string>().Equals((object)string.Empty)).IsFalse();
        await Assert.That(ErrorState.Success<string>().Equals(7)).IsFalse();
        await Assert.That(ErrorState.Success<string>().Equals((object)"hello")).IsFalse();

        await Assert.That(ErrorState.Success<string>().GetHashCode()).IsEqualTo(ErrorState.Success<string>().GetHashCode());
        await Assert.That(ErrorState.Error(string.Empty).GetHashCode()).IsEqualTo(ErrorState.Error(string.Empty).GetHashCode());
    }
}
