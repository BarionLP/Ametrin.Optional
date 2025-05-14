using System;
using System.Threading.Tasks;
using TUnit.Assertions.AssertConditions;

namespace Ametrin.Optional.Testing.TUnit;

internal sealed class ErrorStateAssertErrorTypeCondition<TError>() : BaseAssertCondition<ErrorState> where TError : Exception
{
    protected override string GetExpectation() => $"to be {typeof(TError).Name}";

    protected override ValueTask<AssertionResult> GetResult(ErrorState actualValue, Exception? exception, AssertionMetadata assertionMetadata)
    {
        return OptionsMarshall.GetErrorOrNull(actualValue) is TError ? AssertionResult.Passed : AssertionResult.Fail(actualValue.Match(() => "found Success", e => $"found {e}"));
    }
}
