using System.Threading.Tasks;

namespace Ametrin.Optional;

partial struct Option
{
    public static implicit operator bool(Option state) => state._success;
    public static implicit operator Option(bool state) => new(state);
}

partial struct Option<TValue>
{
    public static implicit operator Option<TValue>(TValue? value) => Option.Of(value);
    public static explicit operator Option(Option<TValue> option) => option._hasValue;

    [AsyncExtension]
    public Result<TValue> ToResult(Exception? error = null)
        => _hasValue ? _value : Result.Error<TValue>(error);

    [AsyncExtension]
    [OverloadResolutionPriority(1)] // so delegates returning subclasses of Exception also take this overload
    public Result<TValue> ToResult(Func<Exception> error)
        => _hasValue ? _value : error();
    public Result<TValue> ToResult<TArg>(TArg arg, Func<TArg, Exception> error)
        where TArg : allows ref struct
        => _hasValue ? _value : error(arg);

    [AsyncExtension]
    // [OverloadResolutionPriority(1)] // would prevent ToResult(Func) from being called because TError becomes Func<T>
    public Result<TValue, TError> ToResult<TError>(TError error)
        => _hasValue ? _value : error;
    [AsyncExtension]
    public Result<TValue, TError> ToResult<TError>(Func<TError> error)
        => _hasValue ? _value : error();
    public Result<TValue, TError> ToResult<TError, TArg>(TArg arg, Func<TArg, TError> error)
        where TArg : allows ref struct
        => _hasValue ? _value : error(arg);

}

partial struct Result<TValue>
{
    public static implicit operator Result<TValue>(TValue value) => Result.Success(value);
    public static implicit operator Result<TValue>(Exception error) => Result.Error<TValue>(error);
    public static implicit operator Result<TValue>(Result<TValue, Exception> other) => other._hasValue ? other._value : other._error;
    public static explicit operator Result<TValue, Exception>(Result<TValue> other) => other._hasValue ? other._value : other._error;
    public static explicit operator Option(Result<TValue> result) => result._hasValue;
    public static explicit operator Option<TValue>(Result<TValue> result) => result.ToOption();
    public static explicit operator ErrorState(Result<TValue> result) => result.ToErrorState();

    [AsyncExtension]
    public Option<TValue> ToOption()
        => _hasValue ? Option.Success(_value) : default;

    [AsyncExtension]
    public ErrorState ToErrorState()
        => _hasValue ? default : ErrorState.Error(_error);
}

partial struct Result<TValue, TError>
{
    public static implicit operator Result<TValue, TError>(TValue value) => Result.Success<TValue, TError>(value);
    public static implicit operator Result<TValue, TError>(TError error) => Result.Error<TValue, TError>(error);
    public static explicit operator Option(Result<TValue, TError> result) => result._hasValue;
    public static explicit operator ErrorState<TError>(Result<TValue, TError> result) => result.ToErrorState();
    public static explicit operator Option<TValue>(Result<TValue, TError> result) => result.ToOption();

    [AsyncExtension]
    public Option<TValue> ToOption()
        => _hasValue ? Option.Success(_value) : default;
    [AsyncExtension]
    public ErrorState<TError> ToErrorState()
        => _hasValue ? default : ErrorState.Error(_error);
}

partial struct ErrorState
{
    public static implicit operator ErrorState(Exception error) => error is null ? Success() : Error(error);
    public static explicit operator Option(ErrorState error) => !error._isError;

    [AsyncExtension]
    public Result<TValue> ToResult<TValue>(TValue value)
        => _isError ? _error : value;
    [AsyncExtension]
    public Result<TValue> ToResult<TValue>(Func<TValue> value)
        => _isError ? _error : value();
    public Result<TValue> ToResult<TValue, TArg>(TArg arg, Func<TArg, TValue> value)
        where TArg : allows ref struct
        => _isError ? _error : value(arg);
    [AsyncExtension]
    public async Task<Result<TValue>> ToResultAsync<TValue>(Func<Task<TValue>> value)
        => _isError ? _error : await value();
}

partial struct ErrorState<TError>
{
    public static implicit operator ErrorState<TError>(TError state) => state is TError t ? ErrorState.Error(t) : ErrorState.Success<TError>();
    public static explicit operator Option(ErrorState<TError> error) => !error._isError;

    [AsyncExtension]
    public Result<TValue, TError> ToResult<TValue>(TValue value)
        => _isError ? _error : value;
    [AsyncExtension]
    public Result<TValue, TError> ToResult<TValue>(Func<TValue> value)
        => _isError ? _error : value();
    public Result<TValue, TError> ToResult<TValue, TArg>(TArg arg, Func<TArg, TValue> value)
        where TArg : allows ref struct
        => _isError ? _error : value(arg);
    [AsyncExtension]
    public async Task<Result<TValue, TError>> ToResultAsync<TValue>(Func<Task<TValue>> value)
        => _isError ? _error : await value();
}

partial struct RefOption<TValue>
{
    public static implicit operator RefOption<TValue>(TValue value) => RefOption.Success(value);
    public static explicit operator Option(RefOption<TValue> option) => option._hasValue;
}