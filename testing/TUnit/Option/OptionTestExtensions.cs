using System.Runtime.CompilerServices;
using TUnit.Assertions.AssertConditions;
using TUnit.Assertions.AssertConditions.Interfaces;
using TUnit.Assertions.AssertionBuilders;
using TUnit.Assertions.AssertionBuilders.Wrappers;

namespace Ametrin.Optional.Testing.TUnit;

public static class OptionTestExtensions
{
    public static InvokableValueAssertionBuilder<Option<TValue>> IsSuccess<TValue>(this IValueSource<Option<TValue>> valueSource, TValue expected, [CallerArgumentExpression(nameof(expected))] string doNotPopulateThisValue1 = "")
        => Assert(valueSource, new OptionAssertSuccessCondition<TValue>(expected), [doNotPopulateThisValue1]);
    public static InvokableValueAssertionBuilder<Option<TValue>> IsSuccess<TValue>(this IValueSource<Option<TValue>> valueSource)
        => Assert(valueSource, new OptionAssertCondition<TValue>(true), []);
    public static InvokableValueAssertionBuilder<Option<TValue>> IsError<TValue>(this IValueSource<Option<TValue>> valueSource)
        => Assert(valueSource, new OptionAssertCondition<TValue>(false), []);
    
    public static GenericEqualToAssertionBuilderWrapper<Option> IsSuccess(this IValueSource<Option> valueSource)
        => valueSource.IsEqualTo(true);
    public static GenericEqualToAssertionBuilderWrapper<Option> IsError(this IValueSource<Option> valueSource)
        => valueSource.IsEqualTo(false);

    internal static InvokableValueAssertionBuilder<Option<TValue>> Assert<TValue>(IValueSource<Option<TValue>> valueSource, BaseAssertCondition<Option<TValue>> condition, string[] argumentExpressions)
    {
        var assertionBuilder = valueSource.RegisterAssertion(condition, argumentExpressions);
        return new InvokableValueAssertionBuilder<Option<TValue>>(assertionBuilder);
    }
}
