namespace Ametrin.Optional;

partial struct Option<TValue>
{
    public Option<TValue> Require(Func<TValue, bool> predicate)
        => _hasValue ? (predicate(_value!) ? this : default) : this;

    public Option<TValue> Require<TArg>(TArg arg, Func<TValue, TArg, bool> predicate)
        where TArg : allows ref struct
        => _hasValue ? (predicate(_value!, arg) ? this : default) : this;

    public Option<TResult> Require<TResult>()
        => _hasValue && _value is TResult casted ? casted : Option.Error<TResult>();
}

partial struct Result<TValue>
{
    public Result<TValue> Require(Func<TValue, bool> predicate, Exception? error = null)
        => _hasValue ? (predicate(_value) ? this : error) : this;
    public Result<TValue> Require(Func<TValue, bool> predicate, Func<TValue, Exception> error)
        => _hasValue ? (predicate(_value) ? this : error(_value)) : this;

    public Result<TValue> Require<TArg>(TArg arg, Func<TValue, TArg, bool> predicate, Exception? error = null)
        where TArg : allows ref struct
        => _hasValue ? (predicate(_value!, arg) ? this : error) : this;
    public Result<TValue> Require<TArg>(TArg arg, Func<TValue, TArg, bool> predicate, Func<TValue, TArg, Exception> error)
        where TArg : allows ref struct
        => _hasValue ? (predicate(_value!, arg) ? this : error(_value, arg)) : this;

    public Result<TResult> Require<TResult>(Exception? error = null)
        => _hasValue ? (_value is TResult casted ? casted : error ?? new InvalidCastException($"Cannot cast ${typeof(TValue).Name} to ${typeof(TResult).Name}")) : _error;
    public Result<TResult> Require<TResult>(Func<TValue, Exception> error)
        => _hasValue ? (_value is TResult casted ? casted : error(_value)) : _error;
}

partial struct Result<TValue, TError>
{
    public Result<TValue, TError> Require(Func<TValue, bool> predicate, TError error)
        => _hasValue ? (predicate(_value!) ? this : error) : this;
    public Result<TValue, TError> Require(Func<TValue, bool> predicate, Func<TValue, TError> error)
        => _hasValue ? (predicate(_value!) ? this : error(_value)) : this;

    public Result<TValue, TError> Require<TArg>(TArg arg, Func<TValue, TArg, bool> predicate, TError error)
        where TArg : allows ref struct
        => _hasValue ? (predicate(_value!, arg) ? this : error) : this;
    public Result<TValue, TError> Require<TArg>(TArg arg, Func<TValue, TArg, bool> predicate, Func<TValue, TArg, TError> error)
        where TArg : allows ref struct
        => _hasValue ? (predicate(_value!, arg) ? this : error(_value, arg)) : this;

    public Result<TResult, TError> Require<TResult>(TError error)
        => _hasValue ? (_value is TResult casted ? casted : error) : _error;
    public Result<TResult, TError> Require<TResult>(Func<TValue, TError> error)
        => _hasValue ? (_value is TResult casted ? casted : error(_value)) : _error;
}

partial struct RefOption<TValue>
{
    public RefOption<TValue> Require(Func<TValue, bool> predicate)
        => _hasValue ? (predicate(_value!) ? this : default) : this;

    public RefOption<TValue> Require<TArg>(TArg arg, Func<TValue, TArg, bool> predicate)
        where TArg : allows ref struct
        => _hasValue ? (predicate(_value!, arg) ? this : default) : this;
}
