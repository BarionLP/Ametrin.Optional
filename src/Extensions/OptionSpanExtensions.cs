using System.Buffers;
using System.Collections.Generic;

namespace Ametrin.Optional;

public static class OptionSpanExtensions
{
    public static Option<T> Parse<T>(this RefOption<ReadOnlySpan<char>> option, IFormatProvider? provider = null) where T : ISpanParsable<T>
        => option._hasValue ? OptionParsingExtensions.TryParse<T>(option._value, provider) : default;

    public static RefOption<Span<T>> RejectEmpty<T>(this RefOption<Span<T>> option)
        => option.Reject(static span => span.IsEmpty);
    public static RefOption<ReadOnlySpan<T>> RejectEmpty<T>(this RefOption<ReadOnlySpan<T>> option)
        => option.Reject(static span => span.IsEmpty);

    public static Option<int> TryIndexOf<T>(this ReadOnlySpan<T> span, T value, IEqualityComparer<T>? comparer = null)
        => Option.Success(span.IndexOf(value, comparer)).Require(IsNotNegativeOne);
    public static Option<int> TryIndexOf<T>(this ReadOnlySpan<T> span, ReadOnlySpan<T> value, IEqualityComparer<T>? comparer = null)
        => Option.Success(span.IndexOf(value, comparer)).Require(IsNotNegativeOne);

    public static Option<int> TryIndexOf<T>(this ReadOnlySpan<T> span, T value)
        where T : IEquatable<T>?
        => Option.Success(span.IndexOf(value)).Require(IsNotNegativeOne);
    public static Option<int> TryIndexOf<T>(this ReadOnlySpan<T> span, ReadOnlySpan<T> value)
        where T : IEquatable<T>?
        => Option.Success(span.IndexOf(value)).Require(IsNotNegativeOne);
    // explicit Span<char> overload because string could not find the correct one
    public static Option<int> TryIndexOf(this ReadOnlySpan<char> span, ReadOnlySpan<char> value)
        => Option.Success(span.IndexOf(value)).Require(IsNotNegativeOne);
    public static Option<int> TryIndexOf(this ReadOnlySpan<char> span, ReadOnlySpan<char> value, StringComparison comparisonType)
        => Option.Success(span.IndexOf(value, comparisonType)).Require(IsNotNegativeOne);

    public static Option<int> TryLastIndexOf<T>(this ReadOnlySpan<T> span, T value, IEqualityComparer<T>? comparer = null)
        => Option.Success(span.LastIndexOf(value, comparer)).Require(IsNotNegativeOne);
    public static Option<int> TryLastIndexOf<T>(this ReadOnlySpan<T> span, ReadOnlySpan<T> value, IEqualityComparer<T>? comparer = null)
        => Option.Success(span.LastIndexOf(value, comparer)).Require(IsNotNegativeOne);

    public static Option<int> TryLastIndexOf<T>(this ReadOnlySpan<T> span, T value)
        where T : IEquatable<T>?
        => Option.Success(span.LastIndexOf(value)).Require(IsNotNegativeOne);
    public static Option<int> TryLastIndexOf<T>(this ReadOnlySpan<T> span, ReadOnlySpan<T> value)
        where T : IEquatable<T>?
        => Option.Success(span.LastIndexOf(value)).Require(IsNotNegativeOne);
    // explicit Span<char> overload because string could not find the correct one
    public static Option<int> TryLastIndexOf(this ReadOnlySpan<char> span, ReadOnlySpan<char> value)
        => Option.Success(span.LastIndexOf(value)).Require(IsNotNegativeOne);
    public static Option<int> TryLastIndexOf(this ReadOnlySpan<char> span, ReadOnlySpan<char> value, StringComparison comparisonType)
        => Option.Success(span.LastIndexOf(value, comparisonType)).Require(IsNotNegativeOne);

    public static Option<int> TryIndexOfAny<T>(this ReadOnlySpan<T> span, T value0, T value1)
        where T : IEquatable<T>?
        => Option.Success(span.IndexOfAny(value0, value1));
    public static Option<int> TryIndexOfAny<T>(this ReadOnlySpan<T> span, T value0, T value1, T value2)
        where T : IEquatable<T>?
        => Option.Success(span.IndexOfAny(value0, value1, value2));
    public static Option<int> TryIndexOfAny<T>(this ReadOnlySpan<T> span, ReadOnlySpan<T> values)
        where T : IEquatable<T>?
        => Option.Success(span.IndexOfAny(values));
    public static Option<int> TryIndexOfAny(this ReadOnlySpan<char> span, SearchValues<string> values)
        => Option.Success(span.IndexOfAny(values));

    private static bool IsNotNegativeOne(int value) => value is not -1;
}
