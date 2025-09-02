using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TUnit.Assertions.AssertConditions;

namespace Ametrin.Optional.Testing.TUnit;

internal sealed class OptionAssertSuccessCondition<TValue>(TValue expectValue) : BaseAssertCondition<Option<TValue>>
{
    private readonly TValue expectValue = expectValue;

    protected override string GetExpectation() => $"to be {expectValue}";

    protected override ValueTask<AssertionResult> GetResult(Option<TValue> actualValue, Exception? exception, AssertionMetadata assertionMetadata)
    {
        var hasValue = actualValue.Branch(out var actual);
        return hasValue && EqualityComparer<TValue>.Default.Equals(expectValue, actual) ? AssertionResult.Passed : AssertionResult.Fail(hasValue ? $"found {actual}" : "found Error");
    }
}
