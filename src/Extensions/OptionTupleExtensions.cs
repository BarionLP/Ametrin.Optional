namespace Ametrin.Optional;

public static class OptionTupleExtensions
{
    [Obsolete("use Map")]
    public static Option<R> Select<R, T1, T2>(this (Option<T1>, Option<T2>) options, Func<T1, T2, R> selector)
        => options.Item1._hasValue && options.Item2._hasValue ? selector(options.Item1._value, options.Item2._value) : default(Option<R>);
    [Obsolete("use Map")]
    public static Option<R> Select<R, T1, T2>(this (Option<T1>, Option<T2>) options, Func<T1, T2, Option<R>> selector)
        => options.Item1._hasValue && options.Item2._hasValue ? selector(options.Item1._value, options.Item2._value) : default;
        
    public static Option<R> Map<R, T1, T2>(this (Option<T1>, Option<T2>) options, Func<T1, T2, R> selector)
        => options.Item1._hasValue && options.Item2._hasValue ? selector(options.Item1._value, options.Item2._value) : default(Option<R>);
    public static Option<R> Map<R, T1, T2>(this (Option<T1>, Option<T2>) options, Func<T1, T2, Option<R>> selector)
        => options.Item1._hasValue && options.Item2._hasValue ? selector(options.Item1._value, options.Item2._value) : default;

    public static void Consume<T1, T2>(this (Option<T1>, Option<T2>) options, Action<T1, T2>? success = null, Action? error = null)
    {
        if (options.Item1._hasValue && options.Item2._hasValue)
        {
            success?.Invoke(options.Item1._value, options.Item2._value);
        }
        else
        {
            error?.Invoke();
        }
    }

    public static void Consume<T1, T2>(this (Result<T1>, Result<T2>) options, Action<T1, T2>? success = null, Action? error = null)
    {
        if (options.Item1._hasValue && options.Item2._hasValue)
        {
            success?.Invoke(options.Item1._value, options.Item2._value);
        }
        else
        {
            error?.Invoke();
        }
    }
}
