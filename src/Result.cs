namespace Ametrin.Optional;


public static class Result
{
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

public readonly struct Result<TValue> : IEquatable<Result<TValue>>, IEquatable<TValue>
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

    public Result<TValue> Where(Func<TValue, bool> predicate, Exception? error = null)
        => _hasValue ? predicate(_value!) ? this : error : this;
    public Result<TValue> Where(Func<TValue, bool> predicate, Func<Exception> error)
        => _hasValue ? predicate(_value!) ? this : error() : this;
    public Result<TValue> WhereNot(Func<TValue, bool> predicate, Exception? error = null)
        => _hasValue ? !predicate(_value!) ? this : error : this;
    public Result<TValue> WhereNot(Func<TValue, bool> predicate, Func<Exception> error)
        => _hasValue ? !predicate(_value!) ? this : error() : this;
    public Result<TResult> Where<TResult>(Exception? error = null)
        => _hasValue && _value is TResult casted ? casted : error ?? new InvalidCastException($"Cannot cast ${typeof(TValue).Name} to ${typeof(TResult).Name}");

    public Result<TResult> Select<TResult>(Func<TValue, TResult> selector)
        => _hasValue ? selector(_value) : _error;
    public Result<TResult> Select<TResult>(Func<TValue, Result<TResult>> selector)
        => _hasValue ? selector(_value) : _error;

#if NET9_0_OR_GREATER
    [OverloadResolutionPriority(1)] // to allow 'Or(null)' which would normally be ambigious
#endif
    public TValue Or(TValue other) => _hasValue ? _value! : other;
    public TValue Or(Func<TValue> factory) => _hasValue ? _value! : factory();
    public TValue OrThrow() => _hasValue ? _value! : throw new NullReferenceException("Result was None");

#if NET9_0_OR_GREATER
    public void Consume(Action<Exception>? fail = null, Action<TValue>? some = null) => Consume(some, fail);
#endif
    public void Consume(Action<TValue>? some = null, Action<Exception>? fail = null)
    {
        if (_hasValue)
        {
            some?.Invoke(_value);
        }
        else
        {
            fail?.Invoke(_error);
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

public readonly struct Result<TValue, TError> : IEquatable<Result<TValue>>, IEquatable<TValue>
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

    public Result<TValue, TError> Where(Func<TValue, bool> predicate, TError error)
        => _hasValue ? predicate(_value!) ? this : error : this;
    public Result<TValue, TError> WhereNot(Func<TValue, bool> predicate, TError error)
        => _hasValue ? !predicate(_value!) ? this : error : this;
    public Result<TResult, TError> Where<TResult>(TError error)
        => _hasValue && _value is TResult casted ? casted : error;

    public Result<TResult, TError> Select<TResult>(Func<TValue, TResult> selector)
        => _hasValue ? selector(_value!) : _error;
    public Result<TResult, TError> Select<TResult>(Func<TValue, Result<TResult, TError>> selector)
        => _hasValue ? selector(_value!) : _error;


#if NET9_0_OR_GREATER
    [OverloadResolutionPriority(1)] // to allow 'Or(null)' which would normally be ambigious
#endif
    public TValue Or(TValue other) => _hasValue ? _value! : other;
    public TValue Or(Func<TValue> factory) => _hasValue ? _value! : factory();
    public TValue OrThrow() => _hasValue ? _value! : throw new NullReferenceException("Result was None");

    public void Consume(Action<TError>? fail = null, Action<TValue>? some = null) => Consume(some, fail);
    public void Consume(Action<TValue>? some = null, Action<TError>? fail = null)
    {
        if (_hasValue)
        {
            some?.Invoke(_value);
        }
        else
        {
            fail?.Invoke(_error);
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
