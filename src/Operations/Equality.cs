namespace Ametrin.Optional;

partial struct Option : IEquatable<Option>
{
    public override int GetHashCode() => _success.GetHashCode();

    public bool Equals(Option other) => _success == other._success;
    public override bool Equals(object? obj) => obj is Option s && Equals(s);

    public static bool operator ==(Option left, Option right) => left.Equals(right);
    public static bool operator !=(Option left, Option right) => !(left == right);
}

partial struct Option<TValue> : IEquatable<Option<TValue>>, IEquatable<TValue>
{
    public override int GetHashCode() => HashCode.Combine(_hasValue, _value);

    public bool Equals(Option<TValue> other)
        => _hasValue ? other._hasValue && EqualityComparer<TValue>.Default.Equals(_value, other._value) : !other._hasValue;
    public bool Equals(TValue? other)
        => _hasValue ? other is not null && EqualityComparer<TValue>.Default.Equals(_value, other) : other is null;
    public override bool Equals(object? obj) => obj switch
    {
        Option<TValue> option => Equals(option),
        TValue value => Equals(value),
        _ => false,
    };

    public static bool operator ==(Option<TValue> left, Option<TValue> right) => left.Equals(right);
    public static bool operator !=(Option<TValue> left, Option<TValue> right) => !(left == right);

    public static bool operator ==(Option<TValue> left, TValue right) => left.Equals(right);
    public static bool operator !=(Option<TValue> left, TValue right) => !(left == right);
}

partial struct Result<TValue> : IEquatable<Result<TValue>>, IEquatable<TValue>
{
    public override int GetHashCode() => _hasValue ? HashCode.Combine(_hasValue, _value) : HashCode.Combine(_hasValue, _error);

    public bool Equals(Result<TValue> other) => _hasValue
            ? other._hasValue && EqualityComparer<TValue>.Default.Equals(_value, other._value)
            : !other._hasValue && _error == other._error;
    public bool Equals(TValue? other)
        => _hasValue && other is not null && EqualityComparer<TValue>.Default.Equals(_value, other);
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
}

partial struct Result<TValue, TError> : IEquatable<Result<TValue, TError>>, IEquatable<TValue>
{
    public override int GetHashCode() => _hasValue ? HashCode.Combine(_hasValue, _value) : HashCode.Combine(_hasValue, _error);

    public bool Equals(Result<TValue, TError> other) => _hasValue
            ? other._hasValue && EqualityComparer<TValue>.Default.Equals(_value, other._value)
            : !other._hasValue && EqualityComparer<TError>.Default.Equals(_error, other._error);

    public bool Equals(TValue? other)
        => _hasValue && other is not null && EqualityComparer<TValue>.Default.Equals(_value, other);

    public override bool Equals(object? obj) => obj switch
    {
        Result<TValue, TError> result => Equals(result),
        TValue value => Equals(value),
        _ => false,
    };

    public static bool operator ==(Result<TValue, TError> left, Result<TValue, TError> right) => left.Equals(right);
    public static bool operator !=(Result<TValue, TError> left, Result<TValue, TError> right) => !(left == right);

    public static bool operator ==(Result<TValue, TError> left, TValue right) => left.Equals(right);
    public static bool operator !=(Result<TValue, TError> left, TValue right) => !(left == right);
}

partial struct ErrorState : IEquatable<ErrorState>
{
    public override int GetHashCode() => _isFail ? HashCode.Combine(_isFail.GetHashCode(), _error.GetHashCode()) : _isFail.GetHashCode();

    public bool Equals(ErrorState other)
        => _isFail ? other._isFail && _error == other._error : !other._isFail;
    public override bool Equals(object? obj) => obj is ErrorState s && Equals(s);

    public static bool operator ==(ErrorState left, ErrorState right) => left.Equals(right);
    public static bool operator !=(ErrorState left, ErrorState right) => !(left == right);
}

partial struct ErrorState<TError> : IEquatable<ErrorState<TError>>
{
    public override int GetHashCode() => _isFail ? HashCode.Combine(_isFail.GetHashCode(), _error!.GetHashCode()) : _isFail.GetHashCode();

    public bool Equals(ErrorState<TError> other)
        => _isFail ? other._isFail && EqualityComparer<TError>.Default.Equals(_error, other._error) : !other._isFail;
    public override bool Equals(object? obj) => obj is ErrorState<TError> s && Equals(s);

    public static bool operator ==(ErrorState<TError> left, ErrorState<TError> right) => left.Equals(right);
    public static bool operator !=(ErrorState<TError> left, ErrorState<TError> right) => !(left == right);
}