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

    [Test]
    public async Task Option_Tuple_Match_Test()
    {
        var success_success = Option.Success("1").Join(Option.Success(2));
        var success_error = Option.Success("1").Join(Option.Error<int>());
        var error_success = Option.Error<string>().Join(Option.Success(2));
        var error_error = Option.Error<string>().Join(Option.Error<int>());

        await Assert.That(success_success.Match((s, i) => s, () => "nay")).IsEqualTo("1");
        await Assert.That(success_error.Match((s, i) => s, () => "nay")).IsEqualTo("nay");
        await Assert.That(error_success.Match((s, i) => s, () => "nay")).IsEqualTo("nay");
        await Assert.That(error_error.Match((s, i) => s, () => "nay")).IsEqualTo("nay");

        await Assert.That(success_success.Match(true, (s, i, arg) => arg ? s : "m", arg => arg ? "nay" : "no")).IsEqualTo("1");
        await Assert.That(success_error.Match(true, (s, i, arg) => arg ? s : "m", arg => arg ? "nay" : "no")).IsEqualTo("nay");
        await Assert.That(error_success.Match(true, (s, i, arg) => arg ? s : "m", arg => arg ? "nay" : "no")).IsEqualTo("nay");
        await Assert.That(error_error.Match(true, (s, i, arg) => arg ? s : "m", arg => arg ? "nay" : "no")).IsEqualTo("nay");
    }

    [Test]
    public async Task Result_Tuple_Match_Test()
    {
        var exception = new Exception("nay");
        var success_success = Result.Success("1").Join(Result.Success(2));
        var success_error = Result.Success("1").Join(Result.Error<int>(exception));
        var error_success = Result.Error<string>(exception).Join(Result.Success(2));
        var error_error = Result.Error<string>(exception).Join(Result.Error<int>(exception));

        await Assert.That(success_success.Match((s, i) => s, e => e.Message)).IsEqualTo("1");
        await Assert.That(success_error.Match((s, i) => s, e => e.Message)).IsEqualTo("nay");
        await Assert.That(error_success.Match((s, i) => s, e => e.Message)).IsEqualTo("nay");
        await Assert.That(error_error.Match((s, i) => s, e => e.Message)).IsEqualTo("One or more errors occurred. (nay) (nay)");

        await Assert.That(success_success.Match(true, (s, i, arg) => arg ? s : "m", (e, arg) => arg ? e.Message : "no")).IsEqualTo("1");
        await Assert.That(success_error.Match(true, (s, i, arg) => arg ? s : "m", (e, arg) => arg ? e.Message : "no")).IsEqualTo("nay");
        await Assert.That(error_success.Match(true, (s, i, arg) => arg ? s : "m", (e, arg) => arg ? e.Message : "no")).IsEqualTo("nay");
        await Assert.That(error_error.Match(true, (s, i, arg) => arg ? s : "m", (e, arg) => arg ? e.Message : "no")).IsEqualTo("One or more errors occurred. (nay) (nay)");
    }

    [Test]
    public async Task Result_Generic_Tuple_Match_Test()
    {
        var error = "nay";
        var success_success = Result.Success<int, string>(1).Join(Result.Success<int, string>(2), ErrorAggregate);
        var success_error = Result.Success<int, string>(1).Join(Result.Error<int, string>(error), ErrorAggregate);
        var error_success = Result.Error<int, string>(error).Join(Result.Success<int, string>(2), ErrorAggregate);
        var error_error = Result.Error<int, string>(error).Join(Result.Error<int, string>(error), ErrorAggregate);

        await Assert.That(success_success.Match((s, i) => (s + i).ToString(), e => e)).IsEqualTo("3");
        await Assert.That(success_error.Match((s, i) => (s + i).ToString(), e => e)).IsEqualTo("nay");
        await Assert.That(error_success.Match((s, i) => (s + i).ToString(), e => e)).IsEqualTo("nay");
        await Assert.That(error_error.Match((s, i) => (s + i).ToString(), e => e)).IsEqualTo("naynay");

        await Assert.That(success_success.Match(true, (s, i, arg) => arg ? (s + i).ToString() : "m", (e, arg) => arg ? e : "no")).IsEqualTo("3");
        await Assert.That(success_error.Match(true, (s, i, arg) => arg ? (s + i).ToString() : "m", (e, arg) => arg ? e : "no")).IsEqualTo("nay");
        await Assert.That(error_success.Match(true, (s, i, arg) => arg ? (s + i).ToString() : "m", (e, arg) => arg ? e : "no")).IsEqualTo("nay");
        await Assert.That(error_error.Match(true, (s, i, arg) => arg ? (s + i).ToString() : "m", (e, arg) => arg ? e : "no")).IsEqualTo("naynay");
        
        static string ErrorAggregate(string a, string b) => a + b;
    }
}
