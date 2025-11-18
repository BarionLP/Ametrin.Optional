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

    public static Result<FileInfo> RequireExists(this Result<FileInfo> result)
        => result.Require(static info => info.Exists, static info => new FileNotFoundException($"The system cannot find the file '{info.FullName}'.", info.FullName));

    public static Result<DirectoryInfo> RequireExists(this Result<DirectoryInfo> result)
        => result.Require(static info => info.Exists, static info => new DirectoryNotFoundException($"The system cannot find the directory '{info.FullName}'."));
    
    public static Result<FileSystemInfo> RequireExists(this Result<FileSystemInfo> result)
        => result.Require(static info => info.Exists, static info => new FileNotFoundException($"The system cannot find the path '{info.FullName}'.", info.FullName));

    public static Result<T> RequireExists<T>(this Result<T> result, Func<T, Exception> errorSupplier)
        where T : FileSystemInfo
        => result.Require(static info => info.Exists, errorSupplier);

    public static Result<TValue, TError> RequireExists<TValue, TError>(this Result<TValue, TError> result, Func<TValue, TError> errorSupplier)
        where TValue : FileSystemInfo
        => result.Require(static info => info.Exists, errorSupplier);
}
