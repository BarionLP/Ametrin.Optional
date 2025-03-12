namespace Ametrin.Optional.Nullable;

public static class NullableRejectExtensions
{
    public static TValue? Reject<TValue>(this TValue? value, Func<TValue, bool> predicate)
        where TValue : class
        => value is null ? null : (predicate(value) ? null : value);

    public static TValue? Reject<TValue>(this TValue? value, Func<TValue, bool> predicate)
        where TValue : struct
        => value.HasValue ? (predicate(value.Value) ? null : value) : null;
}
