using System.Threading.Tasks;

namespace Ametrin.Optional;

public static class OptionDisposeAsyncExtensions
{
    public static ValueTask Dispose<TValue>(this Option<TValue> option) where TValue : IAsyncDisposable
    {
        return option._hasValue ? option._value.DisposeAsync() : ValueTask.CompletedTask;
    }

    public static ValueTask DisposeAsync<TValue>(this Result<TValue> option) where TValue : IAsyncDisposable
    {
        return option._hasValue ? option._value.DisposeAsync() : ValueTask.CompletedTask;
    }

    public static ValueTask DisposeAsync<TValue, TError>(this Result<TValue, TError> option) where TValue : IAsyncDisposable
    {
        return option._hasValue ? option._value.DisposeAsync() : ValueTask.CompletedTask;
    }

    public static ValueTask DisposeAsync<TError>(this ErrorState<TError> option) where TError : IAsyncDisposable
    {
        return option._isError ? option._error.DisposeAsync() : ValueTask.CompletedTask;
    }

    public static ValueTask DisposeAsync<TValue>(this RefOption<TValue> option)
        where TValue : struct, IAsyncDisposable, allows ref struct
    {
        return option._hasValue ? option._value.DisposeAsync() : ValueTask.CompletedTask;
    }
}
