using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TUnit.Assertions.AssertConditions;

namespace Ametrin.Optional.Testing.TUnit;

internal sealed class ResultAssertSuccessCondition<TValue>(TValue expectValue) : BaseAssertCondition<Result<TValue>>
{
    private readonly TValue expectValue = expectValue;

    protected override string GetExpectation() => $"to be {expectValue}";

    protected override Task<AssertionResult> GetResult(Result<TValue> actualValue, Exception? exception, AssertionMetadata assertionMetadata)
    {
        var hasValue = OptionsMarshall.TryGetValue(actualValue, out var actual);

        return hasValue && EqualityComparer<TValue>.Default.Equals(expectValue, actual) ? AssertionResult.Passed : AssertionResult.Fail(hasValue ? $"found {actual}" : "found Error");
    }
}

internal sealed class ResultAssertSuccessCondition<TValue, TError>(TValue expectValue) : BaseAssertCondition<Result<TValue, TError>>
{
    private readonly TValue expectValue = expectValue;

    protected override string GetExpectation() => $"to be {expectValue}";

    protected override Task<AssertionResult> GetResult(Result<TValue, TError> actualValue, Exception? exception, AssertionMetadata assertionMetadata)
    {
        var hasValue = OptionsMarshall.TryGetValue(actualValue, out var actual);

        return hasValue && EqualityComparer<TValue>.Default.Equals(expectValue, actual) ? AssertionResult.Passed : AssertionResult.Fail(hasValue ? $"found {actual}" : "found Error");
    }
}
