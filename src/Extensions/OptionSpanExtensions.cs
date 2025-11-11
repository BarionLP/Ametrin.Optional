using System.Collections.Generic;

namespace Ametrin.Optional;

public static class OptionSpanExtensions
{
    public static Option<T> Parse<T>(this RefOption<ReadOnlySpan<char>> option, IFormatProvider? provider = null) where T : ISpanParsable<T>
        => option._hasValue && T.TryParse(option._value, provider, out var result) ? Option.Success(result) : default;
    public static RefOption<Span<T>> RejectEmpty<T>(this RefOption<Span<T>> option)
        => option.Reject(static span => span.IsEmpty);
    public static RefOption<ReadOnlySpan<T>> RejectEmpty<T>(this RefOption<ReadOnlySpan<T>> option)
        => option.Reject(static span => span.IsEmpty);

#if NET10_0_OR_GREATER
    public static Option<int> TryIndexOf<T>(this ReadOnlySpan<T> span, T value, IEqualityComparer<T>? comparer = null)
        => Option.Success(span.IndexOf(value, comparer)).Require(IsNotNegativeOne);
    public static Option<int> TryIndexOf<T>(this ReadOnlySpan<T> span, ReadOnlySpan<T> value, IEqualityComparer<T>? comparer = null)
        => Option.Success(span.IndexOf(value, comparer)).Require(IsNotNegativeOne);
#endif

    public static Option<int> TryIndexOf(this ReadOnlySpan<char> span, char value)
        => Option.Success(span.IndexOf(value)).Require(IsNotNegativeOne);
    public static Option<int> TryIndexOf(this ReadOnlySpan<char> span, ReadOnlySpan<char> value)
        => Option.Success(span.IndexOf(value)).Require(IsNotNegativeOne);
    public static Option<int> TryIndexOf(this ReadOnlySpan<char> span, ReadOnlySpan<char> value, StringComparison comparer)
        => Option.Success(span.IndexOf(value, comparer)).Require(IsNotNegativeOne);

    private static bool IsNotNegativeOne(int value) => value is not -1;
}
