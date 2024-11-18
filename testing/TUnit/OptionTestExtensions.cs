using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using TUnit.Assertions.AssertConditions;
using TUnit.Assertions.AssertConditions.Interfaces;
using TUnit.Assertions.AssertionBuilders;
using TUnit.Assertions.AssertionBuilders.Wrappers;

namespace Ametrin.Optional.Testing.TUnit;

public static class OptionTestExtensions
{
    public static GenericEqualToAssertionBuilderWrapper<Option<TValue>> IsSuccess<TValue>(this IValueSource<Option<TValue>> valueSource, TValue expected, [CallerArgumentExpression(nameof(expected))] string doNotPopulateThisValue1 = "")
    {
        return valueSource.IsEqualTo(expected, doNotPopulateThisValue1);
    }

    public static InvokableValueAssertionBuilder<Option<TValue>> IsSuccess<TValue>(this IValueSource<Option<TValue>> valueSource)
    {
        var assertionBuilder = valueSource.RegisterAssertion(new OptionAssertCondition<TValue>(true), []);
        return new InvokableValueAssertionBuilder<Option<TValue>>(assertionBuilder);
    }

    public static InvokableValueAssertionBuilder<Option<TValue>> IsError<TValue>(this IValueSource<Option<TValue>> valueSource)
    {
        var assertionBuilder = valueSource.RegisterAssertion(new OptionAssertCondition<TValue>(false), []);
        return new InvokableValueAssertionBuilder<Option<TValue>>(assertionBuilder);
    }
}

public sealed class OptionAssertCondition<TValue>(bool expectValue) : BaseAssertCondition<Option<TValue>>
{
    private readonly bool expectValue = expectValue;

    protected override string GetExpectation() => expectValue
            ? "to be Success"
            : "to be Error";

    protected override Task<AssertionResult> GetResult(Option<TValue> actualValue, Exception? exception)
    {
        var hasValue = OptionsMarshall.IsSuccess(actualValue);

        return hasValue == expectValue ? AssertionResult.Passed : AssertionResult.Fail(hasValue ? "found Success" : "found Error");
    }
}
