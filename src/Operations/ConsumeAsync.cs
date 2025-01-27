using System.Threading.Tasks;

namespace Ametrin.Optional;

partial struct Option<TValue>
{
    public Task ConsumeAsync(Func<TValue, Task> success)
        => _hasValue ? success.Invoke(_value) : Task.CompletedTask;
    public Task ConsumeAsync(Func<TValue, Task> success, Func<Task> error)
        => _hasValue ? success.Invoke(_value) : error.Invoke();

    public Task ConsumeAsync(Func<TValue, Task> success, Action error)
    {
        if (_hasValue)
        {
            return success.Invoke(_value);
        }
        else
        {
            error.Invoke();
            return Task.CompletedTask;
        }
    }
    public Task ConsumeAsync(Action<TValue> success, Func<Task> error)
    {
        if (_hasValue)
        {
            success.Invoke(_value);
            return Task.CompletedTask;
        }
        else
        {
            return error.Invoke();
        }
    }
}

public static class ConsumeAsyncOptionExtensions
{
    // TODO: which one is better?
    public static Task ConsumeAsync<TValue>(this Task<Option<TValue>> optionTask, Func<TValue, Task> success)
        => optionTask.ContinueWith(task => task.Result.ConsumeAsync(success));
    public static async Task ConsumeAsync<TValue>(this Task<Option<TValue>> optionTask, Func<TValue, Task> success, Func<Task> error)
        => await (await optionTask).ConsumeAsync(success, error);
    public static async Task ConsumeAsync<TValue>(this Task<Option<TValue>> optionTask, Func<TValue, Task> success, Action error)
        => await (await optionTask).ConsumeAsync(success, error);
    public static async Task ConsumeAsync<TValue>(this Task<Option<TValue>> optionTask, Action<TValue> success, Func<Task> error)
        => await (await optionTask).ConsumeAsync(success, error);

    public static async Task ConsumeAsync<TValue>(this Option<Task<TValue>> optionTask, Func<TValue, Task> success, Func<Task> error)
    {
        if (optionTask._hasValue)
        {
            var value = await optionTask._value;
            await success.Invoke(value);
        }
        else
        {
            await error.Invoke();
        }

        await optionTask.ConsumeAsync(success, error);
    }
}