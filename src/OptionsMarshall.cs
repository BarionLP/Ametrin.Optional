namespace Ametrin.Optional;

/// <summary>
/// providing unsafe access to all option types
/// </summary>
public static partial class OptionsMarshall
{
    public static TValue GetValue<TValue, TError>(Result<TValue, TError> result)
        => TryGetValue(result, out var value) ? value : throw new InvalidOperationException();
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
        => TryGetError(result, out var value) ? value : throw new InvalidOperationException();
    public static bool TryGetError<TResult>(Result<TResult> result, out Exception error)
    {
        if (!result._hasValue)
        {
            error = result._error;
            return true;
        }
        error = default!;
        return false;
    }
    
    public static TError GetError<TValue, TError>(Result<TValue, TError> result)
        => TryGetError(result, out var value) ? value : throw new InvalidOperationException();
    public static bool TryGetError<TResult, TError>(Result<TResult, TError> result, out TError error)
    {
        if (!result._hasValue)
        {
            error = result._error;
            return true;
        }
        error = default!;
        return false;
    }
}