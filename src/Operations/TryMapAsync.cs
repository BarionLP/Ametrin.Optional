
using System.Threading.Tasks;

namespace Ametrin.Optional;

partial struct Result<TValue>
{
    public async Task<Result<TResult>> TryMapAsync<TResult>(Func<TValue, Task<TResult>> selector)
    {
        if (!_hasValue)
        {
            return _error;
        }

        var task = selector(_value);

        await ((Task)task).ConfigureAwait(ConfigureAwaitOptions.SuppressThrowing);

        return task.IsCompletedSuccessfully ? Result.Success(task.Result) : task.Exception;
    }

    public async Task<Result<TResult>> TryMapAsync<TResult>(Func<TValue, Task<Result<TResult>>> selector)
    {
        if (!_hasValue)
        {
            return _error;
        }

        var task = selector(_value);

        await ((Task)task).ConfigureAwait(ConfigureAwaitOptions.SuppressThrowing);

        return task.IsCompletedSuccessfully ? task.Result : task.Exception;
    }
}