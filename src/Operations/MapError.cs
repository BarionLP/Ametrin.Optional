using System.Threading.Tasks;

namespace Ametrin.Optional;

partial struct Result<TValue>
{
    public Result<TValue, TNewError> MapError<TNewError>(Func<Exception, TNewError> errorMap)
        => _hasValue ? _value : errorMap(_error);
    public Result<TValue> MapError(Func<Exception, Exception> errorMap)
        => _hasValue ? _value : errorMap(_error);
}

partial struct Result<TValue, TError>
{
    public Result<TValue> MapError(Func<TError, Exception> errorMap)
        => _hasValue ? _value : errorMap(_error);
    public Result<TValue, TNewError> MapError<TNewError>(Func<TError, TNewError> errorMap)
        => _hasValue ? _value : errorMap(_error);
}

partial struct ErrorState
{
    public ErrorState<TNewError> MapError<TNewError>(Func<Exception, TNewError> errorMap)
        => _isError ? errorMap(_error) : default(ErrorState<TNewError>);
    public ErrorState MapError(Func<Exception, Exception> errorMap)
        => _isError ? errorMap(_error) : default(ErrorState);
}

partial struct ErrorState<TError>
{
    public ErrorState MapError(Func<TError, Exception> errorMap)
        => _isError ? errorMap(_error) : default(ErrorState);
    public ErrorState<TNewError> MapError<TNewError>(Func<TError, TNewError> errorMap)
        => _isError ? errorMap(_error) : default(ErrorState<TNewError>);
}

public static class OptionMapErrorAsyncExtensions
{
    public static async Task<Result<TValue, TNewError>> MapError<TValue, TNewError>(this Task<Result<TValue>> optionalTask, Func<Exception, TNewError> errorMap)
        => (await optionalTask).MapError(errorMap);

    public static async Task<Result<TValue>> MapError<TValue, TError, TNewError>(this Task<Result<TValue, TError>> optionalTask, Func<TError, Exception> errorMap)
        => (await optionalTask).MapError(errorMap);
    public static async Task<Result<TValue, TNewError>> MapError<TValue, TError, TNewError>(this Task<Result<TValue, TError>> optionalTask, Func<TError, TNewError> errorMap)
        => (await optionalTask).MapError(errorMap);

    public static async Task<ErrorState<TNewError>> MapError<TNewError>(this Task<ErrorState> optionalTask, Func<Exception, TNewError> errorMap)
        => (await optionalTask).MapError(errorMap);

    public static async Task<ErrorState> MapError<TError>(this Task<ErrorState<TError>> optionalTask, Func<TError, Exception> errorMap)
        => (await optionalTask).MapError(errorMap);
    public static async Task<ErrorState<TNewError>> MapError<TError, TNewError>(this Task<ErrorState<TError>> optionalTask, Func<TError, TNewError> errorMap)
        => (await optionalTask).MapError(errorMap);
}
