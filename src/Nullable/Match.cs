namespace Ametrin.Optional.Nullable;

public static class NullableMatchExtensions
{
    public static TResult Match<TValue, TResult>(this TValue? nullable, Func<TValue, TResult> success, Func<TResult> error)
        where TValue : class
        => nullable is null ? error.Invoke() : success.Invoke(nullable);

    public static TResult Match<TValue, TResult>(this TValue? nullable, Func<TValue, TResult> success, Func<TResult> error)
        where TValue : struct
        => nullable.HasValue ? success.Invoke(nullable.Value) : error.Invoke();
}