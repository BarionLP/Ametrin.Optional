namespace Ametrin.Optional;

partial struct Option
{
    [AsyncExtension]
    public TResult Match<TResult>(Func<TResult> success, Func<TResult> error) where TResult : allows ref struct
        => _success ? success() : error();
}

partial struct Option<TValue>
{
    [AsyncExtension]
    public TResult Match<TResult>(Func<TValue, TResult> success, Func<TResult> error) where TResult : allows ref struct
        => _hasValue ? success(_value) : error();
}

partial struct Result<TValue>
{
    [AsyncExtension]
    public TResult Match<TResult>(Func<TValue, TResult> success, Func<Exception, TResult> error) where TResult : allows ref struct
        => _hasValue ? success(_value) : error(_error);
}

partial struct Result<TValue, TError>
{
    [AsyncExtension]
    public TResult Match<TResult>(Func<TValue, TResult> success, Func<TError, TResult> error) where TResult : allows ref struct
        => _hasValue ? success(_value) : error(_error);
}

partial struct ErrorState
{
    [AsyncExtension]
    public TResult Match<TResult>(Func<TResult> success, Func<Exception, TResult> error) where TResult : allows ref struct
        => _isError ? error(_error) : success();
}

partial struct ErrorState<TError>
{
    [AsyncExtension]
    public TResult Match<TResult>(Func<TResult> success, Func<TError, TResult> error) where TResult : allows ref struct
        => _isError ? error(_error) : success();
}

partial struct RefOption<TValue>
{
    public TResult Match<TResult>(Func<TValue, TResult> success, Func<TResult> error) where TResult : allows ref struct
        => _hasValue ? success(_value) : error();
}