#if NET11_0_OR_GREATER
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;

namespace Ametrin.Optional;

[Union]
partial struct Result<TValue> : Result<TValue>.IUnionMembers
{
    [Obsolete("do not use")]
    object? IUnionMembers.Value => _hasValue ? _value : _error;

    bool IUnionMembers.TryGetValue([MaybeNullWhen(false)] out TValue value)
    {
        value = _value;
        return _hasValue;
    }
    bool IUnionMembers.TryGetValue([MaybeNullWhen(false)] out Exception value)
    {
        value = _error;
        return !_hasValue;
    }

    public interface IUnionMembers
    {
        static Result<TValue> Create(TValue value) => Result.Success(value);
        static Result<TValue> Create(Exception value) => Result.Error<TValue>(value);
        [Obsolete("do not use")]
        object? Value { get; }
        bool TryGetValue([MaybeNullWhen(false)] out TValue value);
        bool TryGetValue([MaybeNullWhen(false)] out Exception value);
    }
}

[Union]
partial struct Result<TValue, TError> : Result<TValue, TError>.IUnionMembers
{
    [Obsolete("do not use")]
    object? IUnionMembers.Value => _hasValue ? _value : _error;

    bool IUnionMembers.TryGetValue([MaybeNullWhen(false)] out TValue value)
    {
        value = _value;
        return _hasValue;
    }
    bool IUnionMembers.TryGetValue([MaybeNullWhen(false)] out TError value)
    {
        value = _error;
        return !_hasValue;
    }

    public interface IUnionMembers
    {
        static Result<TValue, TError> Create(TValue value) => Result.Success<TValue, TError>(value);
        static Result<TValue, TError> Create(TError value) => Result.Error<TValue, TError>(value);
        [Obsolete("do not use")]
        object? Value { get; }
        bool TryGetValue([MaybeNullWhen(false)] out TValue value);
        bool TryGetValue([MaybeNullWhen(false)] out TError value);
    }
}
#endif
