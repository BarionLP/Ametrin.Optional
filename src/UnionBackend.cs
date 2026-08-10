#if NET11_0_OR_GREATER
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;

namespace Ametrin.Optional;

[Union]
partial struct Option<TValue> : Option<TValue>.IUnionMembers
{
    [Obsolete("do not use")]
    object? IUnionMembers.Value => _value;

    bool IUnionMembers.TryGetValue([MaybeNullWhen(false)] out TValue value)
    {
        value = _value;
        return _hasValue;
    }

    public interface IUnionMembers
    {
        static Option<TValue> Create(TValue value) => Option.Success(value);
        [Obsolete("do not use")]
        object? Value { get; }
        bool TryGetValue([MaybeNullWhen(false)] out TValue value);
    }
}
#endif
