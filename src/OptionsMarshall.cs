using System.Diagnostics.CodeAnalysis;

namespace Ametrin.Optional;

/// <summary>
/// providing unrecommended and unsafe access to all option types for edge case scenarios
/// </summary>
public static partial class OptionsMarshall
{
    public static bool IsSuccess<TValue>(Option<TValue> option) => option._hasValue;
    public static bool IsSuccess<TValue>(Result<TValue> result) => result._hasValue;
    public static bool IsSuccess<TValue, TError>(Result<TValue, TError> result) => result._hasValue;
    public static bool IsSuccess(ErrorState errorState) => !errorState._isError;
    public static bool IsSuccess<TError>(ErrorState<TError> errorState) => !errorState._isError;

    public static bool TryGetValue<TValue>(Option<TValue> option, out TValue value)
    {
        if (option._hasValue)
        {
            value = option._value;
            return true;
        }
        value = default!;
        return false;
    }

    public static bool TryGetValue<TValue>(Result<TValue> result, out TValue value)
    {
        if (result._hasValue)
        {
            value = result._value;
            return true;
        }
        value = default!;
        return false;
    }
    
    public static bool TryGetValue<TValue, TError>(Result<TValue, TError> result, out TValue value)
    {
        if (result._hasValue)
        {
            value = result._value;
            return true;
        }
        value = default!;
        return false;
    }

    public static Exception GetError<TValue>(Result<TValue> result)
        => TryGetError(result, out var error) ? error : throw new InvalidOperationException();
    public static bool TryGetError<TValue>(Result<TValue> result, [NotNullWhen(true)] out Exception? error)
    {
        if (!result._hasValue)
        {
            error = result._error;
            return true;
        }
        error = default;
        return false;
    }

    public static TError GetError<TValue, TError>(Result<TValue, TError> result)
        => TryGetError(result, out var error) ? error : throw new InvalidOperationException();
    public static bool TryGetError<TValue, TError>(Result<TValue, TError> result, out TError error)
    {
        if (!result._hasValue)
        {
            error = result._error;
            return true;
        }
        error = default!;
        return false;
    }

    public static Exception GetError(ErrorState result)
    => TryGetError(result, out var error) ? error : throw new InvalidOperationException();
    public static bool TryGetError(ErrorState result, [NotNullWhen(true)] out Exception? error)
    {
        if (result._isError)
        {
            error = result._error;
            return true;
        }
        error = default;
        return false;
    }

    public static TError GetError<TError>(ErrorState<TError> result)
    => TryGetError(result, out var error) ? error : throw new InvalidOperationException();
    public static bool TryGetError<TError>(ErrorState<TError> result, out TError error)
    {
        if (result._isError)
        {
            error = result._error;
            return true;
        }
        error = default!;
        return false;
    }
}