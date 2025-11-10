namespace Ametrin.Optional;

public static class OptionSpanExtensions
{
    public static Option<T> Parse<T>(this RefOption<ReadOnlySpan<char>> option, IFormatProvider? provider = null) where T : ISpanParsable<T>
        => option._hasValue && T.TryParse(option._value, provider, out var result) ? Option.Success(result) : default;
    public static RefOption<Span<T>> RejectEmpty<T>(this RefOption<Span<T>> option)
        => option.Reject(static span => span.IsEmpty);
    public static RefOption<ReadOnlySpan<T>> RejectEmpty<T>(this RefOption<ReadOnlySpan<T>> option)
        => option.Reject(static span => span.IsEmpty);
}
