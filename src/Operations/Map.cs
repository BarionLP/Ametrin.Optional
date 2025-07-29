namespace Ametrin.Optional;

partial struct Option<TValue>
{
    [AsyncExtension]
    public Option<TResult> Map<TResult>(Func<TValue, TResult> map)
        => _hasValue ? Option.Success(map(_value)) : default;
    [AsyncExtension]
    public Option<TResult> Map<TResult>(Func<TValue, Option<TResult>> map)
        => _hasValue ? map(_value) : default;

    public Option<TResult> Map<TArg, TResult>(TArg arg, Func<TValue, TArg, TResult> map) where TArg : allows ref struct
        => _hasValue ? Option.Success(map(_value, arg)) : default;
    public Option<TResult> Map<TArg, TResult>(TArg arg, Func<TValue, TArg, Option<TResult>> map) where TArg : allows ref struct
        => _hasValue ? map(_value, arg) : default;
}

partial struct Result<TValue>
{
    [AsyncExtension]
    public Result<TResult> Map<TResult>(Func<TValue, TResult> map)
        => _hasValue ? map(_value) : _error;
    [AsyncExtension]
    public Result<TResult> Map<TResult>(Func<TValue, Result<TResult>> map)
        => _hasValue ? map(_value) : _error;

    public Result<TResult> Map<TArg, TResult>(TArg arg, Func<TValue, TArg, TResult> map) where TArg : allows ref struct
        => _hasValue ? map(_value, arg) : _error;
    public Result<TResult> Map<TArg, TResult>(TArg arg, Func<TValue, TArg, Result<TResult>> map) where TArg : allows ref struct
        => _hasValue ? map(_value, arg) : _error;
}

partial struct Result<TValue, TError>
{
    [AsyncExtension]
    public Result<TResult, TError> Map<TResult>(Func<TValue, TResult> map)
        => _hasValue ? map(_value) : _error;
    [AsyncExtension]
    public Result<TResult, TError> Map<TResult>(Func<TValue, Result<TResult, TError>> map)
        => _hasValue ? map(_value) : _error;

    public Result<TResult, TError> Map<TArg, TResult>(TArg arg, Func<TValue, TArg, TResult> map) where TArg : allows ref struct
        => _hasValue ? map(_value, arg) : _error;
    public Result<TResult, TError> Map<TArg, TResult>(TArg arg, Func<TValue, TArg, Result<TResult, TError>> map) where TArg : allows ref struct
        => _hasValue ? map(_value, arg) : _error;
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

    public RefOption<TResult> Map<TArg, TResult>(TArg arg, Func<TValue, TArg, TResult> map) where TArg : allows ref struct
        where TResult : struct, allows ref struct
        => _hasValue ? RefOption.Success(map(_value, arg)) : default;
    public RefOption<TResult> Map<TArg, TResult>(TArg arg, Func<TValue, TArg, RefOption<TResult>> map) where TArg : allows ref struct
        where TResult : struct, allows ref struct
        => _hasValue ? map(_value, arg) : default;
    public Option<TResult> Map<TArg, TResult>(TArg arg, Func<TValue, TArg, Option<TResult>> map) where TArg : allows ref struct
        => _hasValue ? map(_value, arg) : default;
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

    public static Option<TResult> Map<TValue, TArg, TResult>(this RefOption<TValue> option, TArg arg, Func<TValue, TArg, TResult> map) where TArg : allows ref struct
        where TValue : struct, allows ref struct
        where TResult : class
        => option._hasValue ? Option.Success(map(option._value, arg)) : default;

    public static RefOption<TResult> Map<TValue, TArg, TResult>(this Option<TValue> option, TArg arg, Func<TValue, TArg, TResult> map) where TArg : allows ref struct
        where TResult : struct, allows ref struct
        => option._hasValue ? RefOption.Success(map(option._value, arg)) : default;
}
