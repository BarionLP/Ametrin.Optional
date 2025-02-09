using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TUnit.Assertions.AssertConditions;

namespace Ametrin.Optional.Testing.TUnit;

internal sealed class ResultAssertErrorCondition<TValue, TError>(TError expectedError) : BaseAssertCondition<Result<TValue, TError>>
{
    private readonly TError expectedError = expectedError;

    protected override string GetExpectation() => $"to be {expectedError}";

    protected override Task<AssertionResult> GetResult(Result<TValue, TError> actualValue, Exception? exception, AssertionMetadata assertionMetadata)
    {
        var hasError = OptionsMarshall.TryGetError(actualValue, out var actual);

        return hasError && EqualityComparer<TError>.Default.Equals(expectedError, actual) ? AssertionResult.Passed : AssertionResult.Fail(hasError ? $"found {actual}" : "found Success");
    }
}
