namespace Ametrin.Optional;

partial struct Option
{
    [Obsolete("use Map")]
    public TResult Select<TResult>(Func<TResult> success, Func<TResult> error) 
        => _success ? success() : error();
}

partial struct Option<TValue>
{
    [Obsolete("use Map")]
    public Option<TResult> Select<TResult>(Func<TValue, TResult> selector)
        => _hasValue ? Option.Success(selector(_value)) : default;
    [Obsolete("use .ToResult().Map")]
    public Result<TResult> Select<TResult>(Func<TValue, Result<TResult>> selector)
        => _hasValue ? selector(_value) : null;
    [Obsolete("use Map")]
    public Option<TResult> Select<TResult>(Func<TValue, Option<TResult>> selector)
        => _hasValue ? selector(_value) : default;
}

partial struct Result<TValue>
{
    [Obsolete("use Map")]
    public Result<TResult> Select<TResult>(Func<TValue, TResult> selector)
        => _hasValue ? selector(_value) : _error;
    [Obsolete("use Map")]
    public Result<TResult, TNewError> Select<TResult, TNewError>(Func<TValue, TResult> selector, Func<Exception, TNewError> errorSelector)
        => _hasValue ? selector(_value) : errorSelector(_error);
    [Obsolete("use Map")]
    public Result<TResult> Select<TResult>(Func<TValue, Result<TResult>> selector)
        => _hasValue ? selector(_value) : _error;
}

partial struct Result<TValue, TError>
{
    [Obsolete("use Map")]
    public Result<TResult, TError> Select<TResult>(Func<TValue, TResult> selector)
        => _hasValue ? selector(_value) : _error;
    [Obsolete("use Map")]
    public Result<TResult> Select<TResult>(Func<TValue, TResult> selector, Func<TError, Exception> errorSelector)
        => _hasValue ? selector(_value) : errorSelector(_error);
    [Obsolete("use Map")]
    public Result<TResult, TNewError> Select<TResult, TNewError>(Func<TValue, TResult> selector, Func<TError, TNewError> errorSelector)
        => _hasValue ? selector(_value) : errorSelector(_error);
    [Obsolete("use Map")]
    public Result<TResult, TError> Select<TResult>(Func<TValue, Result<TResult, TError>> selector)
        => _hasValue ? selector(_value) : _error;
}

partial struct ErrorState
{
    [Obsolete("use Map")]
    public TResult Select<TResult>(Func<TResult> success, Func<Exception, TResult> error) => _isError ? error(_error) : success();
}

partial struct ErrorState<TError>
{
    [Obsolete("use Map")]
    public TResult Select<TResult>(Func<TResult> success, Func<TError, TResult> error) => _isError ? error(_error) : success();
}

partial struct RefOption<TValue>
{
    [Obsolete("use Map")]
    public RefOption<TResult> Select<TResult>(Func<TValue, TResult> selector)
        where TResult : struct, allows ref struct
        => _hasValue ? RefOption.Success(selector(_value)) : default;
    [Obsolete("use Map")]
    public RefOption<TResult> Select<TResult>(Func<TValue, RefOption<TResult>> selector)
        where TResult : struct, allows ref struct
        => _hasValue ? selector(_value) : default;
    [Obsolete("use Map")]
    public Option<TResult> Select<TResult>(Func<TValue, Option<TResult>> selector)
        => _hasValue ? selector(_value) : default;
}

public static class SelectExtensions
{
    [Obsolete("use Map")]
    public static Option<TResult> Select<TValue, TResult>(this RefOption<TValue> option, Func<TValue, TResult> selector)
        where TValue : struct, allows ref struct
        where TResult : class
        => option._hasValue ? Option.Success(selector(option._value)) : default;

    [Obsolete("use Map")]
    public static RefOption<TResult> Select<TValue, TResult>(this Option<TValue> option, Func<TValue, TResult> selector)
        where TResult : struct, allows ref struct
        => option._hasValue ? RefOption.Success(selector(option._value)) : default;
}