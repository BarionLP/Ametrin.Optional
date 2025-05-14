using System.Threading.Tasks;

namespace Ametrin.Optional;

partial struct Option
{
    public Task<TResult> MatchAsync<TResult>(Func<Task<TResult>> success, Func<Task<TResult>> error)
        => _success ? success() : error();

    public Task<TResult> MatchAsync<TResult>(Func<Task<TResult>> success, Func<TResult> error)
        => _success ? success() : Task.FromResult(error());
}

partial struct Option<TValue>
{
    public Task<TResult> MatchAsync<TResult>(Func<TValue, Task<TResult>> success, Func<Task<TResult>> error)
        => _hasValue ? success(_value) : error();

    public Task<TResult> MatchAsync<TResult>(Func<TValue, Task<TResult>> success, Func<TResult> error)
        => _hasValue ? success(_value) : Task.FromResult(error());
}

partial struct Result<TValue>
{
    public Task<TResult> MatchAsync<TResult>(Func<TValue, Task<TResult>> success, Func<Exception, Task<TResult>> error)
        => _hasValue ? success(_value) : error(_error);

    public Task<TResult> MatchAsync<TResult>(Func<TValue, Task<TResult>> success, Func<Exception, TResult> error)
        => _hasValue ? success(_value) : Task.FromResult(error(_error));
}

partial struct Result<TValue, TError>
{
    public Task<TResult> MatchAsync<TResult>(Func<TValue, Task<TResult>> success, Func<TError, Task<TResult>> error)
        => _hasValue ? success(_value) : error(_error);

    public Task<TResult> MatchAsync<TResult>(Func<TValue, Task<TResult>> success, Func<TError, TResult> error)
        => _hasValue ? success(_value) : Task.FromResult(error(_error));
}

partial struct ErrorState
{
    public Task<TResult> MatchAsync<TResult>(Func<Task<TResult>> success, Func<Exception, Task<TResult>> error)
        => _isError ? error(_error) : success();

    public Task<TResult> MatchAsync<TResult>(Func<Task<TResult>> success, Func<Exception, TResult> error)
        => _isError ? Task.FromResult(error(_error)) : success();
}

partial struct ErrorState<TError>
{
    public Task<TResult> MatchAsync<TResult>(Func<Task<TResult>> success, Func<TError, Task<TResult>> error)
        => _isError ? error(_error) : success();

    public Task<TResult> MatchAsync<TResult>(Func<Task<TResult>> success, Func<TError, TResult> error)
        => _isError ? Task.FromResult(error(_error)) : success();
}

partial struct RefOption<TValue>
{
    public Task<TResult> MatchAsync<TResult>(Func<TValue, Task<TResult>> success, Func<Task<TResult>> error)
        => _hasValue ? success(_value) : error();

    public Task<TResult> MatchAsync<TResult>(Func<TValue, Task<TResult>> success, Func<TResult> error)
        => _hasValue ? success(_value) : Task.FromResult(error());
}

public static class OptionMatchAsyncExtensions
{
    public static async Task<TResult> MatchAsync<TResult>(this Task<Option> optionalTask, Func<Task<TResult>> success, Func<Task<TResult>> error)
        => await (await optionalTask).MatchAsync(success, error);
    public static async Task<TResult> MatchAsync<TResult>(this Task<Option> optionalTask, Func<Task<TResult>> success, Func<TResult> error)
        => await (await optionalTask).MatchAsync(success, error);

    public static async Task<TResult> MatchAsync<TValue, TResult>(this Task<Option<TValue>> optionalTask, Func<TValue, Task<TResult>> success, Func<Task<TResult>> error)
        => await (await optionalTask).MatchAsync(success, error);
    public static async Task<TResult> MatchAsync<TValue, TResult>(this Task<Option<TValue>> optionalTask, Func<TValue, Task<TResult>> success, Func<TResult> error)
        => await (await optionalTask).MatchAsync(success, error);

    public static async Task<TResult> MatchAsync<TValue, TResult>(this Task<Result<TValue>> optionalTask, Func<TValue, Task<TResult>> success, Func<Exception, Task<TResult>> error)
        => await (await optionalTask).MatchAsync(success, error);
    public static async Task<TResult> MatchAsync<TValue, TResult>(this Task<Result<TValue>> optionalTask, Func<TValue, Task<TResult>> success, Func<Exception, TResult> error)
        => await (await optionalTask).MatchAsync(success, error);

    public static async Task<TResult> MatchAsync<TValue, TError, TResult>(this Task<Result<TValue, TError>> optionalTask, Func<TValue, Task<TResult>> success, Func<TError, Task<TResult>> error)
        => await (await optionalTask).MatchAsync(success, error);
    public static async Task<TResult> MatchAsync<TValue, TError, TResult>(this Task<Result<TValue, TError>> optionalTask, Func<TValue, Task<TResult>> success, Func<TError, TResult> error)
        => await (await optionalTask).MatchAsync(success, error);

    public static async Task<TResult> MatchAsync<TResult>(this Task<ErrorState> optionalTask, Func<Task<TResult>> success, Func<Exception, Task<TResult>> error)
        => await (await optionalTask).MatchAsync(success, error);
    public static async Task<TResult> MatchAsync<TResult>(this Task<ErrorState> optionalTask, Func<Task<TResult>> success, Func<Exception, TResult> error)
        => await (await optionalTask).MatchAsync(success, error);

    public static async Task<TResult> MatchAsync<TError, TResult>(this Task<ErrorState<TError>> optionalTask, Func<Task<TResult>> success, Func<TError, Task<TResult>> error)
        => await (await optionalTask).MatchAsync(success, error);
    public static async Task<TResult> MatchAsync<TError, TResult>(this Task<ErrorState<TError>> optionalTask, Func<Task<TResult>> success, Func<TError, TResult> error)
        => await (await optionalTask).MatchAsync(success, error);
}