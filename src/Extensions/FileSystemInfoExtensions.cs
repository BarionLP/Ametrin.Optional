using System.IO;

namespace Ametrin.Optional;

public static class FileSystemInfoExtensions
{
    public static Option<T> WhereExists<T>(this T info) where T : FileSystemInfo
        => info.Exists ? info : default(Option<T>);

    public static Option<T> WhereExists<T>(this Option<T> option) where T : FileSystemInfo
        => option.Where(static info => info.Exists);

    public static Result<T> WhereExists<T>(this Result<T> result) where T : FileSystemInfo
        => result.Where(static info => info.Exists, static info => new FileNotFoundException(null, info.FullName));
}
