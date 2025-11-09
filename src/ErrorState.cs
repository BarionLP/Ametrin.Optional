namespace Ametrin.Optional;

// these classes use isError instead of isSuccess so the default value (isError = false) is a success state.
// this behaviour should be kept internal to avoid confusion

/// <summary>
/// A simplified version of <see cref="ErrorState{TError}"/> with Exception as TError
/// </summary>
[GenerateAsyncExtensions]
public readonly partial struct ErrorState
{
    internal readonly bool _isError;
    internal readonly Exception _error;

    public ErrorState() : this(false, default!) { }
    internal ErrorState(bool isFail, Exception error)
    {
        _isError = isFail;
        _error = error;
    }
    public ErrorState(ErrorState other)
        : this(other._isError, other._error) { }
}

/// <summary>
/// A struct representing either success or an error value of <typeparamref name="TError"/>
/// </summary>
/// <typeparam name="TError">Type of the Error</typeparam>
[GenerateAsyncExtensions]
public readonly partial struct ErrorState<TError>
{
    internal readonly bool _isError;
    internal readonly TError _error;

    public ErrorState() : this(false, default!) { }
    internal ErrorState(bool isFail, TError error)
    {
        _isError = isFail;
        _error = error;
    }
    public ErrorState(ErrorState<TError> other) : this(other._isError, other._error) { }
}

partial struct ErrorState
{
    public static ErrorState Success() => new();

    [OverloadResolutionPriority(1)] // to avoid ambiguity with Error(TError) with subclasses of Exception
    public static ErrorState Error(Exception? error = null) => new(true, error ?? new Exception());

    public static ErrorState Try(Action action)
    {
        try
        {
            action();
            return default;
        }
        catch (Exception e)
        {
            return e;
        }
    }

    public static ErrorState Try<TArg>(TArg arg, Action<TArg> action)
        where TArg : allows ref struct
    {
        try
        {
            action(arg);
            return default;
        }
        catch (Exception e)
        {
            return e;
        }
    }

    public static ErrorState<TError> Success<TError>() => new();
    public static ErrorState<TError> Error<TError>(TError error) => new(true, error);

    public static ErrorState CombineErrors(ErrorState a, ErrorState b) => (a._isError, b._isError) switch
    {
        (false, false) => Success(),
        (true, false) => a._error,
        (false, true) => b._error,
        (true, true) => new AggregateException(a._error, b._error),
    };

    public static ErrorState<E> CombineErrors<E>(ErrorState<E> a, ErrorState<E> b, Func<E, E, E> errorCombiner) => (a._isError, b._isError) switch
    {
        (false, false) => Success<E>(),
        (true, false) => a._error,
        (false, true) => b._error,
        (true, true) => errorCombiner(a._error, b._error),
    };

    public static ErrorState<E> CombineErrors<E, TArg>(ErrorState<E> a, ErrorState<E> b, TArg arg, Func<E, E, TArg, E> errorCombiner) where TArg : allows ref struct => (a._isError, b._isError) switch
    {
        (false, false) => Success<E>(),
        (true, false) => a._error,
        (false, true) => b._error,
        (true, true) => errorCombiner(a._error, b._error, arg),
    };

    public static ErrorState CombineErrors(ErrorState a, ErrorState b, ErrorState c) => (a._isError, b._isError, c._isError) switch
    {
        (false, false, false) => Success(),
        (true, false, false) => a._error,
        (false, true, false) => b._error,
        (false, false, true) => c._error,
        (true, true, false) => new AggregateException(a._error, b._error),
        (true, false, true) => new AggregateException(a._error, c._error),
        (false, true, true) => new AggregateException(b._error, c._error),
        (true, true, true) => new AggregateException(a._error, b._error, c._error),
    };
}