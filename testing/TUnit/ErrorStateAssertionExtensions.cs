using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using TUnit.Assertions.Attributes;
using TUnit.Assertions.Conditions;
using TUnit.Assertions.Core;

namespace Ametrin.Optional.Testing.TUnit;

public static class ErrorStateAssertionExtensions
{
    public const string EXPECTED_SUCCESS_MESSAGE = "to be Success";
    public const string EXPECTED_ERROR_MESSAGE = "to be Error";
    /// <summary>
    /// Asserts the ErrorState is Success
    /// </summary>
    /// <typeparam name="TError">type parameter of <see cref="ErrorState{TError}"/></typeparam>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [GenerateAssertion(ExpectationMessage = EXPECTED_SUCCESS_MESSAGE)]
    public static bool IsSuccess<TError>(this ErrorState<TError> errorState)
    {
        return OptionsMarshall.IsSuccess(errorState);
    }

    /// <summary>
    /// Asserts the ErrorState is Success
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [GenerateAssertion(ExpectationMessage = EXPECTED_SUCCESS_MESSAGE)]
    public static bool IsSuccess(this ErrorState errorState)
    {
        return OptionsMarshall.IsSuccess(errorState);
    }
    
    /// <summary>
    /// Asserts the ErrorState is Error
    /// </summary>
    /// <typeparam name="TError">type parameter of <see cref="ErrorState{TError}"/></typeparam>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [GenerateAssertion(ExpectationMessage = EXPECTED_ERROR_MESSAGE)]
    public static bool IsError<TError>(this ErrorState<TError> errorState)
    {
        return !OptionsMarshall.IsSuccess(errorState);
    }

    /// <summary>
    /// Asserts the ErrorState is Error
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [GenerateAssertion(ExpectationMessage = EXPECTED_ERROR_MESSAGE)]
    public static bool IsError(this ErrorState errorState)
    {
        return !OptionsMarshall.IsSuccess(errorState);
    }

    /// <summary>
    /// Asserts the ErrorState is an Error state matching the <paramref name="condition"/>
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [GenerateAssertion(ExpectationMessage = EXPECTED_ERROR_MESSAGE)]
    public static bool IsError<TError>(this ErrorState<TError> result, Func<TError, bool> condition)
    {
        return !result.Branch(out var error) && condition(error);
    }

    /// <summary>
    /// Asserts the ErrorState is an Error state matching the <paramref name="condition"/>
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [GenerateAssertion(ExpectationMessage = EXPECTED_ERROR_MESSAGE)]
    public static bool IsError(this ErrorState result, Func<Exception, bool> condition)
    {
        return !result.Branch(out var error) && condition(error);
    }

    /// <summary>
    /// Asserts the ErrorState is Error with a specific error value
    /// </summary>
    /// <typeparam name="TError">type parameter of <see cref="ErrorState{TError}"/></typeparam>
    /// <param name="expected">the expected error value</param>
    public static EqualsAssertion<ErrorState<TError>> IsError<TError>(this IAssertionSource<ErrorState<TError>> valueSource, TError expected, [CallerArgumentExpression(nameof(expected))] string doNotPopulateThisValue1 = "")
        => valueSource.IsEqualTo(expected, doNotPopulateThisValue1);

    /// <summary>
    /// Asserts the ErrorState has an error value of type <typeparamref name="TError"/>
    /// </summary>
    /// <returns></returns>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [GenerateAssertion(ExpectationMessage = EXPECTED_ERROR_MESSAGE)]
    public static bool IsErrorOfType<TError>(this ErrorState errorState) where TError : Exception
    {
        return errorState.Branch(out var error) ? false : error is TError;
    }

    /// <summary>
    /// Asserts the ErrorState has an error value but not of type <typeparamref name="TError"/>
    /// </summary>
    /// <returns></returns>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [GenerateAssertion(ExpectationMessage = EXPECTED_ERROR_MESSAGE)]
    public static bool IsErrorNotOfType<TError>(this ErrorState errorState) where TError : Exception
    {
        return errorState.Branch(out var error) ? false : error is not null and not TError;
    }
}
