namespace Ametrin.Optional;

partial struct Option
{
    [AsyncExtension]
    public TResult Match<TResult>(Func<TResult> success, Func<TResult> error)
        where TResult : allows ref struct
        => _success ? success() : error();

    public TResult Match<TArg, TResult>(TArg arg, Func<TArg, TResult> success, Func<TArg, TResult> error)
        where TResult : allows ref struct
        where TArg : allows ref struct
        => _success ? success(arg) : error(arg);
}

partial struct Option<TValue>
{
    [AsyncExtension]
    public TResult Match<TResult>(Func<TValue, TResult> success, Func<TResult> error)
        where TResult : allows ref struct
        => _hasValue ? success(_value) : error();

    public TResult Match<TArg, TResult>(TArg arg, Func<TValue, TArg, TResult> success, Func<TArg, TResult> error)
        where TArg : allows ref struct
        where TResult : allows ref struct
        => _hasValue ? success(_value, arg) : error(arg);
}

partial struct Result<TValue>
{
    [AsyncExtension]
    public TResult Match<TResult>(Func<TValue, TResult> success, Func<Exception, TResult> error)
        where TResult : allows ref struct
        => _hasValue ? success(_value) : error(_error);

    public TResult Match<TArg, TResult>(TArg arg, Func<TValue, TArg, TResult> success, Func<Exception, TArg, TResult> error)
        where TArg : allows ref struct
        where TResult : allows ref struct
        => _hasValue ? success(_value, arg) : error(_error, arg);
}

partial struct Result<TValue, TError>
{
    [AsyncExtension]
    public TResult Match<TResult>(Func<TValue, TResult> success, Func<TError, TResult> error)
        where TResult : allows ref struct
        => _hasValue ? success(_value) : error(_error);

    public TResult Match<TArg, TResult>(TArg arg, Func<TValue, TArg, TResult> success, Func<TError, TArg, TResult> error)
        where TArg : allows ref struct
        where TResult : allows ref struct
        => _hasValue ? success(_value, arg) : error(_error, arg);
}

partial struct ErrorState
{
    [AsyncExtension]
    public TResult Match<TResult>(Func<TResult> success, Func<Exception, TResult> error)
        where TResult : allows ref struct
        => _isError ? error(_error) : success();

    public TResult Match<TArg, TResult>(TArg arg, Func<TArg, TResult> success, Func<Exception, TArg, TResult> error)
        where TArg : allows ref struct
        where TResult : allows ref struct
        => _isError ? error(_error, arg) : success(arg);
}

partial struct ErrorState<TError>
{
    [AsyncExtension]
    public TResult Match<TResult>(Func<TResult> success, Func<TError, TResult> error)
        where TResult : allows ref struct
        => _isError ? error(_error) : success();

    public TResult Match<TArg, TResult>(TArg arg, Func<TArg, TResult> success, Func<TError, TArg, TResult> error)
        where TArg : allows ref struct
        where TResult : allows ref struct
        => _isError ? error(_error, arg) : success(arg);
}

partial struct RefOption<TValue>
{
    public TResult Match<TResult>(Func<TValue, TResult> success, Func<TResult> error)
        where TResult : allows ref struct
        => _hasValue ? success(_value) : error();

    public TResult Match<TArg, TResult>(TArg arg, Func<TValue, TArg, TResult> success, Func<TArg, TResult> error)
        where TArg : allows ref struct
        where TResult : allows ref struct
        => _hasValue ? success(_value, arg) : error(arg);
}