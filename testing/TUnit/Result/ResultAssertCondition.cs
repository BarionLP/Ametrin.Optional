using System;
using System.Threading.Tasks;
using TUnit.Assertions.AssertConditions;

namespace Ametrin.Optional.Testing.TUnit;

internal sealed class ResultAssertCondition<TValue>(bool hasValue) : BaseAssertCondition<Result<TValue>>
{
    private readonly bool expectValue = hasValue;

    protected override string GetExpectation() => expectValue ? "to be Success" : "to be Error";

    protected override ValueTask<AssertionResult> GetResult(Result<TValue> actualValue, Exception? exception, AssertionMetadata assertionMetadata)
    {
        var hasValue = OptionsMarshall.IsSuccess(actualValue);

        if (!hasValue && OptionsMarshall.GetError(actualValue) is null)
        {
            return AssertionResult.Fail("found Error with null error value");
        }

        return hasValue == expectValue ? AssertionResult.Passed : AssertionResult.Fail(hasValue ? "found Success" : "found Error");
    }
}


internal sealed class ResultAssertCondition<TValue, TError>(bool hasValue) : BaseAssertCondition<Result<TValue, TError>>
{
    private readonly bool expectValue = hasValue;

    protected override string GetExpectation() => expectValue ? "to be Success" : "to be Error";

    protected override ValueTask<AssertionResult> GetResult(Result<TValue, TError> actualValue, Exception? exception, AssertionMetadata assertionMetadata)
    {
        var hasValue = OptionsMarshall.IsSuccess(actualValue); ;

        return hasValue == expectValue ? AssertionResult.Passed : AssertionResult.Fail(hasValue ? "found Success" : "found Error");
    }
}