using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using TUnit.Assertions.AssertConditions;
using TUnit.Assertions.AssertConditions.Interfaces;
using TUnit.Assertions.AssertionBuilders;
using TUnit.Assertions.AssertionBuilders.Wrappers;

namespace Ametrin.Optional.Testing.TUnit;

public static class ResultTestExtensions
{
    /// <summary>
    /// Asserts the Result is Success
    /// </summary>
    /// <typeparam name="TValue">type parameter of <see cref="Result{TValue}"/></typeparam>
    /// <returns></returns>
    public static InvokableValueAssertionBuilder<Result<TValue>> IsSuccess<TValue>(this IValueSource<Result<TValue>> valueSource)
    {
        var assertionBuilder = valueSource.RegisterAssertion(new ResultAssertCondition<TValue>(true), []);
        return new InvokableValueAssertionBuilder<Result<TValue>>(assertionBuilder);
    }

    /// <summary>
    /// Asserts the Result is Success with a specific value
    /// </summary>
    /// <typeparam name="TValue">type parameter of <see cref="Result{TValue}"/></typeparam>
    /// <param name="expected">the expected value</param>
    /// <returns></returns>
    public static GenericEqualToAssertionBuilderWrapper<Result<TValue>> IsSuccess<TValue>(this IValueSource<Result<TValue>> valueSource, TValue expected, [CallerArgumentExpression(nameof(expected))] string doNotPopulateThisValue1 = "")
        => valueSource.IsEqualTo(expected, doNotPopulateThisValue1);

    public static InvokableValueAssertionBuilder<Result<TValue>> IsError<TValue>(this IValueSource<Result<TValue>> valueSource)
    {
        var assertionBuilder = valueSource.RegisterAssertion(new ResultAssertCondition<TValue>(false), []);
        return new InvokableValueAssertionBuilder<Result<TValue>>(assertionBuilder);
    }
    public static InvokableValueAssertionBuilder<Result<TValue>> IsErrorOfType<TValue, TError>(this IValueSource<Result<TValue>> valueSource) where TError : Exception
    {
        var assertionBuilder = valueSource.RegisterAssertion(new ResultAssertErrorTypeCondition<TValue, TError>(), []);
        return new InvokableValueAssertionBuilder<Result<TValue>>(assertionBuilder);
    }
    public static InvokableValueAssertionBuilder<Result<TValue>> IsErrorNotOfType<TValue, TError>(this IValueSource<Result<TValue>> valueSource) where TError : Exception
    {
        var assertionBuilder = valueSource.RegisterAssertion(new ResultAssertErrorTypeNotCondition<TValue, TError>(), []);
        return new InvokableValueAssertionBuilder<Result<TValue>>(assertionBuilder);
    }

    public static GenericEqualToAssertionBuilderWrapper<Result<TValue, TError>> IsSuccess<TValue, TError>(this ValueAssertionBuilder<Result<TValue, TError>> assertionBuilder, TValue expected, [CallerArgumentExpression(nameof(expected))] string doNotPopulateThisValue1 = "")
        => assertionBuilder.IsEqualTo(expected, doNotPopulateThisValue1);
    public static GenericEqualToAssertionBuilderWrapper<Result<TValue, TError>> IsError<TValue, TError>(this ValueAssertionBuilder<Result<TValue, TError>> assertionBuilder, TError expected, [CallerArgumentExpression(nameof(expected))] string doNotPopulateThisValue1 = "")
        => assertionBuilder.IsEqualTo(expected, doNotPopulateThisValue1);

    public static InvokableValueAssertionBuilder<Result<TValue, TError>> IsSuccess<TValue, TError>(this IValueSource<Result<TValue, TError>> valueSource)
    {
        var assertionBuilder = valueSource.RegisterAssertion(new Result2AssertCondition<TValue, TError>(true), []);
        return new InvokableValueAssertionBuilder<Result<TValue, TError>>(assertionBuilder);
    }
    public static InvokableValueAssertionBuilder<Result<TValue, TError>> IsError<TValue, TError>(this IValueSource<Result<TValue, TError>> valueSource)
    {
        var assertionBuilder = valueSource.RegisterAssertion(new Result2AssertCondition<TValue, TError>(false), []);
        return new InvokableValueAssertionBuilder<Result<TValue, TError>>(assertionBuilder);
    }
}


public sealed class ResultAssertCondition<TValue>(bool hasValue) : BaseAssertCondition<Result<TValue>>
{
    private readonly bool expectValue = hasValue;

    protected override string GetExpectation() => expectValue ? "to be Success" : "to be Error";

    protected override Task<AssertionResult> GetResult(Result<TValue> actualValue, Exception? exception)
    {
        var hasValue = OptionsMarshall.IsSuccess(actualValue);

        return hasValue == expectValue ? AssertionResult.Passed : AssertionResult.Fail(() => hasValue ? "found Success" : "found Error");
    }
}

public sealed class ResultAssertErrorTypeCondition<TValue, TError>() : BaseAssertCondition<Result<TValue>> where TError : Exception
{
    protected override string GetExpectation() => $"to be {typeof(TError).Name}";

    protected override Task<AssertionResult> GetResult(Result<TValue> actualValue, Exception? exception)
    {
        return OptionsMarshall.GetErrorOrNull(actualValue) is TError ? AssertionResult.Passed : AssertionResult.Fail(() => actualValue.Select(v => "found Success").Or(e => $"found {e}"));
    }
}
public sealed class ResultAssertErrorTypeNotCondition<TValue, TError>() : BaseAssertCondition<Result<TValue>> where TError : Exception
{
    protected override string GetExpectation() => $"not to be {typeof(TError).Name}";

    protected override Task<AssertionResult> GetResult(Result<TValue> actualValue, Exception? exception)
    {   
        var error = OptionsMarshall.GetErrorOrNull(actualValue);
        return error switch
        {
            null => AssertionResult.Fail(() => "found Success"),
            not TError => AssertionResult.Passed,
            _ => AssertionResult.Fail(() => $"found {error}")
        };
    }
}

public sealed class Result2AssertCondition<TValue, TError>(bool hasValue) : BaseAssertCondition<Result<TValue, TError>>
{
    private readonly bool expectValue = hasValue;

    protected override string GetExpectation() => expectValue ? "to be Success" : "to be Error";

    protected override Task<AssertionResult> GetResult(Result<TValue, TError> actualValue, Exception? exception)
    {
        var hasValue = OptionsMarshall.IsSuccess(actualValue); ;

        return hasValue == expectValue ? AssertionResult.Passed : AssertionResult.Fail(() => hasValue ? "found Success" : "found Error");
    }
}