using System.Diagnostics;
using System.Threading.Tasks;

namespace Ametrin.Optional;

public static class OptionOrAsyncExtensions
{
    // Option<T>
    [OverloadResolutionPriority(1)] // to allow 'Or(default)' which would normally be ambigious
    public static async Task<TValue> OrAsync<TValue>(this Task<Option<TValue>> task, TValue other) => (await task).Or(other);
    public static async Task<TValue> OrAsync<TValue>(this Task<Option<TValue>> task, Func<TValue> factory) => (await task).Or(factory);
    [StackTraceHidden]
    public static async Task<TValue> OrThrowAsync<TValue>(this Task<Option<TValue>> task) => (await task).OrThrow();
    [StackTraceHidden]
    public static async Task<TValue> OrThrowAsync<TValue>(this Task<Option<TValue>> task, string message) => (await task).OrThrow(message);
    [StackTraceHidden]
    public static async Task<TValue> OrThrowAsync<TValue>(this Task<Option<TValue>> task, Func<string> messageSupplier) => (await task).OrThrow(messageSupplier);

    // Result<T>
    [OverloadResolutionPriority(1)]
    public static async Task<TValue> OrAsync<TValue>(this Task<Result<TValue>> task, TValue other) => (await task).Or(other);
    public static async Task<TValue> OrAsync<TValue>(this Task<Result<TValue>> task, Func<Exception, TValue> factory) => (await task).Or(factory);
    [StackTraceHidden]
    public static async Task<TValue> OrThrowAsync<TValue>(this Task<Result<TValue>> task) => (await task).OrThrow();
    [StackTraceHidden]
    public static async Task<TValue> OrThrowAsync<TValue>(this Task<Result<TValue>> task, string message) => (await task).OrThrow(message);
    [StackTraceHidden]
    public static async Task<TValue> OrThrowAsync<TValue>(this Task<Result<TValue>> task, Func<Exception, string> messageSupplier) => (await task).OrThrow(messageSupplier);

    // Result<T, E>
    [OverloadResolutionPriority(1)]
    public static async Task<TValue> OrAsync<TValue, TError>(this Task<Result<TValue, TError>> task, TValue other) => (await task).Or(other);
    public static async Task<TValue> OrAsync<TValue, TError>(this Task<Result<TValue, TError>> task, Func<TError, TValue> factory) => (await task).Or(factory);
    [StackTraceHidden]
    public static async Task<TValue> OrThrowAsync<TValue, TError>(this Task<Result<TValue, TError>> task) => (await task).OrThrow();
    [StackTraceHidden]
    public static async Task<TValue> OrThrowAsync<TValue, TError>(this Task<Result<TValue, TError>> task, string message) => (await task).OrThrow(message);
    [StackTraceHidden]
    public static async Task<TValue> OrThrowAsync<TValue, TError>(this Task<Result<TValue, TError>> task, Func<TError, string> messageSupplier) => (await task).OrThrow(messageSupplier);
}
