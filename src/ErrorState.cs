namespace Ametrin.Optional;

public readonly struct ErrorState : IEquatable<ErrorState>
{
    internal readonly bool _success;
    public ErrorState() : this(true) { }
    internal ErrorState(bool success)
    {
        _success = success;
    }

    public static ErrorState Success() => new(true);
    public static ErrorState Fail() => new(false);

    public static ErrorState<T> Success<T>() => new(default!, true);
    public static ErrorState<T> Fail<T>(T error) => new(error, false);

    public TResult Select<TResult>(Func<TResult> success, Func<TResult> fail) => _success ? success() : fail();

    public void IfSuccess(Action action)
    {
        if (_success)
        {
            action();
        }
    }

    public void IfFail(Action action)
    {
        if (!_success)
        {
            action();
        }
    }

    public override string ToString() => _success ? "Success" : "None";
    public override int GetHashCode() => _success.GetHashCode();
    public override bool Equals(object? obj) => obj is ErrorState s && Equals(s);
    public bool Equals(ErrorState other) => _success == other._success;
    public static bool operator ==(ErrorState left, ErrorState right) => left.Equals(right);
    public static bool operator !=(ErrorState left, ErrorState right) => !(left == right);

    public static implicit operator bool(ErrorState state) => state._success;
    public static implicit operator ErrorState(bool state) => new(state);
}

public readonly struct ErrorState<T> : IEquatable<ErrorState<T>>
{
    internal readonly bool _success;
    internal readonly T _error;
    public ErrorState() : this(default!, true) { }
    internal ErrorState(T error, bool success)
    {
        _success = success;
        _error = error;
    }

    public TResult Select<TResult>(Func<TResult> success, Func<T, TResult> fail) => _success ? success() : fail(_error);
    public void IfSuccess(Action action)
    {
        if (_success)
        {
            action();
        }
    }

    public void IfFail(Action<T> action)
    {
        if (!_success)
        {
            action(_error);
        }
    }

    public override string ToString() => _success ? "Success" : _error?.ToString() ?? "Fail";
    public override int GetHashCode() => _success ? _success.GetHashCode() : HashCode.Combine(_success.GetHashCode(), _error!.GetHashCode());
    public override bool Equals(object? obj) => obj is ErrorState s && Equals(s);
    public bool Equals(ErrorState<T> other) => _success == other._success && (_success || EqualityComparer<T>.Default.Equals(_error, other._error));

    public static bool operator ==(ErrorState<T> left, ErrorState<T> right) => left.Equals(right);
    public static bool operator !=(ErrorState<T> left, ErrorState<T> right) => !(left == right);

    public static implicit operator bool(ErrorState<T> state) => state._success;
    public static implicit operator ErrorState<T>(T? state) => state is T t ? ErrorState.Fail(t) : ErrorState.Success<T>();
}
