using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

namespace Ametrin.Optional;

partial struct Option<TValue>
{
    [AsyncExtension]
    public Option<TResult> Map<TResult>(Func<TValue, TResult> map)
        => _hasValue ? Option.Success(map(_value)) : default;
    [AsyncExtension]
    public Option<TResult> Map<TResult>(Func<TValue, Option<TResult>> map)
        => _hasValue ? map(_value) : default;

    public Option<TResult> Map<TArg, TResult>(TArg arg, Func<TValue, TArg, TResult> map)
        where TArg : allows ref struct
        => _hasValue ? Option.Success(map(_value, arg)) : default;
    public Option<TResult> Map<TArg, TResult>(TArg arg, Func<TValue, TArg, Option<TResult>> map)
        where TArg : allows ref struct
        => _hasValue ? map(_value, arg) : default;
}

partial struct Result<TValue>
{
    [AsyncExtension]
    public Result<TResult> Map<TResult>(Func<TValue, TResult> map)
        => _hasValue ? map(_value) : _error;
    [AsyncExtension]
    public Result<TResult> Map<TResult>(Func<TValue, Result<TResult>> map)
        => _hasValue ? map(_value) : _error;

    public Result<TResult> Map<TArg, TResult>(TArg arg, Func<TValue, TArg, TResult> map)
        where TArg : allows ref struct
        => _hasValue ? map(_value, arg) : _error;
    public Result<TResult> Map<TArg, TResult>(TArg arg, Func<TValue, TArg, Result<TResult>> map)
        where TArg : allows ref struct
        => _hasValue ? map(_value, arg) : _error;
}

partial struct Result<TValue, TError>
{
    [AsyncExtension]
    public Result<TResult, TError> Map<TResult>(Func<TValue, TResult> map)
        => _hasValue ? map(_value) : _error;
    [AsyncExtension]
    public Result<TResult, TError> Map<TResult>(Func<TValue, Result<TResult, TError>> map)
        => _hasValue ? map(_value) : _error;

    public Result<TResult, TError> Map<TArg, TResult>(TArg arg, Func<TValue, TArg, TResult> map)
        where TArg : allows ref struct
        => _hasValue ? map(_value, arg) : _error;
    public Result<TResult, TError> Map<TArg, TResult>(TArg arg, Func<TValue, TArg, Result<TResult, TError>> map)
        where TArg : allows ref struct
        => _hasValue ? map(_value, arg) : _error;
}

partial struct RefOption<TValue>
{
    public RefOption<TResult> Map<TResult>(Func<TValue, TResult> map)
        where TResult : struct, allows ref struct
        => _hasValue ? RefOption.Success(map(_value)) : default;
    public RefOption<TResult> Map<TResult>(Func<TValue, RefOption<TResult>> map)
        where TResult : struct, allows ref struct
        => _hasValue ? map(_value) : default;
    public Option<TResult> Map<TResult>(Func<TValue, Option<TResult>> map)
        => _hasValue ? map(_value) : default;

    public RefOption<TResult> Map<TArg, TResult>(TArg arg, Func<TValue, TArg, TResult> map)
        where TArg : allows ref struct
        where TResult : struct, allows ref struct
        => _hasValue ? RefOption.Success(map(_value, arg)) : default;
    public RefOption<TResult> Map<TArg, TResult>(TArg arg, Func<TValue, TArg, RefOption<TResult>> map)
        where TArg : allows ref struct
        where TResult : struct, allows ref struct
        => _hasValue ? map(_value, arg) : default;
    public Option<TResult> Map<TArg, TResult>(TArg arg, Func<TValue, TArg, Option<TResult>> map)
        where TArg : allows ref struct
        => _hasValue ? map(_value, arg) : default;
}

public static class OptionMapExtensions
{
    public static Option<TResult> Map<TValue, TResult>(this RefOption<TValue> option, Func<TValue, TResult> map)
        where TValue : struct, allows ref struct
        where TResult : class
        => option._hasValue ? Option.Success(map(option._value)) : default;

    public static RefOption<TResult> Map<TValue, TResult>(this Option<TValue> option, Func<TValue, TResult> map)
        where TResult : struct, allows ref struct
        => option._hasValue ? RefOption.Success(map(option._value)) : default;

    public static Option<TResult> Map<TValue, TArg, TResult>(this RefOption<TValue> option, TArg arg, Func<TValue, TArg, TResult> map)
        where TArg : allows ref struct
        where TValue : struct, allows ref struct
        where TResult : class
        => option._hasValue ? Option.Success(map(option._value, arg)) : default;

    public static RefOption<TResult> Map<TValue, TArg, TResult>(this Option<TValue> option, TArg arg, Func<TValue, TArg, TResult> map)
        where TArg : allows ref struct
        where TResult : struct, allows ref struct
        => option._hasValue ? RefOption.Success(map(option._value, arg)) : default;
}

