namespace Ametrin.Optional;

// these classes use isError instead of isSuccess so the default value (isError = false) is a success state.
// this behaviour should be kept internal to avoid confusion

/// <summary>
/// A simplified version of <see cref="ErrorState{TError}"/> with Exception as TError
/// </summary>
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
    public static ErrorState Error(Exception? error = null) => new(true, error ?? new Exception());
    // public static ErrorState Error<TError>(TError? error = null) where TError : Exception => new(true, error ?? new Exception());

    public static ErrorState<TError> Success<TError>() => new();
    public static ErrorState<TError> Error<TError>(TError error) => new(true, error);
}