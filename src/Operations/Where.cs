namespace Ametrin.Optional;

partial struct Option<T>
{
    public Option<T> Where(Func<T, bool> predicate)
        => _hasValue ? predicate(_value!) ? this : default : this;

    public Option<TResult> Where<TResult>()
        => _hasValue && _value is TResult casted ? casted : default(Option<TResult>);

    public Option<T> WhereNot(Func<T, bool> predicate)
        => _hasValue ? !predicate(_value!) ? this : default : this;
}

partial struct Result<TValue>
{
    public Result<TValue> Where(Func<TValue, bool> predicate, Exception? error = null)
        => _hasValue ? predicate(_value) ? this : error : this;
    public Result<TValue> Where(Func<TValue, bool> predicate, Func<TValue, Exception> error)
        => _hasValue ? predicate(_value) ? this : error(_value) : this;

    public Result<TResult> Where<TResult>(Exception? error = null)
        => _hasValue && _value is TResult casted ? casted : error ?? new InvalidCastException($"Cannot cast ${typeof(TValue).Name} to ${typeof(TResult).Name}");
    public Result<TResult> Where<TResult>(Func<TValue, Exception> error)
        => _hasValue && _value is TResult casted ? casted : error(_value);

    public Result<TValue> WhereNot(Func<TValue, bool> predicate, Exception? error = null)
        => _hasValue ? !predicate(_value) ? this : error : this;
    public Result<TValue> WhereNot(Func<TValue, bool> predicate, Func<TValue, Exception> error)
        => _hasValue ? !predicate(_value) ? this : error(_value) : this;
}

partial struct Result<TValue, TError>
{
    public Result<TValue, TError> Where(Func<TValue, bool> predicate, TError error)
        => _hasValue ? predicate(_value!) ? this : error : this;
    public Result<TValue, TError> Where(Func<TValue, bool> predicate, Func<TValue, TError> error)
        => _hasValue ? predicate(_value!) ? this : error(_value) : this;

    public Result<TResult, TError> Where<TResult>(TError error)
        => _hasValue && _value is TResult casted ? casted : error;
    public Result<TResult, TError> Where<TResult>(Func<TValue, TError> error)
        => _hasValue && _value is TResult casted ? casted : error(_value);

    public Result<TValue, TError> WhereNot(Func<TValue, bool> predicate, TError error)
        => _hasValue ? !predicate(_value!) ? this : error : this;
    public Result<TValue, TError> WhereNot(Func<TValue, bool> predicate, Func<TValue, TError> error)
        => _hasValue ? !predicate(_value!) ? this : error(_value) : this;
}