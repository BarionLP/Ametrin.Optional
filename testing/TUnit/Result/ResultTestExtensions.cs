using System;
using System.Runtime.CompilerServices;
using TUnit.Assertions.AssertConditions;
using TUnit.Assertions.AssertConditions.Interfaces;
using TUnit.Assertions.AssertionBuilders;

namespace Ametrin.Optional.Testing.TUnit;

public static class ResultTestExtensions
{
    /// <summary>
    /// Asserts the Result is Success with a specific value
    /// </summary>
    /// <typeparam name="TValue">type parameter of <see cref="Result{TValue}"/></typeparam>
    /// <param name="expected">the expected value</param>
    /// <returns></returns>
    public static InvokableValueAssertionBuilder<Result<TValue>> IsSuccess<TValue>(this IValueSource<Result<TValue>> valueSource, TValue expected, [CallerArgumentExpression(nameof(expected))] string doNotPopulateThisValue1 = "")
        => Assert(valueSource, new ResultAssertSuccessCondition<TValue>(expected), [doNotPopulateThisValue1]);

    public static InvokableValueAssertionBuilder<Result<TValue>> IsSuccess<TValue>(this IValueSource<Result<TValue>> valueSource)
        => Assert(valueSource, new ResultAssertCondition<TValue>(true), []);
    public static InvokableValueAssertionBuilder<Result<TValue>> IsError<TValue>(this IValueSource<Result<TValue>> valueSource)
        => Assert(valueSource, new ResultAssertCondition<TValue>(false), []);

    public static InvokableValueAssertionBuilder<Result<TValue>> IsErrorOfType<TValue, TError>(this IValueSource<Result<TValue>> valueSource) where TError : Exception
        => Assert(valueSource, new ResultAssertErrorTypeCondition<TValue, TError>(), []);
    public static InvokableValueAssertionBuilder<Result<TValue>> IsErrorNotOfType<TValue, TError>(this IValueSource<Result<TValue>> valueSource) where TError : Exception
        => Assert(valueSource, new ResultAssertErrorTypeNotCondition<TValue, TError>(), []);

    public static InvokableValueAssertionBuilder<Result<TValue, TError>> IsSuccess<TValue, TError>(this IValueSource<Result<TValue, TError>> valueSource, TValue expected, [CallerArgumentExpression(nameof(expected))] string doNotPopulateThisValue1 = "")
        => Assert(valueSource, new ResultAssertSuccessCondition<TValue, TError>(expected), [doNotPopulateThisValue1]);
    public static InvokableValueAssertionBuilder<Result<TValue, TError>> IsError<TValue, TError>(this IValueSource<Result<TValue, TError>> valueSource, TError expected, [CallerArgumentExpression(nameof(expected))] string doNotPopulateThisValue1 = "")
        => Assert(valueSource, new ResultAssertErrorCondition<TValue, TError>(expected), [doNotPopulateThisValue1]);

    public static InvokableValueAssertionBuilder<Result<TValue, TError>> IsSuccess<TValue, TError>(this IValueSource<Result<TValue, TError>> valueSource)
        => Assert(valueSource, new ResultAssertCondition<TValue, TError>(true), []);
    public static InvokableValueAssertionBuilder<Result<TValue, TError>> IsError<TValue, TError>(this IValueSource<Result<TValue, TError>> valueSource)
        => Assert(valueSource, new ResultAssertCondition<TValue, TError>(false), []);


    internal static InvokableValueAssertionBuilder<Result<TValue>> Assert<TValue>(IValueSource<Result<TValue>> valueSource, BaseAssertCondition<Result<TValue>> condition, string[] argumentExpressions)
    {
        var assertionBuilder = valueSource.RegisterAssertion(condition, argumentExpressions);
        return new InvokableValueAssertionBuilder<Result<TValue>>(assertionBuilder);
    }
    internal static InvokableValueAssertionBuilder<Result<TValue, TError>> Assert<TValue, TError>(IValueSource<Result<TValue, TError>> valueSource, BaseAssertCondition<Result<TValue, TError>> condition, string[] argumentExpressions)
    {
        var assertionBuilder = valueSource.RegisterAssertion(condition, argumentExpressions);
        return new InvokableValueAssertionBuilder<Result<TValue, TError>>(assertionBuilder);
    }
}
