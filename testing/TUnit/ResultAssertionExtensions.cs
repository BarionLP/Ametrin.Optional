using System;
using System.Collections.Generic;
using System.ComponentModel;
using TUnit.Assertions.Attributes;

namespace Ametrin.Optional.Testing.TUnit;

public static class ResultAssertionExtensions
{
    /// <summary>
    /// Asserts the Result is Success with a specific value
    /// </summary>
    /// <typeparam name="TValue">type parameter of <see cref="Result{TValue}"/></typeparam>
    /// <param name="expected">the expected value</param>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [GenerateAssertion(ExpectationMessage = "to be {expected}")]
    public static bool IsSuccess<TValue>(this Result<TValue> result, TValue expected)
    {
        return result.Branch(out var value, out _) && EqualityComparer<TValue>.Default.Equals(value, expected);
    }

    [EditorBrowsable(EditorBrowsableState.Never)]
    [GenerateAssertion(ExpectationMessage = ErrorStateAssertionExtensions.EXPECTED_SUCCESS_MESSAGE)]
    public static bool IsSuccess<TValue>(this Result<TValue> result, Func<TValue, bool> condition)
    {
        return result.Branch(out var value, out _) && condition(value);
    }

    [EditorBrowsable(EditorBrowsableState.Never)]
    [GenerateAssertion(ExpectationMessage = ErrorStateAssertionExtensions.EXPECTED_SUCCESS_MESSAGE)]
    public static bool IsSuccess<TValue>(this Result<TValue> result)
    {
        return OptionsMarshall.IsSuccess(result);
    }

    [EditorBrowsable(EditorBrowsableState.Never)]
    [GenerateAssertion(ExpectationMessage = ErrorStateAssertionExtensions.EXPECTED_ERROR_MESSAGE)]
    public static bool IsError<TValue>(this Result<TValue> result)
    {
        return !OptionsMarshall.IsSuccess(result);
    }

    [EditorBrowsable(EditorBrowsableState.Never)]
    [GenerateAssertion(ExpectationMessage = ErrorStateAssertionExtensions.EXPECTED_ERROR_MESSAGE)]
    public static bool IsError<TValue>(this Result<TValue> result, Func<Exception, bool> condition)
    {
        return !result.Branch(out _, out var error) && condition(error);
    }

    /// <summary>
    /// Asserts the <see cref="Result{TValue}"/> is an error value of type <typeparamref name="TError"/>
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [GenerateAssertion(ExpectationMessage = ErrorStateAssertionExtensions.EXPECTED_ERROR_MESSAGE)]
    public static bool IsErrorOfType<TValue, TError>(this Result<TValue> result) where TError : Exception
    {
        return !result.Branch(out _, out var error) && error is TError;
    }

    /// <summary>
    /// Asserts the <see cref="Result{TValue}"/> has an error value but not of type <typeparamref name="TError"/>
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [GenerateAssertion(ExpectationMessage = ErrorStateAssertionExtensions.EXPECTED_ERROR_MESSAGE)]
    public static bool IsErrorNotOfType<TValue, TError>(this Result<TValue> result) where TError : Exception
    {
        return !result.Branch(out _, out var error) && error is not null and not TError;
    }

    /// <summary>
    /// Asserts the <see cref="Result{TValue, TError}"/> is Success with a specific value
    /// </summary>
    /// <typeparam name="TValue">type parameter of <see cref="Result{TValue}"/></typeparam>
    /// <param name="expected">the expected value</param>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [GenerateAssertion(ExpectationMessage = "to be {expected}")]
    public static bool IsSuccess<TValue, TError>(this Result<TValue, TError> result, TValue expected)
    {
        return result.Branch(out var value, out _) && EqualityComparer<TValue>.Default.Equals(value, expected);
    }

    [EditorBrowsable(EditorBrowsableState.Never)]
    [GenerateAssertion(ExpectationMessage = ErrorStateAssertionExtensions.EXPECTED_SUCCESS_MESSAGE)]
    public static bool IsSuccess<TValue, TError>(this Result<TValue, TError> result, Func<TValue, bool> condition)
    {
        return result.Map(condition).Or(false);
    }


    /// <summary>
    /// Asserts the <see cref="Result{TValue, TError}"/> is Error with a specific value
    /// </summary>
    /// <typeparam name="TValue">type parameter of <see cref="Result{TValue}"/></typeparam>
    /// <param name="expected">the expected value</param>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [GenerateAssertion(ExpectationMessage = "to be {expected}")]
    public static bool IsError<TValue, TError>(this Result<TValue, TError> result, TError expected)
    {
        return !result.Branch(out _, out var error) && EqualityComparer<TError>.Default.Equals(error, expected);
    }

    [EditorBrowsable(EditorBrowsableState.Never)]
    [GenerateAssertion(ExpectationMessage = ErrorStateAssertionExtensions.EXPECTED_ERROR_MESSAGE)]
    public static bool IsError<TValue, TError>(this Result<TValue, TError> result, Func<TError, bool> condition)
    {
        return !result.Branch(out _, out var error) && condition(error);
    }

    [EditorBrowsable(EditorBrowsableState.Never)]
    [GenerateAssertion(ExpectationMessage = ErrorStateAssertionExtensions.EXPECTED_SUCCESS_MESSAGE)]
    public static bool IsSuccess<TValue, TError>(this Result<TValue, TError> result)
    {
        return OptionsMarshall.IsSuccess(result);
    }

    [EditorBrowsable(EditorBrowsableState.Never)]
    [GenerateAssertion(ExpectationMessage = ErrorStateAssertionExtensions.EXPECTED_SUCCESS_MESSAGE)]
    public static bool IsError<TValue, TError>(this Result<TValue, TError> result)
    {
        return !OptionsMarshall.IsSuccess(result);
    }
}
