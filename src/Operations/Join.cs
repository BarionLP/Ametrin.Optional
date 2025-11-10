namespace Ametrin.Optional.Operations;

public static class OptionJoinExtensions
{
    public static Option<(T1, T2)> Join<T1, T2>(this Option<T1> a, Option<T2> b)
        => a._hasValue && b._hasValue ? (a._value, b._value) : Option.Error<(T1, T2)>();
    public static Option<(T1, T2, T3)> Join<T1, T2, T3>(this Option<(T1, T2)> a, Option<T3> b)
        => a._hasValue && b._hasValue ? (a._value.Item1, a._value.Item2, b._value) : Option.Error<(T1, T2, T3)>();
    public static Option<(T1, T2, T3, T4)> Join<T1, T2, T3, T4>(this Option<(T1, T2, T3)> a, Option<T4> b)
        => a._hasValue && b._hasValue ? (a._value.Item1, a._value.Item2, a._value.Item3, b._value) : Option.Error<(T1, T2, T3, T4)>();

    public static Result<(T1, T2)> Join<T1, T2>(this Result<T1> a, Result<T2> b)
        => a._hasValue && b._hasValue ? (a._value, b._value) : Result.CombineErrors(a, b)._error;
    public static Result<(T1, T2, T3)> Join<T1, T2, T3>(this Result<(T1, T2)> a, Result<T3> b)
        => a._hasValue && b._hasValue ? (a._value.Item1, a._value.Item2, b._value) : Result.CombineErrors(a, b)._error;
    public static Result<(T1, T2, T3, T4)> Join<T1, T2, T3, T4>(this Result<(T1, T2, T3)> a, Result<T4> b)
        => a._hasValue && b._hasValue ? (a._value.Item1, a._value.Item2, a._value.Item3, b._value) : Result.CombineErrors(a, b)._error;

    public static Result<(T1, T2), E> Join<T1, T2, E>(this Result<T1, E> a, Result<T2, E> b, Func<E, E, E> errorJoiner)
        => a._hasValue && b._hasValue ? (a._value, b._value) : Result.CombineErrors(a, b, errorJoiner)._error;
    public static Result<(T1, T2, T3), E> Join<T1, T2, T3, E>(this Result<(T1, T2), E> a, Result<T3, E> b, Func<E, E, E> errorJoiner)
        => a._hasValue && b._hasValue ? (a._value.Item1, a._value.Item2, b._value) : Result.CombineErrors(a, b, errorJoiner)._error;
    public static Result<(T1, T2, T3, T4), E> Join<T1, T2, T3, T4, E>(this Result<(T1, T2, T3), E> a, Result<T4, E> b, Func<E, E, E> errorJoiner)
        => a._hasValue && b._hasValue ? (a._value.Item1, a._value.Item2, a._value.Item3, b._value) : Result.CombineErrors(a, b, errorJoiner)._error;
}
