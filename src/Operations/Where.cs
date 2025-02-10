namespace Ametrin.Optional;

partial struct Option<TValue>
{
    public Option<TValue> Where(Func<TValue, bool> predicate)
        => _hasValue ? (predicate(_value!) ? this : default) : this;

    public Option<TResult> Where<TResult>()
        => _hasValue && _value is TResult casted ? casted : Option.Error<TResult>();

    public Option<TValue> WhereNot(Func<TValue, bool> predicate)
        => _hasValue ? (predicate(_value!) ? default : this) : this;
}

partial struct Result<TValue>
{
    public Result<TValue> Where(Func<TValue, bool> predicate, Exception? error = null)
        => _hasValue ? (predicate(_value) ? this : error) : this;
    public Result<TValue> Where(Func<TValue, bool> predicate, Func<TValue, Exception> error)
        => _hasValue ? (predicate(_value) ? this : error(_value)) : this;

    public Result<TResult> Where<TResult>(Exception? error = null)
        => _hasValue ? (_value is TResult casted ? casted : error ?? new InvalidCastException($"Cannot cast ${typeof(TValue).Name} to ${typeof(TResult).Name}")) : _error;
    public Result<TResult> Where<TResult>(Func<TValue, Exception> error)
        => _hasValue ? (_value is TResult casted ? casted : error(_value)) : _error;

    public Result<TValue> WhereNot(Func<TValue, bool> predicate, Exception? error = null)
        => _hasValue ? (predicate(_value) ? error : this) : this;
    public Result<TValue> WhereNot(Func<TValue, bool> predicate, Func<TValue, Exception> error)
        => _hasValue ? (predicate(_value) ? error(_value) : this) : this;
}

partial struct Result<TValue, TError>
{
    public Result<TValue, TError> Where(Func<TValue, bool> predicate, TError error)
        => _hasValue ? (predicate(_value!) ? this : error) : this;
    public Result<TValue, TError> Where(Func<TValue, bool> predicate, Func<TValue, TError> error)
        => _hasValue ? (predicate(_value!) ? this : error(_value)) : this;

    public Result<TResult, TError> Where<TResult>(TError error)
        => _hasValue ? (_value is TResult casted ? casted : error) : _error;
    public Result<TResult, TError> Where<TResult>(Func<TValue, TError> error)
        => _hasValue ? (_value is TResult casted ? casted : error(_value)) : _error;

    public Result<TValue, TError> WhereNot(Func<TValue, bool> predicate, TError error)
        => _hasValue ? (predicate(_value!) ? error : this) : this;
    public Result<TValue, TError> WhereNot(Func<TValue, bool> predicate, Func<TValue, TError> error)
        => _hasValue ? (predicate(_value!) ? error(_value) : this) : this;
}

partial struct RefOption<TValue>
{
    public RefOption<TValue> Where(Func<TValue, bool> predicate)
        => _hasValue ? (predicate(_value!) ? this : default) : this;

    public RefOption<TValue> WhereNot(Func<TValue, bool> predicate)
        => _hasValue ? (predicate(_value!) ? default : this) : this;
}

public static class OptionWhereExtensions
{
    public static TValue? Where<TValue>(this TValue? value, Func<TValue, bool> predicate)
        where TValue : class
        => value is null ? null : (predicate(value) ? value : null);
    public static TValue? WhereNot<TValue>(this TValue? value, Func<TValue, bool> predicate)
        where TValue : class
        => value is null ? null : (predicate(value) ? null : value);

    public static TValue? Where<TValue>(this TValue? value, Func<TValue, bool> predicate)
        where TValue : struct
        => value.HasValue ? (predicate(value.Value) ? value : null) : null;
    public static TValue? WhereNot<TValue>(this TValue? value, Func<TValue, bool> predicate)
        where TValue : struct
        => value.HasValue ? (predicate(value.Value) ? null : value) : null;
}