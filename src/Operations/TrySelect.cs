namespace Ametrin.Optional;

partial struct Option<TValue>
{
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