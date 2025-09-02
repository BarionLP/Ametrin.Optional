namespace Ametrin.Optional;

partial struct Option<TValue>
{
    public Option<TValue> Reject(Func<TValue, bool> predicate)
        => _hasValue ? (predicate(_value) ? default : this) : this;

    public Option<TValue> Reject<TArg>(TArg arg, Func<TValue, TArg, bool> predicate)
        where TArg : allows ref struct
        => _hasValue ? (predicate(_value, arg) ? default : this) : this;
}

partial struct Result<TValue>
{
    public Result<TValue> Reject(Func<TValue, bool> predicate, Exception? error = null)
        => _hasValue ? (predicate(_value) ? Result.Error<TValue>(error) : this) : this;
    public Result<TValue> Reject(Func<TValue, bool> predicate, Func<TValue, Exception> error)
        => _hasValue ? (predicate(_value) ? error(_value) : this) : this;

    public Result<TValue> Reject<TArg>(TArg arg, Func<TValue, TArg, bool> predicate, Exception? error = null)
        where TArg : allows ref struct
        => _hasValue ? (predicate(_value, arg) ? Result.Error<TValue>(error) : this) : this;
    public Result<TValue> Reject<TArg>(TArg arg, Func<TValue, TArg, bool> predicate, Func<TValue, TArg, Exception> error)
        where TArg : allows ref struct
        => _hasValue ? (predicate(_value, arg) ? error(_value, arg) : this) : this;
}

partial struct Result<TValue, TError>
{
    public Result<TValue, TError> Reject(Func<TValue, bool> predicate, TError error)
        => _hasValue ? (predicate(_value) ? error : this) : this;
    public Result<TValue, TError> Reject(Func<TValue, bool> predicate, Func<TValue, TError> error)
        => _hasValue ? (predicate(_value) ? error(_value) : this) : this;

    public Result<TValue, TError> Reject<TArg>(TArg arg, Func<TValue, TArg, bool> predicate, TError error)
        where TArg : allows ref struct
        => _hasValue ? (predicate(_value, arg) ? error : this) : this;
    public Result<TValue, TError> Reject<TArg>(TArg arg, Func<TValue, TArg, bool> predicate, Func<TValue, TArg, TError> error)
        where TArg : allows ref struct
        => _hasValue ? (predicate(_value, arg) ? error(_value, arg) : this) : this;
}

partial struct RefOption<TValue>
{
    public RefOption<TValue> Reject(Func<TValue, bool> predicate)
        => _hasValue ? (predicate(_value) ? default : this) : this;

    public RefOption<TValue> Reject<TArg>(TArg arg, Func<TValue, TArg, bool> predicate)
        where TArg : allows ref struct
        => _hasValue ? (predicate(_value, arg) ? default : this) : this;
}
