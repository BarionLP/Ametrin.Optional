using System.Diagnostics;

namespace Ametrin.Optional;

partial struct Option<TValue>
{
    public Option<TBase> As<TBase>()
    {
        return _hasValue ? _value is TBase value ? Option.Success(value) : throw new UnreachableException() : default;
    }
}

partial struct Result<TValue>
{
    public Result<TBase> As<TBase>()
    {
        return _hasValue ? _value is TBase value ? value : throw new UnreachableException() : _error;
    }
}

partial struct Result<TValue, TError>
{
    public Result<TBase, TError> As<TBase>()
    {
        return _hasValue ? _value is TBase value ? value : throw new UnreachableException() : _error;
    }
}
