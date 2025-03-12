namespace Ametrin.Optional;

partial struct Option
{
    public TResult Map<TResult>(Func<TResult> success, Func<TResult> error)
        => _success ? success() : error();
}

partial struct Option<TValue>
{
    public Option<TResult> Map<TResult>(Func<TValue, TResult> map)
        => _hasValue ? Option.Success(map(_value)) : default;
    public Option<TResult> Map<TResult>(Func<TValue, Option<TResult>> map)
        => _hasValue ? map(_value) : default;
}

partial struct Result<TValue>
{
    public Result<TResult> Map<TResult>(Func<TValue, TResult> map)
        => _hasValue ? map(_value) : _error;
    public Result<TResult, TNewError> Map<TResult, TNewError>(Func<TValue, TResult> map, Func<Exception, TNewError> errorMap)
        => _hasValue ? map(_value) : errorMap(_error);
    public Result<TResult> Map<TResult>(Func<TValue, Result<TResult>> map)
        => _hasValue ? map(_value) : _error;
}

partial struct Result<TValue, TError>
{
    public Result<TResult, TError> Map<TResult>(Func<TValue, TResult> map)
        => _hasValue ? map(_value) : _error;
    public Result<TResult> Map<TResult>(Func<TValue, TResult> map, Func<TError, Exception> errorMap)
        => _hasValue ? map(_value) : errorMap(_error);
    public Result<TResult, TNewError> Map<TResult, TNewError>(Func<TValue, TResult> map, Func<TError, TNewError> errorMap)
        => _hasValue ? map(_value) : errorMap(_error);
    public Result<TResult, TError> Map<TResult>(Func<TValue, Result<TResult, TError>> map)
        => _hasValue ? map(_value) : _error;
}

partial struct ErrorState
{
    public TResult Map<TResult>(Func<TResult> success, Func<Exception, TResult> error) => _isError ? error(_error) : success();
}

partial struct ErrorState<TError>
{
    public TResult Map<TResult>(Func<TResult> success, Func<TError, TResult> error) => _isError ? error(_error) : success();
}

partial struct RefOption<TValue>
{
    public RefOption<TResult> Map<TResult>(Func<TValue, TResult> map)
        where TResult : struct, allows ref struct
        => _hasValue ? RefOption.Success(map(_value)) : default;
    public RefOption<TResult> Map<TResult>(Func<TValue, RefOption<TResult>> map)
        where TResult : struct, allows ref struct
        => _hasValue ? map(_value) : default;
    public Option<TResult> Map<TResult>(Func<TValue, Option<TResult>> map)
        => _hasValue ? map(_value) : default;
}

public static class OptionMapExtensions
{
    public static Option<TResult> Map<TValue, TResult>(this RefOption<TValue> option, Func<TValue, TResult> map)
        where TValue : struct, allows ref struct
        where TResult : class
        => option._hasValue ? Option.Success(map(option._value)) : default;

    public static RefOption<TResult> Map<TValue, TResult>(this Option<TValue> option, Func<TValue, TResult> map)
        where TResult : struct, allows ref struct
        => option._hasValue ? RefOption.Success(map(option._value)) : default;
}
