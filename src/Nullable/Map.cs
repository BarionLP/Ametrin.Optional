namespace Ametrin.Optional.Nullable;

public static class NullableMapExtensions
{
    extension<TValue>(TValue? value) where TValue : class
    {
        public TResult? Map<TResult>(Func<TValue, TResult> map)
            where TResult : class
            => value is null ? null : map(value);

        public TResult? Map<TArg, TResult>(TArg arg, Func<TValue, TArg, TResult> map)
            where TResult : class
            => value is null ? null : map(value, arg);
    }

    extension<TValue>(TValue? value) where TValue : struct
    {
        public TResult? Map<TResult>(Func<TValue, TResult> map)
            where TResult : class
            => value.HasValue ? map(value.Value) : null;
        public TResult? Map<TArg, TResult>(TArg arg, Func<TValue, TArg, TResult> map)
            where TResult : class
            => value.HasValue ? map(value.Value, arg) : null;
    }
}


public static class NullableValueMapExtensions
{
    extension<TValue>(TValue? value) where TValue : struct
    {
        public TResult? Map<TResult>(Func<TValue, TResult> map)
            where TResult : struct
            => value.HasValue ? map(value.Value) : null;

        public TResult? Map<TArg, TResult>(TArg arg, Func<TValue, TArg, TResult> map)
            where TResult : struct
            => value.HasValue ? map(value.Value, arg) : null;
    }

    extension<TValue>(TValue? value) where TValue : class
    {
        public TResult? Map<TResult>(Func<TValue, TResult> map)
            where TResult : struct
            => value is null ? null : map(value);
        public TResult? Map<TArg, TResult>(TArg arg, Func<TValue, TArg, TResult> map)
            where TResult : struct
            => value is null ? null : map(value, arg);
    }
}