using System.Threading.Tasks;

namespace Ametrin.Optional;

partial struct Option<TValue>
{
    internal static readonly Task<Option<TValue>> ErrorTask = Task.FromResult(Option.Error<TValue>());
    public async Task<Option<TResult>> MapAsync<TResult>(Func<TValue, Task<TResult>> selector)
        => _hasValue ? Option.Success(await selector(_value)) : default;
    public Task<Option<TResult>> MapAsync<TResult>(Func<TValue, Task<Option<TResult>>> selector)
        => _hasValue ? selector(_value) : Option<TResult>.ErrorTask;
}

partial struct Result<TValue>
{
    public async Task<Result<TResult>> MapAsync<TResult>(Func<TValue, Task<TResult>> selector)
        => _hasValue ? await selector(_value) : _error;
    public Task<Result<TResult>> MapAsync<TResult>(Func<TValue, Task<Result<TResult>>> selector)
        => _hasValue ? selector(_value) : Task.FromResult(Result.Error<TResult>(_error));
}

partial struct Result<TValue, TError>
{
    public async Task<Result<TResult, TError>> MapAsync<TResult>(Func<TValue, Task<TResult>> selector)
        => _hasValue ? await selector(_value) : _error;
    public Task<Result<TResult, TError>> MapAsync<TResult>(Func<TValue, Task<Result<TResult, TError>>> selector)
        => _hasValue ? selector(_value) : Task.FromResult(Result.Error<TResult, TError>(_error));
}

public static class MapAsyncOptionExtensions
{
    public static async Task<Option<TResult>> Map<TValue, TResult>(this Task<Option<TValue>> optionTask, Func<TValue, TResult> selector)
        => (await optionTask).Map(selector); // ContinueWith is 42x slower and uses 2.7x memory (.NET 9)
    public static async Task<Option<TResult>> MapAsync<TValue, TResult>(this Task<Option<TValue>> optionTask, Func<TValue, Task<TResult>> selector)
        => await (await optionTask).MapAsync(selector);
    public static async Task<Option<TResult>> MapAsync<TValue, TResult>(this Task<Option<TValue>> optionTask, Func<TValue, Task<Option<TResult>>> selector)
        => await (await optionTask).MapAsync(selector);

    public static async Task<Result<TResult>> Select<TValue, TResult>(this Task<Result<TValue>> optionTask, Func<TValue, TResult> selector)
        => (await optionTask).Map(selector);
    public static async Task<Result<TResult>> MapAsync<TValue, TResult>(this Task<Result<TValue>> resultTask, Func<TValue, Task<TResult>> selector)
        => await (await resultTask).MapAsync(selector);
    public static async Task<Result<TResult>> MapAsync<TValue, TResult>(this Task<Result<TValue>> resultTask, Func<TValue, Task<Result<TResult>>> selector)
        => await (await resultTask).MapAsync(selector);
}