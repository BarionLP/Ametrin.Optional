using System;
using System.Threading.Tasks;
using TUnit.Assertions.AssertConditions;

namespace Ametrin.Optional.Testing.TUnit;

internal sealed class ResultAssertCondition<TValue>(bool hasValue) : BaseAssertCondition<Result<TValue>>
{
    private readonly bool expectValue = hasValue;

    protected override string GetExpectation() => expectValue ? "to be Success" : "to be Error";

    protected override Task<AssertionResult> GetResult(Result<TValue> actualValue, Exception? exception)
    {
        var hasValue = OptionsMarshall.IsSuccess(actualValue);

        return hasValue == expectValue ? AssertionResult.Passed : AssertionResult.Fail(() => hasValue ? "found Success" : "found Error");
    }
}


internal sealed class ResultAssertCondition<TValue, TError>(bool hasValue) : BaseAssertCondition<Result<TValue, TError>>
{
    private readonly bool expectValue = hasValue;

    protected override string GetExpectation() => expectValue ? "to be Success" : "to be Error";

    protected override Task<AssertionResult> GetResult(Result<TValue, TError> actualValue, Exception? exception)
    {
        var hasValue = OptionsMarshall.IsSuccess(actualValue); ;

        return hasValue == expectValue ? AssertionResult.Passed : AssertionResult.Fail(() => hasValue ? "found Success" : "found Error");
    }
}