using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using TUnit.Assertions.AssertConditions;
using TUnit.Assertions.AssertConditions.Interfaces;
using TUnit.Assertions.AssertionBuilders;
using TUnit.Assertions.AssertionBuilders.Wrappers;

namespace Ametrin.Optional.Testing.TUnit;

public static class ErrorStateTestExtensions
{
    /// <summary>
    /// Asserts the ErrorState is Success
    /// </summary>
    /// <typeparam name="TError">type parameter of <see cref="ErrorState{TError}"/></typeparam>
    /// <returns></returns>
    public static InvokableValueAssertionBuilder<ErrorState<TError>> IsSuccess<TError>(this IValueSource<ErrorState<TError>> valueSource)
    {
        var assertionBuilder = valueSource.RegisterAssertion(new ErrorStateAssertCondition<TError>(true), []);
        return new InvokableValueAssertionBuilder<ErrorState<TError>>(assertionBuilder);
    }

    /// <summary>
    /// Asserts the ErrorState is Success
    /// </summary>
    /// <returns></returns>
    public static InvokableValueAssertionBuilder<ErrorState> IsSuccess(this IValueSource<ErrorState> valueSource)
    {
        var assertionBuilder = valueSource.RegisterAssertion(new ErrorStateAssertCondition(true), []);
        return new InvokableValueAssertionBuilder<ErrorState>(assertionBuilder);
    }

    /// <summary>
    /// Asserts the ErrorState is Error with a specific error value
    /// </summary>
    /// <typeparam name="TError">type parameter of <see cref="ErrorState{TError}"/></typeparam>
    /// <param name="expected">the expected error value</param>
    /// <returns></returns>
    public static GenericEqualToAssertionBuilderWrapper<ErrorState<TError>> IsError<TError>(this IValueSource<ErrorState<TError>> valueSource, TError expected, [CallerArgumentExpression(nameof(expected))] string doNotPopulateThisValue1 = "")
        => valueSource.IsEqualTo(expected, doNotPopulateThisValue1);

    public static InvokableValueAssertionBuilder<ErrorState<TError>> IsError<TError>(this IValueSource<ErrorState<TError>> valueSource)
        => Assert(valueSource, new ErrorStateAssertCondition<TError>(false), []);

    public static InvokableValueAssertionBuilder<ErrorState> IsError(this IValueSource<ErrorState> valueSource)
        => Assert(valueSource, new ErrorStateAssertCondition(false), []);

    public static InvokableValueAssertionBuilder<ErrorState> IsErrorOfType<TError>(this IValueSource<ErrorState> valueSource)
        where TError : Exception
        => Assert(valueSource, new ErrorStateAssertErrorTypeCondition<TError>(), []);
    public static InvokableValueAssertionBuilder<ErrorState> IsErrorNotOfType<TError>(this IValueSource<ErrorState> valueSource)
        where TError : Exception
        => Assert(valueSource, new ErrorStateAssertErrorTypeNotCondition<TError>(), []);

    internal static InvokableValueAssertionBuilder<ErrorState> Assert(IValueSource<ErrorState> valueSource, BaseAssertCondition<ErrorState> condition, string[] argumentExpressions)
    {
        var assertionBuilder = valueSource.RegisterAssertion(condition, argumentExpressions);
        return new InvokableValueAssertionBuilder<ErrorState>(assertionBuilder);
    }
    internal static InvokableValueAssertionBuilder<ErrorState<TError>> Assert<TError>(IValueSource<ErrorState<TError>> valueSource, BaseAssertCondition<ErrorState<TError>> condition, string[] argumentExpressions)
    {
        var assertionBuilder = valueSource.RegisterAssertion(condition, argumentExpressions);
        return new InvokableValueAssertionBuilder<ErrorState<TError>>(assertionBuilder);
    }
}

public sealed class ErrorStateAssertErrorCondition<TError>(TError expectedError) : BaseAssertCondition<ErrorState<TError>>
{
    private readonly TError expectedError = expectedError;

    protected override string GetExpectation() => $"to be {expectedError}";

    protected override Task<AssertionResult> GetResult(ErrorState<TError> actualValue, Exception? exception)
    {
        var hasError = OptionsMarshall.TryGetError(actualValue, out var error);
        return hasError && EqualityComparer<TError>.Default.Equals(expectedError, error) ? AssertionResult.Passed : AssertionResult.Fail(hasError ? $"found {error}" : "found Success");
    }
}