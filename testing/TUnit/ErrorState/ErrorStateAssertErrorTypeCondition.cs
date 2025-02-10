using System;
using System.Threading.Tasks;
using TUnit.Assertions.AssertConditions;

namespace Ametrin.Optional.Testing.TUnit;

internal sealed class ErrorStateAssertErrorTypeCondition<TError>() : BaseAssertCondition<ErrorState> where TError : Exception
{
    protected override string GetExpectation() => $"to be {typeof(TError).Name}";

    protected override Task<AssertionResult> GetResult(ErrorState actualValue, Exception? exception, AssertionMetadata assertionMetadata)
    {
        return OptionsMarshall.GetErrorOrNull(actualValue) is TError ? AssertionResult.Passed : AssertionResult.Fail(actualValue.Map(() => "found Success", e => $"found {e}"));
    }
}
