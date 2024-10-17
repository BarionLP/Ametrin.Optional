namespace Ametrin.Optional;

public static class TupleExtensions
{
    public static Option<R> Select<R, T1, T2>(this (Option<T1>, Option<T2>) options, Func<T1, T2, R> selector)
        => options.Item1._hasValue && options.Item2._hasValue ? selector(options.Item1._value, options.Item2._value) : default;
    public static Option<R> Select<R, T1, T2>(this (Option<T1>, Option<T2>) options, Func<T1, T2, Option<R>> selector)
        => options.Item1._hasValue && options.Item2._hasValue ? selector(options.Item1._value, options.Item2._value) : default;

    public static void Consume<T1, T2>(this (Option<T1>, Option<T2>) options, Action? fail = null, Action<T1, T2>? action = null) 
        => Consume(options, action, fail);
    public static void Consume<T1, T2>(this (Option<T1>, Option<T2>) options, Action<T1, T2>? action = null, Action? fail = null)
    {
        if (options.Item1._hasValue && options.Item2._hasValue)
        {
            action?.Invoke(options.Item1._value, options.Item2._value);
        }
        else
        {
            fail?.Invoke();
        }
    }
    
    public static void Consume<T1, T2>(this (Result<T1>, Result<T2>) options, Action? fail = null, Action<T1, T2>? action = null) 
        => Consume(options, action, fail);
    public static void Consume<T1, T2>(this (Result<T1>, Result<T2>) options, Action<T1, T2>? action = null, Action? fail = null)
    {
        if (options.Item1._hasValue && options.Item2._hasValue)
        {
            action?.Invoke(options.Item1._value, options.Item2._value);
        }
        else
        {
            fail?.Invoke();
        }
    }
}