partial class OptionTupleExtensions
{
    public static Option<R> Map<T1, T2, R>(this (Option<T1>, Option<T2>) options, Func<T1, T2, R> selector)
        => options.Item1._hasValue && options.Item2._hasValue ? selector(options.Item1._value, options.Item2._value) : Option.Error<R>();

    public static Option<R> Map<T1, T2, R>(this (Option<T1>, Option<T2>) options, Func<T1, T2, Option<R>> selector)
        => options.Item1._hasValue && options.Item2._hasValue ? selector(options.Item1._value, options.Item2._value) : default;

    public static Option<R> Map<T1, T2, TArg, R>(this (Option<T1>, Option<T2>) options, TArg arg, Func<T1, T2, TArg, R> selector)
        where TArg : allows ref struct
        => options.Item1._hasValue && options.Item2._hasValue ? selector(options.Item1._value, options.Item2._value, arg) : Option.Error<R>();

    public static Option<R> Map<T1, T2, TArg, R>(this (Option<T1>, Option<T2>) options, TArg arg, Func<T1, T2, TArg, Option<R>> selector)
        where TArg : allows ref struct
        => options.Item1._hasValue && options.Item2._hasValue ? selector(options.Item1._value, options.Item2._value, arg) : default;

    public static Result<R> Map<T1, T2, R>(this (Result<T1>, Result<T2>) options, Func<T1, T2, R> selector)
        => options.Item1._hasValue && options.Item2._hasValue ? selector(options.Item1._value, options.Item2._value) : Result.CombineErrors(options.Item1, options.Item2).ToResult(Unreachable<R>);

    public static Result<R> Map<T1, T2, R>(this (Result<T1>, Result<T2>) options, Func<T1, T2, Result<R>> selector)
        => options.Item1._hasValue && options.Item2._hasValue ? selector(options.Item1._value, options.Item2._value) : Result.CombineErrors(options.Item1, options.Item2).ToResult(Unreachable<R>);

    public static Result<R> Map<T1, T2, TArg, R>(this (Result<T1>, Result<T2>) options, TArg arg, Func<T1, T2, TArg, R> selector)
        where TArg : allows ref struct
        => options.Item1._hasValue && options.Item2._hasValue ? selector(options.Item1._value, options.Item2._value, arg) : Result.CombineErrors(options.Item1, options.Item2).ToResult(Unreachable<R>);

    public static Result<R> Map<T1, T2, TArg, R>(this (Result<T1>, Result<T2>) options, TArg arg, Func<T1, T2, TArg, Result<R>> selector)
        where TArg : allows ref struct
        => options.Item1._hasValue && options.Item2._hasValue ? selector(options.Item1._value, options.Item2._value, arg) : Result.CombineErrors(options.Item1, options.Item2).ToResult(Unreachable<R>);

    public static Result<R, E> Map<T1, T2, E, R>(this (Result<T1, E>, Result<T2, E>) options, Func<T1, T2, R> selector, Func<E, E, E> errorCombiner)
        => options.Item1._hasValue && options.Item2._hasValue ? selector(options.Item1._value, options.Item2._value) : Result.CombineErrors(options.Item1, options.Item2, errorCombiner).ToResult(Unreachable<R>);

    public static Result<R, E> Map<T1, T2, E, R>(this (Result<T1, E>, Result<T2, E>) options, Func<T1, T2, Result<R, E>> selector, Func<E, E, E> errorCombiner)
        => options.Item1._hasValue && options.Item2._hasValue ? selector(options.Item1._value, options.Item2._value) : Result.CombineErrors(options.Item1, options.Item2, errorCombiner).ToResult(Unreachable<R>);

    public static Result<R, E> Map<T1, T2, E, TArg, R>(this (Result<T1, E>, Result<T2, E>) options, TArg arg, Func<T1, T2, TArg, R> selector, Func<E, E, TArg, E> errorCombiner)
        where TArg : allows ref struct
        => options.Item1._hasValue && options.Item2._hasValue ? selector(options.Item1._value, options.Item2._value, arg) : Result.CombineErrors(options.Item1, options.Item2, arg, errorCombiner).ToResult(Unreachable<R>);

    public static Result<R, E> Map<T1, T2, E, TArg, R>(this (Result<T1, E>, Result<T2, E>) options, TArg arg, Func<T1, T2, TArg, Result<R, E>> selector, Func<E, E, TArg, E> errorCombiner)
        where TArg : allows ref struct
        => options.Item1._hasValue && options.Item2._hasValue ? selector(options.Item1._value, options.Item2._value, arg) : Result.CombineErrors(options.Item1, options.Item2, arg, errorCombiner).ToResult(Unreachable<R>);

    [DoesNotReturn]
    private static TValue Unreachable<TValue>() => throw new UnreachableException();
}