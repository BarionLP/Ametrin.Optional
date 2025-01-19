using System.Threading.Tasks;

namespace Ametrin.Optional;

partial struct Option<TValue>
{
    public async Task<Option<TResult>> SelectAsync<TResult>(Func<TValue, Task<TResult>> selector)
        => _hasValue ? Option.Success(await selector(_value)) : default;

    public Task<Option<TResult>> SelectAsync<TResult>(Func<TValue, Task<Option<TResult>>> selector)
        => _hasValue ? selector(_value) : Task.FromResult(Option.Error<TResult>());
}

partial struct Result<TValue>
{
    public async Task<Result<TResult>> SelectAsync<TResult>(Func<TValue, Task<TResult>> selector)
        => _hasValue ? await selector(_value) : _error;
    public Task<Result<TResult>> SelectAsync<TResult>(Func<TValue, Task<Result<TResult>>> selector)
        => _hasValue ? selector(_value) : Task.FromResult(Result.Error<TResult>(_error));

    //TODO: Better name
    public async Task<Result<TResult>> SelectAsyncSave<TResult>(Func<TValue, Task<TResult>> selector)
    {
        if (!_hasValue)
        {
            return _error;
        }

        var task = selector(_value);

        await ((Task)task).ConfigureAwait(ConfigureAwaitOptions.SuppressThrowing);

        return task.IsCompletedSuccessfully ? Result.Success(task.Result) : task.Exception;
    }
}