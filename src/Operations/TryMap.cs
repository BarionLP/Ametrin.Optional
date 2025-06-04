namespace Ametrin.Optional;

partial struct Option<TValue>
{
    public Option<TResult> TryMap<TResult>(Func<TValue, TResult> map)
    {
        if (_hasValue)
        {
            try
            {
                return map(_value);
            }
            catch { }
        }

        return default;
    }

    public Option<TResult> TryMap<TArg, TResult>(TArg arg, Func<TValue, TArg, TResult> map)
        where TArg : allows ref struct
    {
        if (_hasValue)
        {
            try
            {
                return map(_value, arg);
            }
            catch { }
        }

        return default;
    }
}

partial struct Result<TValue>
{
    public Result<TResult> TryMap<TResult>(Func<TValue, TResult> map)
    {
        if (!_hasValue)
        {
            return _error;
        }

        try
        {
            return map(_value);
        }
        catch (Exception e)
        {
            return e;
        }
    }

    [Obsolete("use TryMap and MapError", error: true)]
    public Result<TResult, TNewError> TryMap<TResult, TNewError>(Func<TValue, TResult> map, Func<Exception, TNewError> errormap)
    {
        return TryMap(map).MapError(errormap);
    }

    public Result<TResult> TryMap<TArg, TResult>(TArg arg, Func<TValue, TArg, TResult> map)
        where TArg : allows ref struct
    {
        if (!_hasValue)
        {
            return _error;
        }

        try
        {
            return map(_value, arg);
        }
        catch (Exception e)
        {
            return e;
        }
    }
}

partial struct Result<TValue, TError>
{
    public Result<TResult, TError> TryMap<TResult>(Func<TValue, TResult> map, Func<Exception, TError> errormap)
    {
        if (!_hasValue)
        {
            return _error;
        }

        try
        {
            return map(_value);
        }
        catch (Exception e)
        {
            return errormap(e);
        }
    }

    public Result<TResult, TError> TryMap<TArg, TResult>(TArg arg, Func<TValue, TArg, TResult> map, Func<Exception, TError> errormap)
        where TArg : allows ref struct
    {
        if (!_hasValue)
        {
            return _error;
        }

        try
        {
            return map(_value, arg);
        }
        catch (Exception e)
        {
            return errormap(e);
        }
    }
}

partial struct RefOption<TValue>
{
    public RefOption<TResult> TryMap<TResult>(Func<TValue, TResult> map)
        where TResult : struct, allows ref struct
    {
        if (!_hasValue)
        {
            return default;
        }

        try
        {
            return map(_value);
        }
        catch
        {
            return default;
        }
    }

    public RefOption<TResult> TryMap<TArg, TResult>(TArg arg, Func<TValue, TArg, TResult> map)
        where TResult : struct, allows ref struct
        where TArg : allows ref struct
    {
        if (!_hasValue)
        {
            return default;
        }

        try
        {
            return map(_value, arg);
        }
        catch
        {
            return default;
        }
    }
}

public static class OptionTryMapExtensions
{
    public static Option<TResult> TryMap<TValue, TResult>(this RefOption<TValue> option, Func<TValue, TResult> map)
        where TValue : struct, allows ref struct
        where TResult : class
    {
        if (option._hasValue)
        {
            try
            {
                return map(option._value);
            }
            catch { }
        }

        return default;
    }

    public static RefOption<TResult> TryMap<TValue, TResult>(this Option<TValue> option, Func<TValue, TResult> map)
        where TResult : struct, allows ref struct
    {
        if (option._hasValue)
        {
            try
            {
                return RefOption.Success(map(option._value));
            }
            catch { }
        }
        return default;
    }

    public static Option<TResult> TryMap<TValue, TArg, TResult>(this RefOption<TValue> option, TArg arg, Func<TValue, TArg, TResult> map)
        where TValue : struct, allows ref struct
        where TResult : class
        where TArg : allows ref struct
    {
        if (option._hasValue)
        {
            try
            {
                return map(option._value, arg);
            }
            catch { }
        }

        return default;
    }

    public static RefOption<TResult> TryMap<TValue, TArg, TResult>(this Option<TValue> option, TArg arg, Func<TValue, TArg, TResult> map)
        where TResult : struct, allows ref struct
        where TArg : allows ref struct
    {
        if (option._hasValue)
        {
            try
            {
                return RefOption.Success(map(option._value, arg));
            }
            catch { }
        }
        return default;
    }
}
