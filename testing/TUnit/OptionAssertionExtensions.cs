using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using TUnit.Assertions.Attributes;
using TUnit.Assertions.Conditions;
using TUnit.Assertions.Core;

namespace Ametrin.Optional.Testing.TUnit;

public static class OptionAssertionExtensions
{
    [EditorBrowsable(EditorBrowsableState.Never)]
    [GenerateAssertion(ExpectationMessage = "to be {expected}")]
    public static bool IsSuccess<TValue>(this Option<TValue> option, TValue expected)
    {
        return option.Branch(out var value) ? EqualityComparer<TValue>.Default.Equals(value, expected) : false;
    }
    
    [EditorBrowsable(EditorBrowsableState.Never)]
    [GenerateAssertion(ExpectationMessage = ErrorStateAssertionExtensions.EXPECTED_SUCCESS_MESSAGE)]
    public static bool IsSuccess<TValue>(this Option<TValue> option)
    {
        return OptionsMarshall.IsSuccess(option);
    }
    
    [EditorBrowsable(EditorBrowsableState.Never)]
    [GenerateAssertion(ExpectationMessage = ErrorStateAssertionExtensions.EXPECTED_SUCCESS_MESSAGE)]
    public static bool IsError<TValue>(this Option<TValue> option)
    {
        return !OptionsMarshall.IsSuccess(option);
    }
    
    public static EqualsAssertion<Option> IsSuccess(this IAssertionSource<Option> valueSource)
        => valueSource.IsEqualTo(true);
    public static EqualsAssertion<Option> IsError(this IAssertionSource<Option> valueSource)
        => valueSource.IsEqualTo(false);
}
