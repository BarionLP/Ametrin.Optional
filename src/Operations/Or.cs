namespace Ametrin.Optional;

partial struct Option<TValue>
{
#if NET9_0_OR_GREATER
    [OverloadResolutionPriority(1)] // to allow 'Or(default)' which would normally be ambigious
#endif
    public TValue Or(TValue other) => _hasValue ? _value : other;
    public TValue Or(Func<TValue> factory) => _hasValue ? _value! : factory();
    public TValue OrThrow() => _hasValue ? _value : throw new NullReferenceException("Option is Error state");
}

partial struct Result<TValue>
{
#if NET9_0_OR_GREATER
    [OverloadResolutionPriority(1)]
#endif
    public TValue Or(TValue other) => _hasValue ? _value : other;
    public TValue Or(Func<Exception, TValue> factory) => _hasValue ? _value : factory(_error);
    public TValue OrThrow() => _hasValue ? _value : throw new NullReferenceException("Result is Error state", _error);
}

partial struct Result<TValue, TError>
{
#if NET9_0_OR_GREATER
    [OverloadResolutionPriority(1)]
#endif
    public TValue Or(TValue other) => _hasValue ? _value! : other;
    public TValue Or(Func<TError, TValue> factory) => _hasValue ? _value! : factory(_error);
    public TValue OrThrow() => _hasValue ? _value! : throw new NullReferenceException("Result is Error state");
}

public static class OrExtensions
{
    public static TValue? OrNull<TValue>(this Option<TValue> option) where TValue : class
        => option._hasValue ? option._value! : null;

    public static TValue? OrNull<TValue>(this Result<TValue> result) where TValue : class
        => result._hasValue ? result._value : null;
        
    public static TValue? OrNull<TValue, TError>(this Result<TValue, TError> result) where TValue : class
        => result._hasValue ? result._value : null;
}