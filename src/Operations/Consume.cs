using System.Diagnostics.CodeAnalysis;

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

partial class OptionTupleExtensions
{
    public static Option Consume<T1, T2>(this (Option<T1>, Option<T2>) options, Action<T1, T2>? success = null, Action? error = null)
    {
        var (a, b) = options;
        if (a._hasValue && b._hasValue)
        {
            success?.Invoke(a._value, b._value);
            return true;
        }
        else
        {
            error?.Invoke();
            return false;
        }
    }

    public static Option Consume<T1, T2, TArg>(this (Option<T1>, Option<T2>) options, TArg arg, Action<T1, T2, TArg>? success = null, Action<TArg>? error = null)
        where TArg : allows ref struct
    {
        var (a, b) = options;
        if (a._hasValue && b._hasValue)
        {
            success?.Invoke(a._value, b._value, arg);
            return true;
        }
        else
        {
            error?.Invoke(arg);
            return false;
        }
    }

    public static ErrorState Consume<T1, T2>(this (Result<T1>, Result<T2>) options, Action<T1, T2>? success = null, Action<Exception>? error = null)
    {
        var (a, b) = options;

        if (a._hasValue && b._hasValue)
        {
            success?.Invoke(a._value, b._value);
            return default;
        }

        if (!ErrorState.CombineErrors(a.ToErrorState(), b.ToErrorState()).Branch(out var err))
        {
            error?.Invoke(err);
            return err;
        }

        throw new UnreachableException();
    }

    public static ErrorState Consume<T1, T2, TArg>(this (Result<T1>, Result<T2>) options, TArg arg, Action<T1, T2, TArg>? success = null, Action<Exception, TArg>? error = null)
        where TArg : allows ref struct
    {
        var (a, b) = options;

        if (a._hasValue && b._hasValue)
        {
            success?.Invoke(a._value, b._value, arg);
            return default;
        }

        if (!Result.CombineErrors(a, b).Branch(out var err))
        {
            error?.Invoke(err, arg);
            return err;
        }

        throw new UnreachableException();
    }

    [Experimental("AmOptional000")]
    public static ErrorState<E> Consume<T1, T2, E>(this (Result<T1, E>, Result<T2, E>) options, Func<E, E, E> errorCombiner, Action<T1, T2>? success = null, Action<E>? error = null)
    {
        var (a, b) = options;

        if (a._hasValue && b._hasValue)
        {
            success?.Invoke(a._value, b._value);
            return default;
        }

        if (!Result.CombineErrors(a, b, errorCombiner).Branch(out var err))
        {
            error?.Invoke(err);
            return err;
        }

        throw new UnreachableException();
    }

    [Experimental("AmOptional000")]
    public static ErrorState<E> Consume<T1, T2, E, TArg>(this (Result<T1, E>, Result<T2, E>) options, TArg arg, Func<E, E, TArg, E> errorCombiner, Action<T1, T2, TArg>? success = null, Action<E, TArg>? error = null)
        where TArg : allows ref struct
    {
        var (a, b) = options;

        if (a._hasValue && b._hasValue)
        {
            success?.Invoke(a._value, b._value, arg);
            return default;
        }

        if (!Result.CombineErrors(a, b, arg, errorCombiner).Branch(out var err))
        {
            error?.Invoke(err, arg);
            return err;
        }

        throw new UnreachableException();
    }
}