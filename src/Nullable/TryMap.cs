namespace Ametrin.Optional.Nullable;

public static class NullableTryMapExtensions
{
    public static TResult? TryMap<TValue, TResult>(this TValue? value, Func<TValue, TResult> map)
        where TValue : class
        where TResult : class
    {
        if (value is not null)
        {
            try
            {
                return map(value);
            }
            catch { }
        }
        return null;
    }

    public static TResult? TryMap<TValue, TResult>(this TValue? value, Func<TValue, TResult> map)
        where TValue : struct
        where TResult : class
    {
        if (value.HasValue)
        {
            try
            {
                return map(value.Value);
            }
            catch { }
        }
        return null;
    }
}

public static class NullableValueTryMapExtensions
{
    public static TResult? TryMap<TValue, TResult>(this TValue? value, Func<TValue, TResult> map)
        where TValue : struct
        where TResult : struct
    {
        if (value.HasValue)
        {
            try
            {
                return map(value.Value);
            }
            catch { }
        }
        return null;
    }

    public static TResult? TryMap<TValue, TResult>(this TValue? value, Func<TValue, TResult> map)
        where TValue : class
        where TResult : struct
    {
        if (value is not null)
        {
            try
            {
                return map(value);
            }
            catch { }
        }
        return null;
    }
}