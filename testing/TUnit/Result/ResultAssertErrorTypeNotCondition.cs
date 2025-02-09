using System;
using System.Threading.Tasks;
using TUnit.Assertions.AssertConditions;

namespace Ametrin.Optional.Testing.TUnit;

internal sealed class ResultAssertErrorTypeNotCondition<TValue, TError>() : BaseAssertCondition<Result<TValue>> where TError : Exception
{
    protected override string GetExpectation() => $"not to be {typeof(TError).Name}";

    protected override Task<AssertionResult> GetResult(Result<TValue> actualValue, Exception? exception, AssertionMetadata assertionMetadata)
    {
        var error = OptionsMarshall.GetErrorOrNull(actualValue);
        return error switch
        {
            null => AssertionResult.Fail("found Success"),
            not TError => AssertionResult.Passed,
            _ => AssertionResult.Fail($"found {error}")
        };
    }
}
