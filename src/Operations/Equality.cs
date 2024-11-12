namespace Ametrin.Optional;

partial struct Option<T> : IEquatable<Option<T>>, IEquatable<T>
{
    public override int GetHashCode() => HashCode.Combine(_hasValue, _value);

    public bool Equals(Option<T> other)
        => _hasValue ? other._hasValue && _value!.Equals(other._value) : !other._hasValue;
    public bool Equals(T? other)
        => _hasValue ? other is not null && _value!.Equals(other) : other is null;

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
}
