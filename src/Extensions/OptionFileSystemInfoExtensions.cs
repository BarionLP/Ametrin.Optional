using System.IO;

namespace Ametrin.Optional;

public static class OptionFileSystemInfoExtensions
{
    public static Option<T> RequireExists<T>(this T info)
        where T : FileSystemInfo
        => info.Exists ? Option.Success(info) : default;

    public static Option<T> RequireExists<T>(this Option<T> option) 
        where T : FileSystemInfo
        => option.Require(static info => info.Exists);

    public static Result<T> RequireExists<T>(this Result<T> result)
        where T : FileSystemInfo
        => result.Require(static info => info.Exists, static info => new FileNotFoundException(null, info.FullName));

    public static Result<T> RequireExists<T>(this Result<T> result, Func<T, Exception> errorSupplier)
        where T : FileSystemInfo
        => result.Require(static info => info.Exists, errorSupplier);
        
    public static Result<TValue, TError> RequireExists<TValue, TError>(this Result<TValue, TError> result, Func<TValue, TError> errorSupplier)
        where TValue : FileSystemInfo
        => result.Require(static info => info.Exists, errorSupplier);
}
