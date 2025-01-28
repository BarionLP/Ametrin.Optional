namespace Ametrin.Optional;

partial struct Option
{
    public TResult Select<TResult>(Func<TResult> success, Func<TResult> error) 
        => _success ? success() : error();
}

partial struct Option<TValue>
{
    public Option<TResult> Select<TResult>(Func<TValue, TResult> selector)
        => _hasValue ? Option.Success(selector(_value)) : default;
    public Result<TResult> Select<TResult>(Func<TValue, Result<TResult>> selector)
        => _hasValue ? selector(_value) : null;
    public Option<TResult> Select<TResult>(Func<TValue, Option<TResult>> selector)
        => _hasValue ? selector(_value) : default;
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
        => _hasValue ? selector(_value) : _error;
    public Result<TResult> Select<TResult>(Func<TValue, TResult> selector, Func<TError, Exception> errorSelector)
        => _hasValue ? selector(_value) : errorSelector(_error);
    public Result<TResult, TNewError> Select<TResult, TNewError>(Func<TValue, TResult> selector, Func<TError, TNewError> errorSelector)
        => _hasValue ? selector(_value) : errorSelector(_error);
    public Result<TResult, TError> Select<TResult>(Func<TValue, Result<TResult, TError>> selector)
        => _hasValue ? selector(_value) : _error;
}

partial struct ErrorState
{
    public TResult Select<TResult>(Func<TResult> success, Func<Exception, TResult> error) 
        => _isError ? error(_error) : success();
}

partial struct ErrorState<TError>
{
    public TResult Select<TResult>(Func<TResult> success, Func<TError, TResult> error) 
        => _isError ? error(_error) : success();
}