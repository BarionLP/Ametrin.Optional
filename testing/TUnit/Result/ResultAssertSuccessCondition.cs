using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TUnit.Assertions.AssertConditions;

namespace Ametrin.Optional.Testing.TUnit;

internal sealed class ResultAssertSuccessCondition<TValue>(TValue expectValue) : BaseAssertCondition<Result<TValue>>
{
    private readonly TValue expectValue = expectValue;

    protected override string GetExpectation() => $"to be {expectValue}";

    protected override ValueTask<AssertionResult> GetResult(Result<TValue> actualValue, Exception? exception, AssertionMetadata assertionMetadata)
    {
        var hasValue = actualValue.Branch(out var actual, out _);

        return hasValue && EqualityComparer<TValue>.Default.Equals(expectValue, actual) ? AssertionResult.Passed : AssertionResult.Fail(hasValue ? $"found {actual}" : "found Error");
    }
}

internal sealed class ResultAssertSuccessCondition<TValue, TError>(TValue expectValue) : BaseAssertCondition<Result<TValue, TError>>
{
    private readonly TValue expectValue = expectValue;

    protected override string GetExpectation() => $"to be {expectValue}";

    protected override ValueTask<AssertionResult> GetResult(Result<TValue, TError> actualValue, Exception? exception, AssertionMetadata assertionMetadata)
    {
        var hasValue = actualValue.Branch(out var actual, out _);

        return hasValue && EqualityComparer<TValue>.Default.Equals(expectValue, actual) ? AssertionResult.Passed : AssertionResult.Fail(hasValue ? $"found {actual}" : "found Error");
    }
}
