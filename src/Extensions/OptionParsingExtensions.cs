namespace Ametrin.Optional;

#if NET10_0_OR_GREATER

public static class OptionParsingExtensions
{
    extension<T>(T) where T : IParsable<T>
    {
        public static Option<T> TryParse(string? s, IFormatProvider? provider = null)
            => T.TryParse(s, provider, out var result) ? Option.Success(result) : default;
    }
    
    extension<T>(T) where T : ISpanParsable<T>
    {
        public static Option<T> TryParse(ReadOnlySpan<char> s, IFormatProvider? provider = null)
            => T.TryParse(s, provider, out var result) ? Option.Success(result) : default;
    }
}

#endif