namespace Ametrin.Optional;

public static class OptionsMarshall
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