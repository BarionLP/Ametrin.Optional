namespace Ametrin.Optional;

public readonly struct ErrorState : IEquatable<ErrorState>
{
    internal readonly bool _isFail;
    internal readonly Exception _error;
    public ErrorState() : this(false, default!) { }
    internal ErrorState(bool isFail, Exception error)
    {
        _isFail = isFail;
        _error = error;
    }

    public static ErrorState Success() => new();
    public static ErrorState Fail(Exception? error = null) => new(true, error ?? new Exception());

    public static ErrorState<T> Success<T>() => new();
    public static ErrorState<T> Fail<T>(T error) => new(true, error);

    public TResult Select<TResult>(Func<TResult> success, Func<TResult> fail) => _isFail ? fail() : success();

    public void IfSuccess(Action action)
    {
        if (!_isFail)
        {
            action();
        }
    }

    public void IfFail(Action action)
    {
        if (_isFail)
        {
            action();
        }
    }

    public override string ToString() => _isFail ? _error?.ToString() ?? "Fail" : "Success";
    public override int GetHashCode() => _isFail ? HashCode.Combine(_isFail.GetHashCode(), _error.GetHashCode()) : _isFail.GetHashCode();
    public override bool Equals(object? obj) => obj is ErrorState s && Equals(s);
    public bool Equals(ErrorState other)
        => _isFail ? other._isFail && _error == other._error : !other._isFail;

    public static bool operator ==(ErrorState left, ErrorState right) => left.Equals(right);
    public static bool operator !=(ErrorState left, ErrorState right) => !(left == right);

    public static implicit operator bool(ErrorState state) => !state._isFail;
    public static implicit operator ErrorState(Exception? error) => error is null ? Success() : Fail(error);
}

public readonly struct ErrorState<T> : IEquatable<ErrorState<T>>
{
    public bool IsSuccess => !_isFail;
    public bool IsFail => _isFail;
    internal readonly bool _isFail;
    internal readonly T _error;
    public ErrorState() : this(false, default!) { }
    internal ErrorState(bool isFail, T error)
    {
        _isFail = isFail;
        _error = error;
    }

    public TResult Select<TResult>(Func<TResult> success, Func<T, TResult> fail) => _isFail ? fail(_error) : success();
    public void IfSuccess(Action action)
    {
        if (!_isFail)
        {
            action();
        }
    }

    public void IfFail(Action<T> action)
    {
        if (_isFail)
        {
            action(_error);
        }
    }

    public void Consume(Action success, Action<T> fail)
    {
        if (_isFail)
        {
            fail(_error);
        }
        else
        {
            success();
        }
    }

    public override string ToString() => _isFail ? _error?.ToString() ?? "Fail" : "Success";
    public override int GetHashCode() => _isFail ? HashCode.Combine(_isFail.GetHashCode(), _error!.GetHashCode()) : _isFail.GetHashCode();
    public override bool Equals(object? obj) => obj is ErrorState<T> s && Equals(s);
    public bool Equals(ErrorState<T> other)
        => _isFail ? other._isFail && EqualityComparer<T>.Default.Equals(_error, other._error) : !other._isFail;

    public static bool operator ==(ErrorState<T> left, ErrorState<T> right) => left.Equals(right);
    public static bool operator !=(ErrorState<T> left, ErrorState<T> right) => !(left == right);

    public static implicit operator bool(ErrorState<T> state) => !state._isFail;
    public static implicit operator ErrorState<T>(T? state) => state is T t ? ErrorState.Fail(t) : ErrorState.Success<T>();
}
