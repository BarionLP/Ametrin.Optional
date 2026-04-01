namespace Ametrin.Optional.Nullable;

public static class NullableMatchExtensions
{
    extension<TValue>(TValue? nullable) where TValue : class
    {
        public TResult Match<TResult>(Func<TValue, TResult> success, Func<TResult> error)
            => nullable is null ? error.Invoke() : success.Invoke(nullable);
    }

    extension<TValue>(TValue? nullable) where TValue : struct
    {
        public TResult Match<TResult>(Func<TValue, TResult> success, Func<TResult> error)
            => nullable.HasValue ? success.Invoke(nullable.Value) : error.Invoke();
    }
}