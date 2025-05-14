using System.Threading.Tasks;

namespace Ametrin.Optional;

partial struct Option<TValue>
{
    internal static readonly Task<Option<TValue>> ErrorTask = Task.FromResult(Option.Error<TValue>());
    public async Task<Option<TResult>> MapAsync<TResult>(Func<TValue, Task<TResult>> map)
        => _hasValue ? Option.Success(await map(_value)) : default;
    public Task<Option<TResult>> MapAsync<TResult>(Func<TValue, Task<Option<TResult>>> map)
        => _hasValue ? map(_value) : Option<TResult>.ErrorTask;
}

partial struct Result<TValue>
{
    public async Task<Result<TResult>> MapAsync<TResult>(Func<TValue, Task<TResult>> map)
        => _hasValue ? await map(_value) : _error;
    public Task<Result<TResult>> MapAsync<TResult>(Func<TValue, Task<Result<TResult>>> map)
        => _hasValue ? map(_value) : Task.FromResult(Result.Error<TResult>(_error));
    public async Task<Result<TResult, TError>> MapAsync<TResult, TError>(Func<TValue, Task<TResult>> map, Func<Exception, TError> errorMap)
        => _hasValue ? await map(_value) : errorMap(_error);
}

partial struct Result<TValue, TError>
{
    public async Task<Result<TResult, TError>> MapAsync<TResult>(Func<TValue, Task<TResult>> map)
        => _hasValue ? await map(_value) : _error;
    public Task<Result<TResult, TError>> MapAsync<TResult>(Func<TValue, Task<Result<TResult, TError>>> map)
        => _hasValue ? map(_value) : Task.FromResult(Result.Error<TResult, TError>(_error));
    [OverloadResolutionPriority(1)]
    public async Task<Result<TResult>> MapAsync<TResult>(Func<TValue, Task<TResult>> map, Func<TError, Exception> errorMap)
        => _hasValue ? await map(_value) : errorMap(_error);
    public async Task<Result<TResult, TNewError>> MapAsync<TResult, TNewError>(Func<TValue, Task<TResult>> map, Func<TError, TNewError> errorMap)
        => _hasValue ? await map(_value) : errorMap(_error);
}

public static class OptionMapAsyncExtensions
{
    public static async Task<Option<TResult>> MapAsync<TValue, TResult>(this Task<Option<TValue>> optionTask, Func<TValue, TResult> map)
        => (await optionTask).Map(map); // ContinueWith is 42x slower and uses 2.7x memory (.NET 9)
    [OverloadResolutionPriority(1)]
    public static async Task<Option<TResult>> MapAsync<TValue, TResult>(this Task<Option<TValue>> optionTask, Func<TValue, Option<TResult>> map)
        => (await optionTask).Map(map);
    public static async Task<Option<TResult>> MapAsync<TValue, TResult>(this Task<Option<TValue>> optionTask, Func<TValue, Task<TResult>> map)
        => await (await optionTask).MapAsync(map);
    public static async Task<Option<TResult>> MapAsync<TValue, TResult>(this Task<Option<TValue>> optionTask, Func<TValue, Task<Option<TResult>>> map)
        => await (await optionTask).MapAsync(map);

    public static async Task<Result<TResult>> MapAsync<TValue, TResult>(this Task<Result<TValue>> optionTask, Func<TValue, TResult> map)
        => (await optionTask).Map(map);
    public static async Task<Result<TResult>> MapAsync<TValue, TResult>(this Task<Result<TValue>> optionTask, Func<TValue, Result<TResult>> map)
        => (await optionTask).Map(map);

    [Obsolete("use MapAsync and MapError")]
    public static async Task<Result<TResult, TNewError>> MapAsync<TValue, TResult, TNewError>(this Task<Result<TValue>> optionTask, Func<TValue, TResult> map, Func<Exception, TNewError> errorMap)
        => (await optionTask).Map(map, errorMap);
    public static async Task<Result<TResult>> MapAsync<TValue, TResult>(this Task<Result<TValue>> resultTask, Func<TValue, Task<TResult>> map)
        => await (await resultTask).MapAsync(map);
    public static async Task<Result<TResult>> MapAsync<TValue, TResult>(this Task<Result<TValue>> resultTask, Func<TValue, Task<Result<TResult>>> map)
        => await (await resultTask).MapAsync(map);
    public static async Task<Result<TResult, TError>> MapAsync<TValue, TResult, TError>(this Task<Result<TValue>> resultTask, Func<TValue, Task<TResult>> map, Func<Exception, TError> errorMap)
        => await (await resultTask).MapAsync(map, errorMap);

    public static async Task<Result<TResult, TError>> MapAsync<TValue, TError, TResult>(this Task<Result<TValue, TError>> optionTask, Func<TValue, TResult> map)
        => (await optionTask).Map(map);
    
    [OverloadResolutionPriority(1)]
    public static async Task<Result<TResult, Exception>> MapAsync<TValue, TError, TResult>(this Task<Result<TValue, TError>> optionTask, Func<TValue, TResult> map, Func<TError, Exception> errorMap)
        => (await optionTask).Map(map, errorMap);

    [Obsolete("use MapAsync and MapError")]
    public static async Task<Result<TResult, TNewError>> MapAsync<TValue, TError, TResult, TNewError>(this Task<Result<TValue, TError>> optionTask, Func<TValue, TResult> map, Func<TError, TNewError> errorMap)
        => (await optionTask).Map(map, errorMap);
    public static async Task<Result<TResult, TError>> MapAsync<TValue, TError, TResult>(this Task<Result<TValue, TError>> resultTask, Func<TValue, Task<TResult>> map)
        => await (await resultTask).MapAsync(map);
    public static async Task<Result<TResult, TError>> MapAsync<TValue, TError, TResult>(this Task<Result<TValue, TError>> resultTask, Func<TValue, Task<Result<TResult, TError>>> map)
        => await (await resultTask).MapAsync(map);
    
    [OverloadResolutionPriority(1)]
    public static async Task<Result<TResult>> MapAsync<TValue, TError, TResult>(this Task<Result<TValue, TError>> resultTask, Func<TValue, Task<TResult>> map, Func<TError, Exception> errorMap)
        => await (await resultTask).MapAsync(map, errorMap);
    public static async Task<Result<TResult, TNewError>> MapAsync<TValue, TError, TResult, TNewError>(this Task<Result<TValue, TError>> resultTask, Func<TValue, Task<TResult>> map, Func<TError, TNewError> errorMap)
        => await (await resultTask).MapAsync(map, errorMap);
}