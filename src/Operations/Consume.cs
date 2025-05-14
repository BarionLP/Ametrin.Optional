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
}

partial struct Option<TValue>
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
}

partial struct Result<TValue>
{
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
}

partial struct Result<TValue, TError>
{
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
}

partial struct ErrorState
{
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
}

partial struct ErrorState<TError>
{
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
}
