namespace Ametrin.Optional;

public readonly partial struct Result<TValue>
{
    internal readonly TValue _value;
    internal readonly Exception _error;
    internal readonly bool _hasValue = false;

    public bool IsSuccess => _hasValue;
    public bool IsFail => !_hasValue;
    public Exception? Error => _hasValue ? null : _error;

    public Result() : this(null) { }
    public Result(Result<TValue> other)
        : this(other._value, other._error, other._hasValue) { }
    internal Result(TValue value) : this(value, default!, true) { }
    internal Result(Exception? error = null) : this(default!, error ?? new Exception(), false) { }
    internal Result(TValue value, Exception error, bool hasValue)
        => (_value, _error, _hasValue) = (value, error, hasValue);

    public static implicit operator Result<TValue>(TValue value) => Result.Success(value);
    public static implicit operator Result<TValue>(Exception? error) => Result.Error<TValue>(error);
}

public readonly partial struct Result<TValue, TError>
{
    internal readonly TValue _value;
    internal readonly TError _error;
    internal readonly bool _hasValue = false;

    public bool IsSuccess => _hasValue;
    public bool IsFail => !_hasValue;
    public TError? Error => _hasValue ? default : _error;

    public Result() : this(default(TError)!) { }
    public Result(Result<TValue, TError> other)
        : this(other._value, other._error, other._hasValue) { }
    internal Result(TValue value) : this(value, default!, true) { }
    internal Result(TError error) : this(default!, error, false) { }
    internal Result(TValue value, TError error, bool hasValue)
        => (_value, _error, _hasValue) = (value, error, hasValue);

    public static implicit operator Result<TValue, TError>(TValue value) => Result.Success<TValue, TError>(value);
    public static implicit operator Result<TValue, TError>(TError error) => Result.Error<TValue, TError>(error);
}

public static class Result
{
    public static Result<T> Success<T>(T value)
        => value is null ? throw new ArgumentNullException(nameof(value), "Cannot create Result with null") : new(value);
    public static Result<T> Error<T>(Exception? error = null) => new(error);

    public static Result<T> Of<T>(T? value, Exception? error = null) => value is null ? error : value;
    public static Result<T> Of<T>(T? value, Func<Exception> error) => value is null ? error() : value;
    public static Result<T> Of<T>(T? value, Exception? error = null) where T : struct
        => value.HasValue ? value.Value : error;

    public static Result<T> Try<T>(Func<T> action)
    {
        try
        {
            return action();
        }
        catch (Exception e)
        {
            return e;
        }
    }

    public static Result<TValue, TError> Success<TValue, TError>(TValue value)
        => value is null ? throw new ArgumentNullException(nameof(value), "Cannot create Result with null") : new(value);
    public static Result<TValue, TError> Error<TValue, TError>(TError error) => new(error);
}