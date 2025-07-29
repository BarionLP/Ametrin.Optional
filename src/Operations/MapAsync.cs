using System.Threading.Tasks;

namespace Ametrin.Optional;

partial struct Option<TValue>
{
    internal static readonly Task<Option<TValue>> ErrorTask = Task.FromResult(Option.Error<TValue>());
    [AsyncExtension]
    public async Task<Option<TResult>> MapAsync<TResult>(Func<TValue, Task<TResult>> map)
        => _hasValue ? Option.Success(await map(_value)) : default;
    [AsyncExtension]
    public Task<Option<TResult>> MapAsync<TResult>(Func<TValue, Task<Option<TResult>>> map)
        => _hasValue ? map(_value) : Option<TResult>.ErrorTask;
}

partial struct Result<TValue>
{
    [AsyncExtension]
    public async Task<Result<TResult>> MapAsync<TResult>(Func<TValue, Task<TResult>> map)
        => _hasValue ? await map(_value) : _error;
    [AsyncExtension]
    public Task<Result<TResult>> MapAsync<TResult>(Func<TValue, Task<Result<TResult>>> map)
        => _hasValue ? map(_value) : Task.FromResult(Result.Error<TResult>(_error));
}

partial struct Result<TValue, TError>
{
    [AsyncExtension]
    public async Task<Result<TResult, TError>> MapAsync<TResult>(Func<TValue, Task<TResult>> map)
        => _hasValue ? await map(_value) : _error;
    [AsyncExtension]
    public Task<Result<TResult, TError>> MapAsync<TResult>(Func<TValue, Task<Result<TResult, TError>>> map)
        => _hasValue ? map(_value) : Task.FromResult(Result.Error<TResult, TError>(_error));
}
