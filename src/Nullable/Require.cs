namespace Ametrin.Optional.Nullable;

public static class NullableRequireExtensions
{
    public static TValue? Require<TValue>(this TValue? value, Func<TValue, bool> predicate)
        where TValue : class
        => value is null ? null : (predicate(value) ? value : null);

    public static TValue? Require<TValue>(this TValue? value, Func<TValue, bool> predicate)
        where TValue : struct
        => value.HasValue ? (predicate(value.Value) ? value : null) : null;
}