namespace Ametrin.Optional.Nullable;

public static class NullableRequireExtensions
{
    extension<TValue>(TValue? value) where TValue : class
    {
        public TValue? Require(Func<TValue, bool> predicate)
            => value is null ? null : (predicate(value) ? value : null);

        public TValue? Require<TArg>(TArg arg, Func<TValue, TArg, bool> predicate)
            where TArg : allows ref struct
            => value is null ? null : (predicate(value, arg) ? value : null);
    }

    extension<TValue>(TValue? value) where TValue : struct
    {
        public TValue? Require(Func<TValue, bool> predicate)
            => value.HasValue ? (predicate(value.Value) ? value : null) : null;

        public TValue? Require<TArg>(TArg arg, Func<TValue, TArg, bool> predicate)
            where TArg : allows ref struct
            => value.HasValue ? (predicate(value.Value, arg) ? value : null) : null;
    }
}