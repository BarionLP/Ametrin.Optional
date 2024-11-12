namespace Ametrin.Optional;


public static class Result
{
    public static Result<T> Of<T>(T? value, Exception? error = null) => value is null ? error : value;
    public static Result<T> Of<T>(T? value, Func<Exception> error) => value is null ? error() : value;
    public static Result<T> Of<T>(T? value) where T : struct
        => value ?? default(Result<T>);
    public static Result<T> Fail<T>(Exception? error = null) => new(error);
    public static Result<T> Some<T>(T value)
        => value is null ? throw new ArgumentNullException(nameof(value), "Cannot create result with null value") : new(value);

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

    public static Result<TValue, TError> Fail<TValue, TError>(TError error) => new(error);
    public static Result<TValue, TError> Some<TValue, TError>(TValue value)
        => value is null ? throw new ArgumentNullException(nameof(value), "Cannot create result with null value") : new(value);

    public static T? OrNull<T>(this Result<T> option) where T : class
        => option._hasValue ? option._value : null;

    public static Result<T> Dispose<T>(this Result<T> option) where T : IDisposable
    {
        if (option._hasValue)
        {
            option._value.Dispose();
        }

        return new ObjectDisposedException(typeof(T).Name, "Result has been disposed");
    }
}

public readonly partial struct Result<TValue> : IEquatable<Result<TValue>>, IEquatable<TValue>
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

    public void Consume(Action<TValue>? success = null, Action<Exception>? error = null)
    {
        if (_hasValue)
        {
            success?.Invoke(_value);
        }
        else
        {
            error?.Invoke(_error);
        }
    }

    public bool Equals(Result<TValue> other)
        => _hasValue ? other._hasValue && _value!.Equals(other._value) : !other._hasValue;
    public bool Equals(TValue? other)
        => _hasValue ? other is not null && _value!.Equals(other) : other is null;

    public override string ToString() => _hasValue ? _value?.ToString() ?? "NullString" : _error.Message ?? "Failed";
    public override int GetHashCode() => HashCode.Combine(_hasValue, _value);
    public override bool Equals(object? obj) => obj switch
    {
        Result<TValue> result => Equals(result),
        TValue value => Equals(value),
        _ => false,
    };

    public static bool operator ==(Result<TValue> left, Result<TValue> right) => left.Equals(right);
    public static bool operator !=(Result<TValue> left, Result<TValue> right) => !(left == right);

    public static bool operator ==(Result<TValue> left, TValue right) => left.Equals(right);
    public static bool operator !=(Result<TValue> left, TValue right) => !(left == right);

    public static implicit operator Result<TValue>(TValue value) => Result.Some(value);
    public static implicit operator Result<TValue>(Exception? error) => Result.Fail<TValue>(error);
}

public readonly partial struct Result<TValue, TError> : IEquatable<Result<TValue>>, IEquatable<TValue>
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

    public void Consume(Action<TValue>? success = null, Action<TError>? error = null)
    {
        if (_hasValue)
        {
            success?.Invoke(_value);
        }
        else
        {
            error?.Invoke(_error);
        }
    }

    public bool Equals(Result<TValue> other)
        => _hasValue ? other._hasValue && _value!.Equals(other._value) : !other._hasValue;
    public bool Equals(TValue? other)
        => _hasValue ? other is not null && _value!.Equals(other) : other is null;

    public override string ToString() => _hasValue ? _value!.ToString() ?? "NullString" : _error!.ToString() ?? "Failed";
    public override int GetHashCode() => HashCode.Combine(_hasValue, _value);
    public override bool Equals(object? obj) => obj switch
    {
        Result<TValue> result => Equals(result),
        TValue value => Equals(value),
        _ => false,
    };

    public static bool operator ==(Result<TValue, TError> left, Result<TValue, TError> right) => left.Equals(right);
    public static bool operator !=(Result<TValue, TError> left, Result<TValue, TError> right) => !(left == right);

    public static bool operator ==(Result<TValue, TError> left, TValue right) => left.Equals(right);
    public static bool operator !=(Result<TValue, TError> left, TValue right) => !(left == right);

    public static implicit operator Result<TValue, TError>(TValue value) => Result.Some<TValue, TError>(value);
    public static implicit operator Result<TValue, TError>(TError error) => Result.Fail<TValue, TError>(error);
}
