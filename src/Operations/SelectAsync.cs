using System.Threading.Tasks;

namespace Ametrin.Optional;

partial struct Option<TValue>
{
    public async Task<Option<TResult>> SelectAsync<TResult>(Func<TValue, Task<TResult>> selector)
    {
        if (!_hasValue)
        {
            return default;
        }

        var task = selector(_value);

        await ((Task)task).ConfigureAwait(ConfigureAwaitOptions.SuppressThrowing);

        return task.IsCompletedSuccessfully ? Option.Success(task.Result) : default;
    }

    public Task<Option<TResult>> SelectAsync<TResult>(Func<TValue, Task<Option<TResult>>> selector)
        => _hasValue ? selector(_value) : Task.FromResult(Option.Error<TResult>());
}

partial struct Result<TValue>
{
    public async Task<Result<TResult>> SelectAsync<TResult>(Func<TValue, Task<TResult>> selector)
    {
        if (!_hasValue)
        {
            return _error;
        }

        var task = selector(_value);

        await ((Task)task).ConfigureAwait(ConfigureAwaitOptions.SuppressThrowing);

        return task.IsCompletedSuccessfully ? Result.Success(task.Result) : task.Exception;
    }

    public async Task<Result<TResult, TNewError>> SelectAsync<TResult, TNewError>(Func<TValue, Task<TResult>> selector, Func<Exception, TNewError> errorSelector)
    {
        if (!_hasValue)
        {
            return errorSelector(_error);
        }
        var task = selector(_value);

        await ((Task)task).ConfigureAwait(ConfigureAwaitOptions.SuppressThrowing);

        return task.IsCompletedSuccessfully ? task.Result : errorSelector(task.Exception!);
    }
    
    public Task<Result<TResult>> SelectAsync<TResult>(Func<TValue, Task<Result<TResult>>> selector)
        => _hasValue ? selector(_value) : Task.FromResult(Result.Error<TResult>(_error));
}