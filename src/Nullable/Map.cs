namespace Ametrin.Optional.Nullable;

public static class NullableMapExtensions
{
    public static TResult? Map<TValue, TResult>(this TValue? value, Func<TValue, TResult> map)
        where TValue : class
        where TResult : class
        => value is null ? null : map(value);
    public static TResult? Map<TValue, TResult>(this TValue? value, Func<TValue, TResult> map)
        where TValue : struct
        where TResult : class
        => value.HasValue ? map(value.Value) : null;

    public static TResult? Map<TValue, TArg, TResult>(this TValue? value, TArg arg, Func<TValue, TArg, TResult> map)
        where TValue : class
        where TResult : class
        => value is null ? null : map(value, arg);
    public static TResult? Map<TValue, TArg, TResult>(this TValue? value, TArg arg, Func<TValue, TArg, TResult> map)
        where TValue : struct
        where TResult : class
        => value.HasValue ? map(value.Value, arg) : null;
}


public static class NullableValueMapExtensions
{
    public static TResult? Map<TValue, TResult>(this TValue? value, Func<TValue, TResult> map)
        where TValue : struct
        where TResult : struct
        => value.HasValue ? map(value.Value) : null;
    public static TResult? Map<TValue, TResult>(this TValue? value, Func<TValue, TResult> map)
        where TValue : class
        where TResult : struct
        => value is null ? null : map(value);

    public static TResult? Map<TValue, TArg, TResult>(this TValue? value, TArg arg, Func<TValue, TArg, TResult> map)
        where TValue : struct
        where TResult : struct
        => value.HasValue ? map(value.Value, arg) : null;
    public static TResult? Map<TValue, TArg, TResult>(this TValue? value, TArg arg, Func<TValue, TArg, TResult> map)
        where TValue : class
        where TResult : struct
        => value is null ? null : map(value, arg);
}