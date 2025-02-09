using System;
using System.Threading.Tasks;
using TUnit.Assertions.AssertConditions;

namespace Ametrin.Optional.Testing.TUnit;

internal sealed class ErrorStateAssertCondition(bool hasValue) : BaseAssertCondition<ErrorState>
{
    private readonly bool expectValue = hasValue;

    protected override string GetExpectation() => expectValue ? "to be Success" : "to be Error";

    protected override Task<AssertionResult> GetResult(ErrorState actualValue, Exception? exception, AssertionMetadata assertionMetadata)
    {
        var hasValue = OptionsMarshall.IsSuccess(actualValue);

        return hasValue == expectValue ? AssertionResult.Passed : AssertionResult.Fail(hasValue ? "found Success" : "found Error");
    }
}

internal sealed class ErrorStateAssertCondition<TError>(bool hasValue) : BaseAssertCondition<ErrorState<TError>>
{
    private readonly bool expectValue = hasValue;

    protected override string GetExpectation() => expectValue ? "to be Success" : "to be Error";

    protected override Task<AssertionResult> GetResult(ErrorState<TError> actualValue, Exception? exception, AssertionMetadata assertionMetadata)
    {
        var hasValue = OptionsMarshall.IsSuccess(actualValue);

        return hasValue == expectValue ? AssertionResult.Passed : AssertionResult.Fail(hasValue ? "found Success" : "found Error");
    }
}
