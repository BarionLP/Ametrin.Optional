namespace Ametrin.Optional;

partial struct Option<TValue>
{
    public Option<TValue> Reject(Func<TValue, bool> predicate)
        => _hasValue ? (predicate(_value) ? default : this) : this;
}

partial struct Result<TValue>
{
    public Result<TValue> Reject(Func<TValue, bool> predicate, Exception? error = null)
        => _hasValue ? (predicate(_value) ? error : this) : this;
    public Result<TValue> Reject(Func<TValue, bool> predicate, Func<TValue, Exception> error)
        => _hasValue ? (predicate(_value) ? error(_value) : this) : this;
}

partial struct Result<TValue, TError>
{
    public Result<TValue, TError> Reject(Func<TValue, bool> predicate, TError error)
        => _hasValue ? (predicate(_value) ? error : this) : this;
    public Result<TValue, TError> Reject(Func<TValue, bool> predicate, Func<TValue, TError> error)
        => _hasValue ? (predicate(_value) ? error(_value) : this) : this;
}

partial struct RefOption<TValue>
{
    public RefOption<TValue> Reject(Func<TValue, bool> predicate)
        => _hasValue ? (predicate(_value) ? default : this) : this;
}
