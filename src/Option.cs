namespace Ametrin.Optional;

public readonly struct Option : IEquatable<Option>
{
    internal readonly bool _success;

    public Option() : this(true) { }
    internal Option(bool success)
    {
        _success = success;
    }

    public static Option Success() => new(true);
    public static Option Fail() => new(false);

    public static Option<T> None<T>() => default;
    public static Option<T> Some<T>(T value)
        => value is null ? throw new ArgumentNullException(nameof(value), "Cannot create Option with null value") : new(value, true);

    public static Option<T> Of<T>(T? value)
        => value is null ? default : new(value, true);
    public static Option<T> Of<T>(T? value) where T : struct
        => value.HasValue ? new(value.Value, true) : default;

    public TResult Select<TResult>(Func<TResult> success, Func<TResult> fail) => _success ? success() : fail();

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

    [Obsolete]
    public void IfSuccess(Action action)
    {
        if (_success)
        {
            action();
        }
    }

    [Obsolete]
    public void IfFail(Action action)
    {
        if (!_success)
        {
            action();
        }
    }

    public override string ToString() => _success ? "Success" : "Error";
    public override int GetHashCode() => _success.GetHashCode();
    public override bool Equals(object? obj) => obj is Option s && Equals(s);
    public bool Equals(Option other) => _success == other._success;
    public static bool operator ==(Option left, Option right) => left.Equals(right);
    public static bool operator !=(Option left, Option right) => !(left == right);

    public static implicit operator bool(Option state) => state._success;
    public static implicit operator Option(bool state) => new(state);
}

public readonly struct Option<T> : IEquatable<Option<T>>, IEquatable<T>
{
    internal readonly T _value;
    internal readonly bool _hasValue = false;

    public Option() : this(default!, false) { }
    internal Option(T value, bool hasValue)
        => (_value, _hasValue) = (value, hasValue);
    public Option(Option<T> other)
        : this(other._value, other._hasValue) { }

    public Option<T> Where(Func<T, bool> predicate)
        => _hasValue ? predicate(_value!) ? this : default : this;
    public Option<T> WhereNot(Func<T, bool> predicate)
        => _hasValue ? !predicate(_value!) ? this : default : this;

    public Option<TResult> Select<TResult>(Func<T, TResult> selector)
        => _hasValue ? selector(_value!) : default(Option<TResult>);
    public Result<TResult> Select<TResult>(Func<T, Result<TResult>> selector)
        => _hasValue ? selector(_value!) : null;
    public Option<TResult> Select<TResult>(Func<T, Option<TResult>> selector)
        => _hasValue ? selector(_value!) : default;

    public Option<TResult> Where<TResult>()
        => _hasValue && _value is TResult casted ? casted : default(Option<TResult>);

#if NET9_0_OR_GREATER
    [OverloadResolutionPriority(1)] // to allow 'Or(null)' which would normally be ambigious
#endif
    public T Or(T other) => _hasValue ? _value! : other;
    public T Or(Func<T> factory) => _hasValue ? _value! : factory();
    public T OrThrow() => _hasValue ? _value! : throw new NullReferenceException("Option was None");

    public void Consume(Action<T>? success = null, Action? error = null)
    {
        if (_hasValue)
        {
            success?.Invoke(_value);
        }
        else
        {
            error?.Invoke();
        }
    }

    public bool Equals(Option<T> other)
        => _hasValue ? other._hasValue && _value!.Equals(other._value) : !other._hasValue;
    public bool Equals(T? other)
        => _hasValue ? other is not null && _value!.Equals(other) : other is null;

    public override string ToString() => _hasValue ? _value!.ToString() ?? "NullString" : "None";
    public override int GetHashCode() => HashCode.Combine(_hasValue, _value);
    public override bool Equals(object? obj) => obj switch
    {
        Option<T> option => Equals(option),
        T value => Equals(value),
        _ => false,
    };

    public static bool operator ==(Option<T> left, Option<T> right) => left.Equals(right);
    public static bool operator !=(Option<T> left, Option<T> right) => !(left == right);

    public static bool operator ==(Option<T> left, T right) => left.Equals(right);
    public static bool operator !=(Option<T> left, T right) => !(left == right);

    public static implicit operator Option<T>(T? value) => Option.Of(value);
}