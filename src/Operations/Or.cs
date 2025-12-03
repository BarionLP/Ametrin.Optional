using Ametrin.Optional.Exceptions;

namespace Ametrin.Optional;

partial struct Option<TValue>
{
    [AsyncExtension]
    [OverloadResolutionPriority(1)] // to allow 'Or(default)' which would normally be ambigious
    public TValue Or(TValue other) => _hasValue ? _value : other;

    [AsyncExtension]
    public TValue Or(Func<TValue> factory) => _hasValue ? _value : factory();

    public TValue Or<TArg>(TArg arg, Func<TArg, TValue> factory)
        where TArg : allows ref struct
        => _hasValue ? _value : factory(arg);

    [AsyncExtension]
    [StackTraceHidden]
    public TValue OrThrow() => _hasValue ? _value : throw new OptionIsErrorException();

    [AsyncExtension]
    [StackTraceHidden]
    public TValue OrThrow(string message) => _hasValue ? _value : throw new OptionIsErrorException(message);

    [AsyncExtension]
    [StackTraceHidden]
    public TValue OrThrow(Func<string> messageSupplier) => _hasValue ? _value : throw new OptionIsErrorException(messageSupplier());
}

partial struct Result<TValue>
{
    [AsyncExtension]
    [OverloadResolutionPriority(1)]
    public TValue Or(TValue other) => _hasValue ? _value : other;

    [AsyncExtension]
    public TValue Or(Func<Exception, TValue> factory) => _hasValue ? _value : factory(_error);

    public TValue Or<TArg>(TArg arg, Func<Exception, TArg, TValue> factory)
        where TArg : allows ref struct
        => _hasValue ? _value : factory(_error, arg);

    [AsyncExtension]
    [StackTraceHidden]
    public TValue OrThrow() => OrThrow("Result is error state");

    [AsyncExtension]
    [StackTraceHidden]
    public TValue OrThrow(string message) => _hasValue ? _value : throw new ResultIsErrorException(message, _error);

    [AsyncExtension]
    [StackTraceHidden]
    public TValue OrThrow(Func<Exception, string> messageSupplier) => _hasValue ? _value : throw new ResultIsErrorException(messageSupplier(_error), _error);
}

partial struct Result<TValue, TError>
{
    [AsyncExtension]
    [OverloadResolutionPriority(1)]
    public TValue Or(TValue other) => _hasValue ? _value! : other;

    [AsyncExtension]
    public TValue Or(Func<TError, TValue> factory) => _hasValue ? _value! : factory(_error);

    public TValue Or<TArg>(TArg arg, Func<TError, TArg, TValue> factory)
        where TArg : allows ref struct
        => _hasValue ? _value : factory(_error, arg);

    [AsyncExtension]
    [StackTraceHidden]
    public TValue OrThrow() => OrThrow("Result is error state");

    [AsyncExtension]
    [StackTraceHidden]
    public TValue OrThrow(string message) => _hasValue ? _value : throw new ResultIsErrorException<TError>(message, _error);

    [AsyncExtension]
    [StackTraceHidden]
    public TValue OrThrow(Func<TError, string> messageSupplier) => _hasValue ? _value : throw new ResultIsErrorException<TError>(messageSupplier(_error), _error);
}

partial struct RefOption<TValue>
{
    [OverloadResolutionPriority(1)]
    public TValue Or(TValue other) => _hasValue ? _value : other;

    public TValue Or(Func<TValue> factory) => _hasValue ? _value! : factory();

    public TValue Or<TArg>(TArg arg, Func<TArg, TValue> factory)
        where TArg : allows ref struct
        => _hasValue ? _value! : factory(arg);

    [StackTraceHidden]
    public TValue OrThrow() => _hasValue ? _value : throw new OptionIsErrorException();

    [StackTraceHidden]
    public TValue OrThrow(string message) => _hasValue ? _value : throw new OptionIsErrorException(message);

    [StackTraceHidden]
    public TValue OrThrow(Func<string> messageSupplier) => _hasValue ? _value : throw new OptionIsErrorException(messageSupplier());
}

public static class OptionReferenceOrNullExtensions
{
    public static TValue? OrNull<TValue>(this Option<TValue> option) where TValue : class
        => option._hasValue ? option._value : null;
    public static TValue? OrNull<TValue>(this Result<TValue> result) where TValue : class
        => result._hasValue ? result._value : null;
    public static TValue? OrNull<TValue, TError>(this Result<TValue, TError> result) where TValue : class
        => result._hasValue ? result._value : null;
}

public static class OptionValueOrNullExtensions
{
    public static TValue? OrNull<TValue>(this Option<TValue> option) where TValue : struct
        => option._hasValue ? option._value! : null;
    public static TValue? OrNull<TValue>(this Result<TValue> result) where TValue : struct
        => result._hasValue ? result._value : null;
    public static TValue? OrNull<TValue, TError>(this Result<TValue, TError> result) where TValue : struct
        => result._hasValue ? result._value : null;
    public static TValue? OrNull<TValue>(this RefOption<TValue> option) where TValue : struct
        => option._hasValue ? option._value! : null;
}