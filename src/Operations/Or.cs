namespace Ametrin.Optional;

partial struct Option<T>
{
#if NET9_0_OR_GREATER
    [OverloadResolutionPriority(1)] // to allow 'Or(null)' which would normally be ambigious
#endif
    public T Or(T other) => _hasValue ? _value! : other;
    public T Or(Func<T> factory) => _hasValue ? _value! : factory();
    public T OrThrow() => _hasValue ? _value! : throw new NullReferenceException("Option failed");
}

partial struct Result<TValue>
{
#if NET9_0_OR_GREATER
    [OverloadResolutionPriority(1)] // to allow 'Or(null)' which would normally be ambigious
#endif
    public TValue Or(TValue other) => _hasValue ? _value : other;
    public TValue Or(Func<Exception, TValue> factory) => _hasValue ? _value : factory(_error);
    public TValue OrThrow() => _hasValue ? _value : throw new NullReferenceException("Result failed");
}

partial struct Result<TValue, TError>
{
#if NET9_0_OR_GREATER
    [OverloadResolutionPriority(1)] // to allow 'Or(null)' which would normally be ambigious
#endif
    public TValue Or(TValue other) => _hasValue ? _value! : other;
    public TValue Or(Func<TError, TValue> factory) => _hasValue ? _value! : factory(_error);
    public TValue OrThrow() => _hasValue ? _value! : throw new NullReferenceException("Result failed");
}