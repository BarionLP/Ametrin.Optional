using System.Threading.Tasks;

namespace Ametrin.Optional;

partial struct Option
{
    [AsyncExtension]
    public Task<TResult> MatchAsync<TResult>(Func<Task<TResult>> success, Func<Task<TResult>> error)
        => _success ? success() : error();

    [AsyncExtension]
    public Task<TResult> MatchAsync<TResult>(Func<Task<TResult>> success, Func<TResult> error)
        => _success ? success() : Task.FromResult(error());
}

partial struct Option<TValue>
{
    [AsyncExtension]
    public Task<TResult> MatchAsync<TResult>(Func<TValue, Task<TResult>> success, Func<Task<TResult>> error)
        => _hasValue ? success(_value) : error();

    [AsyncExtension]
    public Task<TResult> MatchAsync<TResult>(Func<TValue, Task<TResult>> success, Func<TResult> error)
        => _hasValue ? success(_value) : Task.FromResult(error());
}

partial struct Result<TValue>
{
    [AsyncExtension]
    public Task<TResult> MatchAsync<TResult>(Func<TValue, Task<TResult>> success, Func<Exception, Task<TResult>> error)
        => _hasValue ? success(_value) : error(_error);

    [AsyncExtension]
    public Task<TResult> MatchAsync<TResult>(Func<TValue, Task<TResult>> success, Func<Exception, TResult> error)
        => _hasValue ? success(_value) : Task.FromResult(error(_error));
}

partial struct Result<TValue, TError>
{
    [AsyncExtension]
    public Task<TResult> MatchAsync<TResult>(Func<TValue, Task<TResult>> success, Func<TError, Task<TResult>> error)
        => _hasValue ? success(_value) : error(_error);

    [AsyncExtension]
    public Task<TResult> MatchAsync<TResult>(Func<TValue, Task<TResult>> success, Func<TError, TResult> error)
        => _hasValue ? success(_value) : Task.FromResult(error(_error));
}

partial struct ErrorState
{
    [AsyncExtension]
    public Task<TResult> MatchAsync<TResult>(Func<Task<TResult>> success, Func<Exception, Task<TResult>> error)
        => _isError ? error(_error) : success();

    [AsyncExtension]
    public Task<TResult> MatchAsync<TResult>(Func<Task<TResult>> success, Func<Exception, TResult> error)
        => _isError ? Task.FromResult(error(_error)) : success();
}

partial struct ErrorState<TError>
{
    [AsyncExtension]
    public Task<TResult> MatchAsync<TResult>(Func<Task<TResult>> success, Func<TError, Task<TResult>> error)
        => _isError ? error(_error) : success();

    [AsyncExtension]
    public Task<TResult> MatchAsync<TResult>(Func<Task<TResult>> success, Func<TError, TResult> error)
        => _isError ? Task.FromResult(error(_error)) : success();
}
