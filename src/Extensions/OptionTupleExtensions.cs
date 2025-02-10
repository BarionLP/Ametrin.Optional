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

    public static R? Map<R, T1, T2>(this (T1?, T2?) options, Func<T1, T2, R> selector)
        where T1 : class
        where T2 : class
        where R : class
        => options.Item1 is not null && options.Item2 is not null ? selector(options.Item1, options.Item2) : null;
    public static R? Map<R, T1, T2>(this (T1?, T2?) options, Func<T1, T2, R> selector)
        where T1 : struct
        where T2 : class
        where R : class
        => options.Item1.HasValue && options.Item2 is not null ? selector(options.Item1.Value, options.Item2) : null;
    public static R? Map<R, T1, T2>(this (T1?, T2?) options, Func<T1, T2, R> selector)
        where T1 : class
        where T2 : struct
        where R : class
        => options.Item1 is not null && options.Item2.HasValue ? selector(options.Item1, options.Item2.Value) : null;

    public static Option Consume<T1, T2>(this (Option<T1>, Option<T2>) options, Action<T1, T2>? success = null, Action? error = null)
    {
        if (options.Item1._hasValue && options.Item2._hasValue)
        {
            success?.Invoke(options.Item1._value, options.Item2._value);
            return true;
        }
        else
        {
            error?.Invoke();
            return false;
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

    public static bool Consume<T1, T2>(this (T1?, T2?) options, Action<T1, T2>? success = null, Action? error = null)
        where T1 : class
        where T2 : class
    {
        if (options.Item1 is not null && options.Item2 is not null)
        {
            success?.Invoke(options.Item1, options.Item2);
            return true;
        }
        else
        {
            error?.Invoke();
            return false;
        }
    }
    public static bool Consume<T1, T2>(this (T1?, T2?) options, Action<T1, T2>? success = null, Action? error = null)
        where T1 : struct
        where T2 : class
    {
        if (options.Item1.HasValue && options.Item2 is not null)
        {
            success?.Invoke(options.Item1.Value, options.Item2);
            return true;
        }
        else
        {
            error?.Invoke();
            return false;
        }
    }
    public static bool Consume<T1, T2>(this (T1?, T2?) options, Action<T1, T2>? success = null, Action? error = null)
        where T1 : class
        where T2 : struct
    {
        if (options.Item1 is not null && options.Item2.HasValue)
        {
            success?.Invoke(options.Item1, options.Item2.Value);
            return true;
        }
        else
        {
            error?.Invoke();
            return false;
        }
    }
    public static bool Consume<T1, T2>(this (T1?, T2?) options, Action<T1, T2>? success = null, Action? error = null)
        where T1 : struct
        where T2 : struct
    {
        if (options.Item1.HasValue && options.Item2.HasValue)
        {
            success?.Invoke(options.Item1.Value, options.Item2.Value);
            return true;
        }
        else
        {
            error?.Invoke();
            return false;
        }
    }
}

public static class OptionValueTupleExtensions
{
    public static R? Map<R, T1, T2>(this (T1?, T2?) options, Func<T1, T2, R> selector)
        where T1 : class
        where T2 : class
        where R : struct
        => options.Item1 is not null && options.Item2 is not null ? selector(options.Item1, options.Item2) : null;
    public static R? Map<R, T1, T2>(this (T1?, T2?) options, Func<T1, T2, R> selector)
        where T1 : struct
        where T2 : class
        where R : struct
        => options.Item1.HasValue && options.Item2 is not null ? selector(options.Item1.Value, options.Item2) : null;
    public static R? Map<R, T1, T2>(this (T1?, T2?) options, Func<T1, T2, R> selector)
        where T1 : class
        where T2 : struct
        where R : struct
        => options.Item1 is not null && options.Item2.HasValue ? selector(options.Item1, options.Item2.Value) : null;
}