namespace Ametrin.Optional.Nullable;

public static class NullableRejectExtensions
{
    public static TValue? Reject<TValue>(this TValue? value, Func<TValue, bool> predicate)
        where TValue : class
        => value is null ? null : (predicate(value) ? null : value);

    public static TValue? Reject<TValue>(this TValue? value, Func<TValue, bool> predicate)
        where TValue : struct
        => value.HasValue ? (predicate(value.Value) ? null : value) : null;

    public static TValue? Reject<TValue, TArg>(this TValue? value, TArg arg, Func<TValue, TArg, bool> predicate)
        where TValue : class
        where TArg : allows ref struct
        => value is null ? null : (predicate(value, arg) ? null : value);

    public static TValue? Reject<TValue, TArg>(this TValue? value, TArg arg, Func<TValue, TArg, bool> predicate)
        where TValue : struct
        where TArg : allows ref struct
        => value.HasValue ? (predicate(value.Value, arg) ? null : value) : null;
}
