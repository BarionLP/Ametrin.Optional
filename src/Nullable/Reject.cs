namespace Ametrin.Optional.Nullable;

public static class NullableRejectExtensions
{
    extension<TValue>(TValue? value) where TValue : class
    {
        public TValue? Reject(Func<TValue, bool> predicate)
            => value is null ? null : (predicate(value) ? null : value);

        public TValue? Reject<TArg>(TArg arg, Func<TValue, TArg, bool> predicate)
            where TArg : allows ref struct
            => value is null ? null : (predicate(value, arg) ? null : value);
    }

    extension<TValue>(TValue? value) where TValue : struct
    {
        public TValue? Reject(Func<TValue, bool> predicate)
            => value.HasValue ? (predicate(value.Value) ? null : value) : null;

        public TValue? Reject<TArg>(TArg arg, Func<TValue, TArg, bool> predicate)
            where TArg : allows ref struct
            => value.HasValue ? (predicate(value.Value, arg) ? null : value) : null;
    }
}
