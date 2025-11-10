namespace Ametrin.Optional.Test;

public sealed class CombineErrorsTests
{
    [Test]
    public async Task Tests_Exception()
    {
        var success = ErrorState.Success();
        var a = ErrorState.Error(new InvalidCastException());
        var b = ErrorState.Error(new InvalidOperationException());
        var c = ErrorState.Error(new ArgumentException());

        await Assert.That(ErrorState.CombineErrors(success, success)).IsSuccess();
        await Assert.That(ErrorState.CombineErrors(a, success)).IsErrorOfType<InvalidCastException>();
        await Assert.That(ErrorState.CombineErrors(success, b)).IsErrorOfType<InvalidOperationException>();
        await Assert.That(ErrorState.CombineErrors(a, b)).IsError(static e => e is AggregateException { InnerExceptions: [InvalidCastException, InvalidOperationException] });

        await Assert.That(ErrorState.CombineErrors(success, success, success)).IsSuccess();
        await Assert.That(ErrorState.CombineErrors(a, success, success)).IsErrorOfType<InvalidCastException>();
        await Assert.That(ErrorState.CombineErrors(success, b, success)).IsErrorOfType<InvalidOperationException>();
        await Assert.That(ErrorState.CombineErrors(success, success, c)).IsErrorOfType<ArgumentException>();
        await Assert.That(ErrorState.CombineErrors(a, b, success)).IsError(static e => e is AggregateException { InnerExceptions: [InvalidCastException, InvalidOperationException] });
        await Assert.That(ErrorState.CombineErrors(a, success, c)).IsError(static e => e is AggregateException { InnerExceptions: [InvalidCastException, ArgumentException] });
        await Assert.That(ErrorState.CombineErrors(success, b, c)).IsError(static e => e is AggregateException { InnerExceptions: [InvalidOperationException, ArgumentException] });
        await Assert.That(ErrorState.CombineErrors(a, b, c)).IsError(static e => e is AggregateException { InnerExceptions: [InvalidCastException, InvalidOperationException, ArgumentException] });
    }

    [Test]
    public async Task Tests_Generic()
    {
        var success = ErrorState.Success<string>();
        var a = ErrorState.Error("A");
        var b = ErrorState.Error("B");

        await Assert.That(ErrorState.CombineErrors(success, success, ErrorMerge)).IsSuccess();
        await Assert.That(ErrorState.CombineErrors(a, success, ErrorMerge)).IsError("A");
        await Assert.That(ErrorState.CombineErrors(success, b, ErrorMerge)).IsError("B");
        await Assert.That(ErrorState.CombineErrors(a, b, ErrorMerge)).IsError("AB");

        static string ErrorMerge(string a, string b) => a + b;
    }
}
