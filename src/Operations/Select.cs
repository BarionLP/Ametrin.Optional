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
    [Obsolete("use .ToResult().Select")]
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

partial struct RefOption<TValue>
{
    public RefOption<TResult> Select<TResult>(Func<TValue, TResult> selector)
        where TResult : struct, allows ref struct
        => _hasValue ? RefOption.Success(selector(_value)) : default;
    public RefOption<TResult> Select<TResult>(Func<TValue, RefOption<TResult>> selector)
        where TResult : struct, allows ref struct
        => _hasValue ? selector(_value) : default;
    public Option<TResult> Select<TResult>(Func<TValue, Option<TResult>> selector)
        => _hasValue ? selector(_value) : default;
}

public static class SelectExtensions
{
    public static Option<TResult> Select<TValue, TResult>(this RefOption<TValue> option, Func<TValue, TResult> selector)
        where TValue : struct, allows ref struct
        where TResult : class
        => option._hasValue ? Option.Success(selector(option._value)) : default;

    public static RefOption<TResult> Select<TValue, TResult>(this Option<TValue> option, Func<TValue, TResult> selector)
        where TResult : struct, allows ref struct
        => option._hasValue ? RefOption.Success(selector(option._value)) : default;
}
