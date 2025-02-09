using System;
using System.Threading.Tasks;
using TUnit.Assertions.AssertConditions;

namespace Ametrin.Optional.Testing.TUnit;

internal sealed class ErrorStateAssertErrorTypeNotCondition<TError>() : BaseAssertCondition<ErrorState> where TError : Exception
{
    protected override string GetExpectation() => $"not to be {typeof(TError).Name}";

    protected override Task<AssertionResult> GetResult(ErrorState actualValue, Exception? exception, AssertionMetadata assertionMetadata)
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