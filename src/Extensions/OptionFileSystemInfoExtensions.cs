using System.IO;

namespace Ametrin.Optional;

public static class OptionFileSystemInfoExtensions
{
    public static Option<T> WhereExists<T>(this T info) where T : FileSystemInfo
        => info.Exists ? Option.Success(info) : default;

    public static Option<T> WhereExists<T>(this Option<T> option) where T : FileSystemInfo
        => option.Where(static info => info.Exists);

    public static Result<T> WhereExists<T>(this Result<T> result) where T : FileSystemInfo
        => result.Where(static info => info.Exists, static info => new FileNotFoundException(null, info.FullName));
}
