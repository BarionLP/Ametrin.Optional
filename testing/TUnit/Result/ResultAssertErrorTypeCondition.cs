using System;
using System.Threading.Tasks;
using TUnit.Assertions.AssertConditions;

namespace Ametrin.Optional.Testing.TUnit;

internal sealed class ResultAssertErrorTypeCondition<TValue, TError>() : BaseAssertCondition<Result<TValue>> where TError : Exception
{
    protected override string GetExpectation() => $"to be {typeof(TError).Name}";

    protected override ValueTask<AssertionResult> GetResult(Result<TValue> actualValue, Exception? exception, AssertionMetadata assertionMetadata)
    {
        return OptionsMarshall.GetErrorOrNull(actualValue) is TError ? AssertionResult.Passed : AssertionResult.Fail(actualValue.Map(v => "found Success").Or(e => $"found {e}"));
    }
}
