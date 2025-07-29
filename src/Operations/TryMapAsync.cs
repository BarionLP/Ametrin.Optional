using System.Threading.Tasks;

namespace Ametrin.Optional;

// messured in .NET 9 on a Surface Pro X
// | Method                             | Mean        | Error     | StdDev    | Allocated |
// |----------------------------------- |------------:|----------:|----------:|----------:|
// | TryMapAsync_ConfigureAwait_Success |   103.10 ns |  1.584 ns |  1.323 ns |     248 B |
// | TryMapAsync_ConfigureAwait_Error   | 4,148.86 ns | 61.216 ns | 54.267 ns |     880 B |
// | TryMapAsync_TryCatch_Success       |    95.28 ns |  0.818 ns |  0.683 ns |     248 B |
// | TryMapAsync_TryCatch_Error         | 9,206.24 ns | 42.196 ns | 37.406 ns |    1080 B |
// the overhead of configure await in success path is neglectable compared to the error path  

partial struct Option<TValue>
{
    [AsyncExtension]
    public async Task<Option<TResult>> TryMapAsync<TResult>(Func<TValue, Task<TResult>> map)
    {
        if (!_hasValue)
        {
            return default;
        }

        var task = map(_value);

        await ((Task)task).ConfigureAwait(ConfigureAwaitOptions.SuppressThrowing);

        return task.IsCompletedSuccessfully ? Option.Success(task.Result) : default;
    }
}


partial struct Result<TValue>
{
    [AsyncExtension]
    public async Task<Result<TResult>> TryMapAsync<TResult>(Func<TValue, Task<TResult>> map)
    {
        if (!_hasValue)
        {
            return _error;
        }

        var task = map(_value);

        await ((Task)task).ConfigureAwait(ConfigureAwaitOptions.SuppressThrowing);

        return task.IsCompletedSuccessfully ? Result.Success(task.Result) : task.Exception is AggregateException { InnerExceptions.Count: 1 } ae ? ae.InnerException : task.Exception;
    }
}

partial struct Result<TValue, TError>
{
    [AsyncExtension]
    public async Task<Result<TResult, TError>> TryMapAsync<TResult>(Func<TValue, Task<TResult>> map, Func<Exception, TError> errorMap)
    {
        if (!_hasValue)
        {
            return _error;
        }

        var task = map(_value);

        await ((Task)task).ConfigureAwait(ConfigureAwaitOptions.SuppressThrowing);

        return task.IsCompletedSuccessfully ? task.Result : errorMap(task.Exception is AggregateException { InnerExceptions.Count: 1 } ae ? ae.InnerException! : task.Exception!);
    }
}
