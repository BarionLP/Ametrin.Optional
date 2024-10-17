namespace Ametrin.Optional;

public static class OptionExtensions
{
    public static Option<T> ToOption<T>(this T? value) where T : class => Option.Of(value);
    public static Option<T> ToOption<T>(this T? value) where T : struct => Option.Of(value);
    public static Option<T> ToOption<T>(this object? value) => value is T t ? t : default;

    public static T? OrNull<T>(this Option<T> option) where T : class
        => option._hasValue ? option._value! : null;
    public static T OrDefault<T>(this Option<T> option) where T : struct
        => option._hasValue ? option._value! : default;

    public static Option<string> WhereNotEmpty(this Option<string> option)
        => option.WhereNot(string.IsNullOrEmpty);

    public static Option<string> WhereNotWhiteSpace(this Option<string> option)
        => option.WhereNot(string.IsNullOrWhiteSpace);

    public static Option<T> WhereExists<T>(this Option<T> option) where T : FileSystemInfo
        => option.Where(static info => info.Exists);
    public static Option<T> WhereExists<T>(this T info) where T : FileSystemInfo
        => info.Exists ? info : default;
}
