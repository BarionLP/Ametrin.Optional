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

    public override string ToString() => _success ? "Success" : "None";
    public override int GetHashCode() => _success.GetHashCode();
    public override bool Equals(object? obj) => obj is ErrorState s && Equals(s);
    public bool Equals(ErrorState other) => _success == other._success;
    public static bool operator ==(ErrorState left, ErrorState right) => left.Equals(right);
    public static bool operator !=(ErrorState left, ErrorState right) => !(left == right);

    public static implicit operator bool(ErrorState state) => state._success;
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

    public override string ToString() => _success ? "Success" : _error?.ToString() ?? "Fail";
    public override int GetHashCode() => _success.GetHashCode();
    public override bool Equals(object? obj) => obj is ErrorState s && Equals(s);
    public bool Equals(ErrorState<T> other) => _success == other._success && EqualityComparer<T>.Default.Equals(_error, other._error);
    public static bool operator ==(ErrorState<T> left, ErrorState<T> right) => left.Equals(right);
    public static bool operator !=(ErrorState<T> left, ErrorState<T> right) => !(left == right);

    public static implicit operator bool(ErrorState<T> state) => state._success;
}
