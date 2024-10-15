namespace Ametrin.Optional;

public static class Option
{
    public static Option<T> None<T>() => default;
    public static Option<T> Some<T>(T value)
        => value is null ? throw new ArgumentNullException(nameof(value), "Cannot create Option with null value") : new(value, true);

    public static Option<T> Of<T>(T? value)
        => value is null ? default : new(value, true);
    public static Option<T> Of<T>(T? value) where T : struct
        => value.HasValue ? new(value.Value, true) : default;

    public static T? OrNull<T>(this Option<T> option) where T : class
        => option._hasValue ? option._value! : null;
    public static T OrDefault<T>(this Option<T> option) where T : struct
        => option._hasValue ? option._value! : default;

    public static Option<string> WhereNotEmpty(this Option<string> option)
        => option.WhereNot(string.IsNullOrEmpty);

    public static Option<string> WhereNotWhiteSpace(this Option<string> option)
        => option.WhereNot(string.IsNullOrWhiteSpace);

    public static Option<T> WhereExists<T>(this Option<T> option) where T : FileSystemInfo
        => option.Where(static info => info.Exists);
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
        => _hasValue ? selector(_value!) : default;
    public Option<TResult> Select<TResult>(Func<T, Option<TResult>> selector)
        => _hasValue ? selector(_value!) : default;

    public Option<TResult> Cast<TResult>()
        => _hasValue && _value is TResult casted ? casted : default;

    [OverloadResolutionPriority(1)] // to allow 'Or(null)' which would normally be ambigious
    public T Or(T other) => _hasValue ? _value! : other;
    public T Or(Func<T> factory) => _hasValue ? _value! : factory();
    public T OrThrow() => _hasValue ? _value! : throw new NullReferenceException("Option was None");

    public void Consume(Action? fail = null, Action<T>? some = null) => Consume(some, fail);
    public void Consume(Action<T>? some = null, Action? fail = null)
    {
        if (_hasValue)
        {
            some?.Invoke(_value);
        }
        else
        {
            fail?.Invoke();
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