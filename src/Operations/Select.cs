namespace Ametrin.Optional;

partial struct Option<T>
{
    public Option<TResult> Select<TResult>(Func<T, TResult> selector)
        => _hasValue ? selector(_value!) : default(Option<TResult>);
    public Result<TResult> Select<TResult>(Func<T, Result<TResult>> selector)
        => _hasValue ? selector(_value!) : null;
    public Option<TResult> Select<TResult>(Func<T, Option<TResult>> selector)
        => _hasValue ? selector(_value!) : default;
}

partial struct Result<TValue>
{
    public Result<TResult> Select<TResult>(Func<TValue, TResult> selector)
        => _hasValue ? selector(_value) : _error;
    public Result<TResult, TNewError> Select<TResult, TNewError>(Func<TValue, TResult> selector, Func<Exception, TNewError> errorSelector)
        => _hasValue ? selector(_value) : errorSelector(_error);
    public Result<TResult> Select<TResult>(Func<TValue, Result<TResult>> selector)
        => _hasValue ? selector(_value) : _error;
}

partial struct Result<TValue, TError>
{
    public Result<TResult, TError> Select<TResult>(Func<TValue, TResult> selector)
        => _hasValue ? selector(_value!) : _error;
    public Result<TResult> Select<TResult>(Func<TValue, TResult> selector, Func<TError, Exception> errorSelector)
        => _hasValue ? selector(_value!) : errorSelector(_error);
    public Result<TResult, TNewError> Select<TResult, TNewError>(Func<TValue, TResult> selector, Func<TError, TNewError> errorSelector)
        => _hasValue ? selector(_value!) : errorSelector(_error);
    public Result<TResult, TError> Select<TResult>(Func<TValue, Result<TResult, TError>> selector)
        => _hasValue ? selector(_value!) : _error;
}