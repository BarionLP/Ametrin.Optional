namespace Ametrin.Optional;

// these classes use isFail so the default value is a success state.
// this behaviour should be kept internal to avoid confusion
public readonly partial struct ErrorState
{
    public bool IsSuccess => !_isFail;
    public bool IsFail => _isFail;
    internal readonly bool _isFail;
    internal readonly Exception _error;

    public ErrorState() : this(false, default!) { }
    internal ErrorState(bool isFail, Exception error)
    {
        _isFail = isFail;
        _error = error;
    }

    // public static implicit operator bool(ErrorState state) => !state._isFail;
    public static implicit operator ErrorState(Exception? error) => error is null ? Success() : Error(error);
}

public readonly partial struct ErrorState<TError>
{
    public bool IsSuccess => !_isFail;
    public bool IsFail => _isFail;
    internal readonly bool _isFail;
    internal readonly TError _error;

    public ErrorState() : this(false, default!) { }
    internal ErrorState(bool isFail, TError error)
    {
        _isFail = isFail;
        _error = error;
    }

    // public static implicit operator bool(ErrorState<T> state) => !state._isFail;
    public static implicit operator ErrorState<TError>(TError? state) => state is TError t ? ErrorState.Error(t) : ErrorState.Success<TError>();
}

partial struct ErrorState
{
    public static ErrorState Success() => new();
    public static ErrorState Error(Exception? error = null) => new(true, error ?? new Exception());

    public static ErrorState<TError> Success<TError>() => new();
    public static ErrorState<TError> Error<TError>(TError error) => new(true, error);
}