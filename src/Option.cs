namespace Ametrin.Optional;

public readonly partial struct Option
{
    internal readonly bool _success;

    public Option() : this(true) { }
    internal Option(bool success)
    {
        _success = success;
    }

    public static implicit operator bool(Option state) => state._success;
    public static implicit operator Option(bool state) => new(state);
}

public readonly partial struct Option<TValue>
{
    internal readonly TValue _value;
    internal readonly bool _hasValue = false;

    public Option() : this(default!, false) { }
    internal Option(TValue value, bool hasValue)
        => (_value, _hasValue) = (value, hasValue);
    public Option(Option<TValue> other)
        : this(other._value, other._hasValue) { }

    public static implicit operator Option<TValue>(TValue? value) => Option.Of(value);
}

partial struct Option
{
    public static Option Success() => new(true);
    public static Option Error() => new(false);

    public static Option<TValue> Success<TValue>(TValue value)
        => value is null ? throw new ArgumentNullException(nameof(value), "Cannot create Option with null") : new(value, true);
    public static Option<TValue> Error<TValue>() => default;

    public static Option<TValue> Of<TValue>(TValue? value)
        => value is null ? default : new(value, true);
    public static Option<TValue> Of<TValue>(TValue? value) where TValue : struct
        => value.HasValue ? new(value.Value, true) : default;
}