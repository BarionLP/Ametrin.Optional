using System.IO;

namespace Ametrin.Optional;

public static class OptionExtensions
{
    public static Option<T> ToOption<T>(this T? value) where T : class => Option.Of(value);
    public static Option<T> ToOption<T>(this T? value) where T : struct => Option.Of(value);
    public static Option<T> ToOption<T>(this object? value) => value is T t ? t : default;

    public static Result<T> ToResult<T>(this T? value) where T : class => Result.Of(value);
    public static Result<T> ToResult<T>(this T? value) where T : struct => Result.Of(value);

    public static T? OrNull<T>(this Option<T> option) where T : class
        => option._hasValue ? option._value! : null;

    public static Option<T> WhereExists<T>(this T info) where T : FileSystemInfo
        => info.Exists ? info : default(Option<T>);

    public static Option<T> WhereExists<T>(this Option<T> option) where T : FileSystemInfo
        => option.Where(static info => info.Exists);

    public static Result<T> WhereExists<T>(this Result<T> result) where T : FileSystemInfo
        => result.Where(static info => info.Exists, static info => new FileNotFoundException(null, info.FullName));


    public static Option<T> Dispose<T>(this Option<T> option) where T : IDisposable
    {
        if (option._hasValue)
        {
            option._value.Dispose();
        }

        return default;
    }

    public static Option<T> ToOption<T>(this Result<T> result)
        => result._hasValue ? result._value : default(Option<T>);
    public static Result<T> ToResult<T>(this Option<T> option, Exception? error = null) 
        => option._hasValue ? option._value : error;
    public static Result<TValue, TError> ToResult<TValue, TError>(this Option<TValue> option, TError error) 
        => option._hasValue ? option._value : error;
    
    public static Result<T> ToResult<T>(this Option<T> option, Func<Exception> error) 
        => option._hasValue ? option._value : error();
    public static Result<TValue, TError> ToResult<TValue, TError>(this Option<TValue> option, Func<TError> error)
        => option._hasValue ? option._value : error();
}
