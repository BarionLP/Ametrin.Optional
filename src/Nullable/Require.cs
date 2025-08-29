namespace Ametrin.Optional.Nullable;

public static class NullableRequireExtensions
{
    public static TValue? Require<TValue>(this TValue? value, Func<TValue, bool> predicate)
        where TValue : class
        => value is null ? null : (predicate(value) ? value : null);
    public static TValue? Require<TValue>(this TValue? value, Func<TValue, bool> predicate)
        where TValue : struct
        => value.HasValue ? (predicate(value.Value) ? value : null) : null;

    public static TValue? Require<TValue, TArg>(this TValue? value, TArg arg, Func<TValue, TArg, bool> predicate)
        where TValue : class
        where TArg : allows ref struct
    => value is null ? null : (predicate(value, arg) ? value : null);
    public static TValue? Require<TValue, TArg>(this TValue? value, TArg arg, Func<TValue, TArg, bool> predicate)
        where TValue : struct
        where TArg : allows ref struct
        => value.HasValue ? (predicate(value.Value, arg) ? value : null) : null;
}