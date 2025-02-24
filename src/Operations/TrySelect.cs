namespace Ametrin.Optional;

partial struct Option<TValue>
{
    [Obsolete("use TryMap")]
    public Option<TResult> TrySelect<TResult>(Func<TValue, TResult> selector)
    {
        if (_hasValue)
        {
            try
            {
                return selector(_value);
            }
            catch { }
        }

        return default;
    }
}

partial struct Result<TValue>
{
    [Obsolete("use TryMap")]
    public Result<TResult> TrySelect<TResult>(Func<TValue, TResult> selector)
    {
        if (!_hasValue)
        {
            return _error;
        }

        try
        {
            return selector(_value);
        }
        catch (Exception e)
        {
            return e;
        }
    }

    [Obsolete("use TryMap")]
    public Result<TResult, TNewError> TrySelect<TResult, TNewError>(Func<TValue, TResult> selector, Func<Exception, TNewError> errorSelector)
    {
        if (!_hasValue)
        {
            return errorSelector(_error);
        }

        try
        {
            return selector(_value);
        }
        catch (Exception e)
        {
            return errorSelector(e);
        }
    }
}

partial struct Result<TValue, TError>
{
    [Obsolete("use TryMap")]
    public Result<TResult, TError> TrySelect<TResult>(Func<TValue, TResult> selector, Func<Exception, TError> errorSelector)
    {
        if (!_hasValue)
        {
            return _error;
        }

        try
        {
            return selector(_value);
        }
        catch (Exception e)
        {
            return errorSelector(e);
        }
    }
}

partial struct RefOption<TValue>
{
    [Obsolete("use TryMap")]
    public RefOption<TResult> TrySelect<TResult>(Func<TValue, TResult> selector)
        where TResult : struct, allows ref struct
    {
        if (!_hasValue)
        {
            return default;
        }

        try
        {
            return selector(_value);
        }
        catch
        {
            return default;
        }
    }
}

public static class TrySelectOptionExtensions
{
    [Obsolete("use TryMap")]
    public static Option<TResult> TrySelect<TValue, TResult>(this RefOption<TValue> option, Func<TValue, TResult> selector)
        where TValue : struct, allows ref struct
        where TResult : class
    {
        if (option._hasValue)
        {
            try
            {
                return selector(option._value);
            }
            catch { }
        }

        return default;
    }

    [Obsolete("use TryMap")]
    public static RefOption<TResult> TrySelect<TValue, TResult>(this Option<TValue> option, Func<TValue, TResult> selector)
        where TResult : struct, allows ref struct
    {
        if (option._hasValue)
        {
            try
            {
                return RefOption.Success(selector(option._value));
            }
            catch { }
        }
        return default;
    }
}