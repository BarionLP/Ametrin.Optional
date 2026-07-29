using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;

namespace Ametrin.Optional;

#if NET11_0_OR_GREATER
[Union]
partial struct Option<TValue> : Option<TValue>.IUnionMembers
{
    [EditorBrowsable(EditorBrowsableState.Never)]
    object? IUnionMembers.Value => _value;

    [EditorBrowsable(EditorBrowsableState.Never)]
    public bool TryGetValue([MaybeNullWhen(false)] out TValue value)
    {
        value = _value;
        return _hasValue;
    }

    public interface IUnionMembers
    {
        static Option<TValue> Create(TValue value) => Option.Success(value);
        object? Value { get; }
    }
}
#endif
