using System.Diagnostics.CodeAnalysis;

namespace Ametrin.Optional;

/// <summary>
/// providing unrecommended and/or unsafe access to all option types for edge case scenarios
/// </summary>
public static partial class OptionsMarshall
{
    public static bool IsSuccess<TValue>(Option<TValue> option) => option._hasValue;
    public static bool IsSuccess<TValue>(Result<TValue> result) => result._hasValue;
    public static bool IsSuccess<TValue, TError>(Result<TValue, TError> result) => result._hasValue;
    public static bool IsSuccess(ErrorState errorState) => !errorState._isError;
    public static bool IsSuccess<TError>(ErrorState<TError> errorState) => !errorState._isError;
    public static bool IsSuccess<TValue>(RefOption<TValue> option) where TValue : struct, allows ref struct => option._hasValue;

    public static TValue? GetValueUnsafe<TValue>(Option<TValue> option) => option._value;
    public static TValue? GetValueUnsafe<TValue>(Result<TValue> result) => result._value;
    public static TValue? GetValueUnsafe<TValue, TError>(Result<TValue, TError> result) => result._value;
    public static TValue GetValueUnsafe<TValue>(RefOption<TValue> option)
        where TValue : struct, allows ref struct
        => option._value;

    [Obsolete("use Option.Branch(out value)")]
    public static bool TryGetValue<TValue>(Option<TValue> option, [MaybeNullWhen(false)] out TValue value)
    {
        return option.Branch(out value);
    }

    [Obsolete("use Result.Branch(out value, out _)")]
    public static bool TryGetValue<TValue>(Result<TValue> result, [MaybeNullWhen(false)] out TValue value)
    {
        return result.Branch(out value, out _);
    }
    
    [Obsolete("use Result.Branch(out value, out _)")]
    public static bool TryGetValue<TValue, TError>(Result<TValue, TError> result, [MaybeNullWhen(false)] out TValue value)
    {
        return result.Branch(out value, out _);
    }
    
    [Obsolete("use RefOption.Branch(out value)")]
    public static bool TryGetValue<TValue>(RefOption<TValue> option, [MaybeNullWhen(false)] out TValue value)
        where TValue : struct, allows ref struct
    {
        return option.Branch(out value);
    }

    public static Exception? GetErrorOrNull<TValue>(Result<TValue> result)
        => result.Branch(out _, out var error) ? null : error;
    public static Exception GetError<TValue>(Result<TValue> result)
        => result.Branch(out _, out var error) ? throw new InvalidOperationException() : error;
    public static Exception? GetErrorUnsafe<TValue>(Result<TValue> result)
        => result._error;
    [Obsolete("use !Result.Branch(out _, out error)")]
    public static bool TryGetError<TValue>(Result<TValue> result, [NotNullWhen(true)] out Exception? error)
    {
        return !result.Branch(out _, out error);
    }

    public static TError GetError<TValue, TError>(Result<TValue, TError> result)
        => result.Branch(out _, out var error) ? throw new InvalidOperationException() : error;
    public static TError? GetErrorUnsafe<TValue, TError>(Result<TValue, TError> result)
        => result._error;
    [Obsolete("use !Result.Branch(out _, out error)")]
    public static bool TryGetError<TValue, TError>(Result<TValue, TError> result, [MaybeNullWhen(false)] out TError error)
    {
        return !result.Branch(out _, out error);
    }

    public static Exception? GetErrorOrNull(ErrorState errorState)
        => errorState.Branch(out var error) ? null : error;
    public static Exception GetError(ErrorState errorState)
        => errorState.Branch(out var error) ? throw new InvalidOperationException() : error;
    public static Exception? GetErrorUnsafe(ErrorState result)
        => result._error;
    [Obsolete("use !ErrorState.Branch(out error)")]
    public static bool TryGetError(ErrorState errorState, [NotNullWhen(true)] out Exception? error)
    {
        return !errorState.Branch(out error);
    }

    public static TError GetError<TError>(ErrorState<TError> errorState)
        => errorState.Branch(out var error) ? throw new InvalidOperationException() : error;
    public static TError? GetErrorUnsafe<TError>(ErrorState<TError> result)
        => result._error;
    [Obsolete("use !ErrorState.Branch(out error)")]
    public static bool TryGetError<TError>(ErrorState<TError> errorState, [MaybeNullWhen(false)] out TError error)
    {
        return !errorState.Branch(out error);
    }
}