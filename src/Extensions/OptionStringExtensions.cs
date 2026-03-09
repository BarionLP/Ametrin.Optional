namespace Ametrin.Optional;

public static class OptionStringExtensions
{
    public static Option<T> Parse<T>(this Option<string> option, IFormatProvider? provider = null) where T : IParsable<T>
        => option._hasValue ? OptionParsingExtensions.TryParse<T>(option._value, provider) : default;

    public static RefOption<ReadOnlySpan<char>> MapAsSpan(this Option<string> option)
        => option.Map(static s => s.AsSpan());

    public static Option<string> RejectEmpty(this Option<string> option)
        => option.Reject(string.IsNullOrEmpty);

    public static Option<string> RejectWhiteSpace(this Option<string> option)
        => option.Reject(string.IsNullOrWhiteSpace);
}