using System;
using System.Threading.Tasks;
using TUnit.Assertions.AssertConditions;

namespace Ametrin.Optional.Testing.TUnit;

internal sealed class OptionAssertCondition<TValue>(bool expectValue) : BaseAssertCondition<Option<TValue>>
{
    private readonly bool expectValue = expectValue;

    protected override string GetExpectation() => expectValue
            ? "to be Success"
            : "to be Error";

    protected override ValueTask<AssertionResult> GetResult(Option<TValue> actualValue, Exception? exception, AssertionMetadata assertionMetadata)
    {
        var hasValue = OptionsMarshall.IsSuccess(actualValue);
        return hasValue == expectValue ? AssertionResult.Passed : AssertionResult.Fail(hasValue ? "found Success" : "found Error");
    }
}
