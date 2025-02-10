using System.Threading.Tasks;

namespace Ametrin.Optional;

partial struct Option<TValue>
{
    public Task ConsumeAsync(Func<TValue, Task>? success, Func<Task>? error)
        => _hasValue ? success?.Invoke(_value) ?? Task.CompletedTask : error?.Invoke() ?? Task.CompletedTask;

    public Task ConsumeAsync(Func<TValue, Task>? success, Action? error = null)
    {
        if (_hasValue)
        {
            return success?.Invoke(_value) ?? Task.CompletedTask;
        }
        else
        {
            error?.Invoke();
            return Task.CompletedTask;
        }
    }

    public Task ConsumeAsync(Action<TValue>? success, Func<Task> error)
    {
        if (_hasValue)
        {
            success?.Invoke(_value);
            return Task.CompletedTask;
        }
        else
        {
            return error.Invoke();
        }
    }
}

partial struct Result<TValue>
{
    public Task ConsumeAsync(Func<TValue, Task>? success, Func<Exception, Task>? error)
        => _hasValue ? success?.Invoke(_value) ?? Task.CompletedTask : error?.Invoke(_error) ?? Task.CompletedTask;

    public Task ConsumeAsync(Func<TValue, Task>? success, Action<Exception>? error = null)
    {
        if (_hasValue)
        {
            return success?.Invoke(_value) ?? Task.CompletedTask;
        }
        else
        {
            error?.Invoke(_error);
            return Task.CompletedTask;
        }
    }

    public Task ConsumeAsync(Action<TValue>? success, Func<Exception, Task> error)
    {
        if (_hasValue)
        {
            success?.Invoke(_value);
            return Task.CompletedTask;
        }
        else
        {
            return error.Invoke(_error);
        }
    }
}

partial struct Result<TValue, TError>
{
    public Task ConsumeAsync(Func<TValue, Task>? success, Func<TError, Task>? error)
        => _hasValue ? success?.Invoke(_value) ?? Task.CompletedTask : error?.Invoke(_error) ?? Task.CompletedTask;

    public Task ConsumeAsync(Func<TValue, Task>? success, Action<TError>? error = null)
    {
        if (_hasValue)
        {
            return success?.Invoke(_value) ?? Task.CompletedTask;
        }
        else
        {
            error?.Invoke(_error);
            return Task.CompletedTask;
        }
    }

    public Task ConsumeAsync(Action<TValue>? success, Func<TError, Task> error)
    {
        if (_hasValue)
        {
            success?.Invoke(_value);
            return Task.CompletedTask;
        }
        else
        {
            return error.Invoke(_error);
        }
    }
}

partial struct ErrorState
{
    public Task ConsumeAsync(Func<Task>? success, Func<Exception, Task>? error)
        => _isError ? error?.Invoke(_error) ?? Task.CompletedTask : success?.Invoke() ?? Task.CompletedTask;

    public Task ConsumeAsync(Func<Task>? success, Action<Exception>? error = null)
    {
        if (_isError)
        {
            error?.Invoke(_error);
            return Task.CompletedTask;
        }
        else
        {
            return success?.Invoke() ?? Task.CompletedTask;
        }
    }

    public Task ConsumeAsync(Action? success, Func<Exception, Task> error)
    {
        if (_isError)
        {
            return error.Invoke(_error);
        }
        else
        {
            success?.Invoke();
            return Task.CompletedTask;
        }
    }
}

partial struct ErrorState<TError>
{
    public Task ConsumeAsync(Func<Task>? success, Func<TError, Task>? error)
        => _isError ? error?.Invoke(_error) ?? Task.CompletedTask : success?.Invoke() ?? Task.CompletedTask;

    public Task ConsumeAsync(Func<Task>? success, Action<TError>? error = null)
    {
        if (_isError)
        {
            error?.Invoke(_error);
            return Task.CompletedTask;
        }
        else
        {
            return success?.Invoke() ?? Task.CompletedTask;
        }
    }

    public Task ConsumeAsync(Action? success, Func<TError, Task> error)
    {
        if (_isError)
        {
            return error.Invoke(_error);
        }
        else
        {
            success?.Invoke();
            return Task.CompletedTask;
        }
    }
}

public static class ConsumeAsyncOptionExtensions
{
    public static async Task Consume<TValue>(this Task<Option<TValue>> optionTask, Action<TValue>? success, Action? error)
        => (await optionTask).Consume(success, error);
    public static async Task ConsumeAsync<TValue>(this Task<Option<TValue>> optionTask, Func<TValue, Task>? success, Func<Task>? error)
        => await (await optionTask).ConsumeAsync(success, error);
    public static async Task ConsumeAsync<TValue>(this Task<Option<TValue>> optionTask, Func<TValue, Task>? success, Action? error)
        => await (await optionTask).ConsumeAsync(success, error);
    public static async Task ConsumeAsync<TValue>(this Task<Option<TValue>> optionTask, Action<TValue>? success, Func<Task> error)
        => await (await optionTask).ConsumeAsync(success, error);

    public static async Task Consume<TValue>(this Task<Result<TValue>> optionTask, Action<TValue>? success, Action<Exception>? error)
        => (await optionTask).Consume(success, error);
    public static async Task ConsumeAsync<TValue>(this Task<Result<TValue>> optionTask, Func<TValue, Task>? success, Func<Exception, Task>? error)
        => await (await optionTask).ConsumeAsync(success, error);
    public static async Task ConsumeAsync<TValue>(this Task<Result<TValue>> optionTask, Func<TValue, Task>? success, Action<Exception>? error)
        => await (await optionTask).ConsumeAsync(success, error);
    public static async Task ConsumeAsync<TValue>(this Task<Result<TValue>> optionTask, Action<TValue>? success, Func<Exception, Task> error)
        => await (await optionTask).ConsumeAsync(success, error);

    public static async Task Consume<TValue, TError>(this Task<Result<TValue, TError>> optionTask, Action<TValue>? success, Action<TError>? error)
        => (await optionTask).Consume(success, error);
    public static async Task ConsumeAsync<TValue, TError>(this Task<Result<TValue, TError>> optionTask, Func<TValue, Task>? success, Func<TError, Task>? error)
        => await (await optionTask).ConsumeAsync(success, error);
    public static async Task ConsumeAsync<TValue, TError>(this Task<Result<TValue, TError>> optionTask, Func<TValue, Task>? success, Action<TError>? error)
        => await (await optionTask).ConsumeAsync(success, error);
    public static async Task ConsumeAsync<TValue, TError>(this Task<Result<TValue, TError>> optionTask, Action<TValue>? success, Func<TError, Task> error)
        => await (await optionTask).ConsumeAsync(success, error);
}