using System.Diagnostics.CodeAnalysis;

namespace Ametrin.Optional;

partial struct Option
{
    public bool Branch()
    {
        return _success;
    }
}

partial struct Option<TValue>
{
    public bool Branch([MaybeNullWhen(false)] out TValue value)
    {
        value = _value;
        return _hasValue;
    }
}

partial struct Result<TValue>
{
    public bool Branch([MaybeNullWhen(false)] out TValue value, [MaybeNullWhen(true)] out Exception error)
    {
        value = _value;
        error = _error;
        return _hasValue;
    }
}

partial struct Result<TValue, TError>
{
    public bool Branch([MaybeNullWhen(false)] out TValue value, [MaybeNullWhen(true)] out TError error)
    {
        value = _value;
        error = _error;
        return _hasValue;
    }
}

partial struct ErrorState
{
    public bool Branch([MaybeNullWhen(true)] out Exception error)
    {
        error = _error;
        return !_isError;
    }
}

partial struct ErrorState<TError>
{
    public bool Branch([MaybeNullWhen(true)] out TError error)
    {
        error = _error;
        return !_isError;
    }
}

partial struct RefOption<TValue>
{
    public bool Branch([MaybeNullWhen(false)] out TValue value)
    {
        value = _value;
        return _hasValue;
    }
}