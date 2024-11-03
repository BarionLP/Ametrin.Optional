namespace Ametrin.Optional;

public static class StringExtensions
{
    public static Option<T> ParseOrNone<T>(this string s, IFormatProvider? provider = null) where T : ISpanParsable<T>
        => T.TryParse(s, provider, out var result) ? result : default(Option<T>);

    public static Option<T> ParseOrNone<T>(this ReadOnlySpan<char> s, IFormatProvider? provider = null) where T : ISpanParsable<T>
        => T.TryParse(s, provider, out var result) ? result : default(Option<T>);
    public static Option<T> Parse<T>(this Option<string> option, IFormatProvider? provider = null) where T : ISpanParsable<T>
        => option._hasValue && T.TryParse(option._value, provider, out var result) ? result : default(Option<T>);

    public static Option<string> WhereNotEmpty(this Option<string> option)
        => option.WhereNot(string.IsNullOrEmpty);

    public static Option<string> WhereNotWhiteSpace(this Option<string> option)
        => option.WhereNot(string.IsNullOrWhiteSpace);

}