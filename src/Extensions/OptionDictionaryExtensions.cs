using System.Collections.Frozen;
using System.Collections.Generic;

namespace Ametrin.Optional;

public static class OptionDictionaryExtensions
{
    public static Option<TValue> TryGetValue<TKey, TValue>(this IReadOnlyDictionary<TKey, TValue> dictionary, TKey key)
        where TKey : notnull
        => dictionary.TryGetValue(key, out var result) ? Option.Success(result) : default;

    public static Option<TValue> TryGetValue<TKey, TValue, TAlternate>(this Dictionary<TKey, TValue>.AlternateLookup<TAlternate> lookup, TAlternate key)
        where TKey : notnull
        where TAlternate : notnull, allows ref struct
        => lookup.TryGetValue(key, out var result) ? Option.Success(result) : default;

    public static Option<TValue> TryGetValue<TKey, TValue, TAlternate>(this FrozenDictionary<TKey, TValue>.AlternateLookup<TAlternate> lookup, TAlternate key)
        where TKey : notnull
        where TAlternate : notnull, allows ref struct
        => lookup.TryGetValue(key, out var result) ? Option.Success(result) : default;
}
