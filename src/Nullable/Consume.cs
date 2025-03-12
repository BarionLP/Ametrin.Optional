namespace Ametrin.Optional.Nullable;

public static class NullableConsumeExtensions
{
    public static bool Consume<TValue>(this TValue? nullable, Action<TValue>? success = null, Action? error = null)
        where TValue : class
    {
        if (nullable is null)
        {
            error?.Invoke();
            return false;
        }
        else
        {
            success?.Invoke(nullable);
            return true;
        }
    }

    public static bool Consume<TValue>(this TValue? nullable, Action<TValue>? success = null, Action? error = null)
        where TValue : struct
    {
        if (nullable.HasValue)
        {
            success?.Invoke(nullable.Value);
            return true;
        }
        else
        {
            error?.Invoke();
            return false;
        }
    }
}