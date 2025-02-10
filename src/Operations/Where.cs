namespace Ametrin.Optional;

partial struct Option<TValue>
{
    [Obsolete("Use .Require")]
    public Option<TValue> Where(Func<TValue, bool> predicate)
        => _hasValue ? (predicate(_value!) ? this : default) : this;

    [Obsolete("Use .Require")]
    public Option<TResult> Where<TResult>()
        => _hasValue && _value is TResult casted ? casted : Option.Error<TResult>();

    [Obsolete("Use .Reject")]
    public Option<TValue> WhereNot(Func<TValue, bool> predicate)
        => _hasValue ? (predicate(_value!) ? default : this) : this;
}

partial struct Result<TValue>
{
    [Obsolete("Use .Require")]
    public Result<TValue> Where(Func<TValue, bool> predicate, Exception? error = null)
        => _hasValue ? (predicate(_value) ? this : error) : this;
    [Obsolete("Use .Require")]
    public Result<TValue> Where(Func<TValue, bool> predicate, Func<TValue, Exception> error)
        => _hasValue ? (predicate(_value) ? this : error(_value)) : this;

    [Obsolete("Use .Require")]
    public Result<TResult> Where<TResult>(Exception? error = null)
        => _hasValue ? (_value is TResult casted ? casted : error ?? new InvalidCastException($"Cannot cast ${typeof(TValue).Name} to ${typeof(TResult).Name}")) : _error;
    [Obsolete("Use .Require")]
    public Result<TResult> Where<TResult>(Func<TValue, Exception> error)
        => _hasValue ? (_value is TResult casted ? casted : error(_value)) : _error;

    [Obsolete("Use .Reject")]
    public Result<TValue> WhereNot(Func<TValue, bool> predicate, Exception? error = null)
        => _hasValue ? (predicate(_value) ? error : this) : this;
    [Obsolete("Use .Reject")]
    public Result<TValue> WhereNot(Func<TValue, bool> predicate, Func<TValue, Exception> error)
        => _hasValue ? (predicate(_value) ? error(_value) : this) : this;
}

partial struct Result<TValue, TError>
{
    [Obsolete("Use .Require")]
    public Result<TValue, TError> Where(Func<TValue, bool> predicate, TError error)
        => _hasValue ? (predicate(_value!) ? this : error) : this;
    [Obsolete("Use .Require")]
    public Result<TValue, TError> Where(Func<TValue, bool> predicate, Func<TValue, TError> error)
        => _hasValue ? (predicate(_value!) ? this : error(_value)) : this;

    [Obsolete("Use .Require")]
    public Result<TResult, TError> Where<TResult>(TError error)
        => _hasValue ? (_value is TResult casted ? casted : error) : _error;
    [Obsolete("Use .Require")]
    public Result<TResult, TError> Where<TResult>(Func<TValue, TError> error)
        => _hasValue ? (_value is TResult casted ? casted : error(_value)) : _error;

    [Obsolete("Use .Reject")]
    public Result<TValue, TError> WhereNot(Func<TValue, bool> predicate, TError error)
        => _hasValue ? (predicate(_value!) ? error : this) : this;
    [Obsolete("Use .Reject")]
    public Result<TValue, TError> WhereNot(Func<TValue, bool> predicate, Func<TValue, TError> error)
        => _hasValue ? (predicate(_value!) ? error(_value) : this) : this;
}

partial struct RefOption<TValue>
{
    [Obsolete("Use .Require")]
    public RefOption<TValue> Where(Func<TValue, bool> predicate)
        => _hasValue ? (predicate(_value!) ? this : default) : this;

    [Obsolete("Use .Reject")]
    public RefOption<TValue> WhereNot(Func<TValue, bool> predicate)
        => _hasValue ? (predicate(_value!) ? default : this) : this;
}

public static class OptionWhereExtensions
{
    [Obsolete("Use .Require")]
    public static TValue? Where<TValue>(this TValue? value, Func<TValue, bool> predicate)
        where TValue : class
        => value is null ? null : (predicate(value) ? value : null);
    [Obsolete("Use .Reject")]
    public static TValue? WhereNot<TValue>(this TValue? value, Func<TValue, bool> predicate)
        where TValue : class
        => value is null ? null : (predicate(value) ? null : value);

    [Obsolete("Use .Require")]
    public static TValue? Where<TValue>(this TValue? value, Func<TValue, bool> predicate)
        where TValue : struct
        => value.HasValue ? (predicate(value.Value) ? value : null) : null;
    [Obsolete("Use .Reject")]
    public static TValue? WhereNot<TValue>(this TValue? value, Func<TValue, bool> predicate)
        where TValue : struct
        => value.HasValue ? (predicate(value.Value) ? null : value) : null;
}