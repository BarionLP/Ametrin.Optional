namespace Ametrin.Optional;

/// <summary>
/// A ref struct representing a value of type <typeparamref name="TValue"/> or error
/// </summary>
/// <typeparam name="TValue">ref struct type of the value</typeparam>
public readonly ref partial struct RefOption<TValue>
    where TValue : struct, allows ref struct
{
    internal readonly TValue _value;
    internal readonly bool _hasValue = false;

    public RefOption() : this(default!, false) { }
    internal RefOption(TValue value, bool hasValue)
    {
        _hasValue = hasValue;
        _value = value;
    }

    public RefOption(RefOption<TValue> other)
        : this(other._value, other._hasValue) { }
}

public static class RefOption
{
    public static RefOption<TValue> Success<TValue>(TValue value) where TValue : struct, allows ref struct
        => new(value, true);
    public static RefOption<TValue> Error<TValue>() where TValue : struct, allows ref struct
        => default;
}