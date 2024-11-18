namespace Ametrin.Optional;

public readonly ref partial struct RefOption<TValue>
#if NET9_0_OR_GREATER
#endif
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

    public static RefOption<TValue> Of<TValue>(TValue? value) where TValue : struct
        => value is null ? default : new(value.Value, true);
}