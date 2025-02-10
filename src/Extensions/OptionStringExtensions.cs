namespace Ametrin.Optional;

public static class OptionStringExtensions
{
    public static Option<T> Parse<T>(this Option<string> option, IFormatProvider? provider = null) where T : IParsable<T>
        => option._hasValue && T.TryParse(option._value, provider, out var result) ? Option.Success(result) : default;

    public static Option<T> Parse<T>(this RefOption<ReadOnlySpan<char>> option, IFormatProvider? provider = null) where T : ISpanParsable<T>
        => option._hasValue && T.TryParse(option._value, provider, out var result) ? Option.Success(result) : default;

    public static RefOption<ReadOnlySpan<char>> MapAsSpan(this Option<string> option)
        => option.Map(static s => s.AsSpan());

    public static Option<string> RejectEmpty(this Option<string> option)
        => option.Reject(string.IsNullOrEmpty);

    public static Option<string> RejectWhiteSpace(this Option<string> option)
        => option.Reject(string.IsNullOrWhiteSpace);
}