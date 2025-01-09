namespace Ametrin.Optional;

public static class OptionStringExtensions
{
    public static Option<T> Parse<T>(this Option<string> option, IFormatProvider? provider = null) where T : IParsable<T>
        => option._hasValue && T.TryParse(option._value, provider, out var result) ? Option.Success(result) : default;

    public static Option<T> Parse<T>(this RefOption<ReadOnlySpan<char>> option, IFormatProvider? provider = null) where T : ISpanParsable<T>
        => option._hasValue && T.TryParse(option._value, provider, out var result) ? Option.Success(result) : default;

    public static RefOption<ReadOnlySpan<char>> SelectAsSpan(this Option<string> option)
        => option.Select(s => s.AsSpan());

    public static Option<string> WhereNotEmpty(this Option<string> option)
        => option.WhereNot(string.IsNullOrEmpty);

    public static Option<string> WhereNotWhiteSpace(this Option<string> option)
        => option.WhereNot(string.IsNullOrWhiteSpace);
}