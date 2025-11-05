namespace Ametrin.Optional.Parsing;

public interface IOptionSpanParsable<TSelf> where TSelf : IOptionSpanParsable<TSelf>
{
    public static abstract Option<TSelf> TryParse(ReadOnlySpan<char> span, IFormatProvider? provider = null);
}
