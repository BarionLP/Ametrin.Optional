namespace Ametrin.Optional;

partial struct Option
{
    public void Consume(Action? success = null, Action? error = null)
    {
        if (_success)
        {
            success?.Invoke();
        }
        else
        {
            error?.Invoke();
        }
    }

    public void Consume<TArg>(TArg arg, Action<TArg>? success = null, Action<TArg>? error = null)
        where TArg : allows ref struct
    {
        if (_success)
        {
            success?.Invoke(arg);
        }
        else
        {
            error?.Invoke(arg);
        }
    }
}

partial struct Option<TValue>
{
    [AsyncExtension]
    public Option Consume(Action<TValue>? success = null, Action? error = null)
    {
        if (_hasValue)
        {
            success?.Invoke(_value);
        }
        else
        {
            error?.Invoke();
        }

        return _hasValue;
    }

    public Option Consume<TArg>(TArg arg, Action<TValue, TArg>? success = null, Action<TArg>? error = null)
        where TArg : allows ref struct
    {
        if (_hasValue)
        {
            success?.Invoke(_value, arg);
        }
        else
        {
            error?.Invoke(arg);
        }

        return _hasValue;
    }
}

partial struct Result<TValue>
{
    [AsyncExtension]
    public ErrorState Consume(Action<TValue>? success = null, Action<Exception>? error = null)
    {
        if (_hasValue)
        {
            success?.Invoke(_value);
            return default;
        }
        else
        {
            error?.Invoke(_error);
            return _error;
        }
    }

    public ErrorState Consume<TArg>(TArg arg, Action<TValue, TArg>? success = null, Action<Exception, TArg>? error = null)
        where TArg : allows ref struct
    {
        if (_hasValue)
        {
            success?.Invoke(_value, arg);
            return default;
        }
        else
        {
            error?.Invoke(_error, arg);
            return _error;
        }
    }
}

partial struct Result<TValue, TError>
{
    [AsyncExtension]
    public ErrorState<TError> Consume(Action<TValue>? success = null, Action<TError>? error = null)
    {
        if (_hasValue)
        {
            success?.Invoke(_value);
            return default;
        }
        else
        {
            error?.Invoke(_error);
            return _error;
        }
    }


    public ErrorState<TError> Consume<TArg>(TArg arg, Action<TValue, TArg>? success = null, Action<TError, TArg>? error = null)
        where TArg : allows ref struct
    {
        if (_hasValue)
        {
            success?.Invoke(_value, arg);
            return default;
        }
        else
        {
            error?.Invoke(_error, arg);
            return _error;
        }
    }
}

partial struct ErrorState
{
    [AsyncExtension]
    public void Consume(Action? success = null, Action<Exception>? error = null)
    {
        if (_isError)
        {
            error?.Invoke(_error);
        }
        else
        {
            success?.Invoke();
        }
    }

    public void Consume<TArg>(TArg arg, Action<TArg>? success = null, Action<Exception, TArg>? error = null)
        where TArg : allows ref struct
    {
        if (_isError)
        {
            error?.Invoke(_error, arg);
        }
        else
        {
            success?.Invoke(arg);
        }
    }
}

partial struct ErrorState<TError>
{
    [AsyncExtension]
    public void Consume(Action? success = null, Action<TError>? error = null)
    {
        if (_isError)
        {
            error?.Invoke(_error);
        }
        else
        {
            success?.Invoke();
        }
    }

    public void Consume<TArg>(TArg arg, Action<TArg>? success = null, Action<TError, TArg>? error = null)
        where TArg : allows ref struct
    {
        if (_isError)
        {
            error?.Invoke(_error, arg);
        }
        else
        {
            success?.Invoke(arg);
        }
    }
}

partial struct RefOption<TValue>
{
    public Option Consume(Action<TValue>? success = null, Action? error = null)
    {
        if (_hasValue)
        {
            success?.Invoke(_value);
        }
        else
        {
            error?.Invoke();
        }

        return _hasValue;
    }

    public Option Consume<TArg>(TArg arg, Action<TValue, TArg>? success = null, Action<TArg>? error = null)
        where TArg : allows ref struct
    {
        if (_hasValue)
        {
            success?.Invoke(_value, arg);
        }
        else
        {
            error?.Invoke(arg);
        }

        return _hasValue;
    }
}
