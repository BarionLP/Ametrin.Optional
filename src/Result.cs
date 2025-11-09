namespace Ametrin.Optional;

/// <summary>
/// A simplified version of <see cref="Result{TValue, TError}"/> with <see cref="Exception"/> as error type
/// </summary>
/// <typeparam name="TValue">type of value</typeparam>
[GenerateAsyncExtensions]
public readonly partial struct Result<TValue>
{
    internal readonly TValue _value;
    internal readonly Exception _error;
    internal readonly bool _hasValue = false;

    public Result() : this(null) { }
    internal Result(TValue value)
        : this(value, default!, true) { }
    internal Result(Exception? error = null)
        : this(default!, error ?? new Exception(), false) { }
    internal Result(TValue value, Exception error, bool hasValue)
        => (_value, _error, _hasValue) = (value, error, hasValue);
    public Result(Result<TValue> other)
        : this(other._value, other._error, other._hasValue) { }
}

/// <summary>
/// A struct representing either a value of type <typeparamref name="TValue"/> or an error of type <typeparamref name="TError"/>
/// </summary>
/// <typeparam name="TValue">type of value</typeparam>
/// <typeparam name="TError">type of error</typeparam>
[GenerateAsyncExtensions]
public readonly partial struct Result<TValue, TError>
{
    internal readonly TValue _value;
    internal readonly TError _error;
    internal readonly bool _hasValue = false;

    public Result() : this(default(TError)!) { }
    internal Result(TValue value)
        : this(value, default!, true) { }
    internal Result(TError error)
        : this(default!, error ?? throw new ArgumentNullException(nameof(error), "Cannot create Error with null"), false) { }
    internal Result(TValue value, TError error, bool hasValue)
        => (_value, _error, _hasValue) = (value, error, hasValue);
    public Result(Result<TValue, TError> other)
        : this(other._value, other._error, other._hasValue) { }
}

public static class Result
{
    public static Result<T> Success<T>(T value)
        => value is null ? throw new ArgumentNullException(nameof(value), "Cannot create Success with null value") : new(value);
    public static Result<T> Error<T>(Exception? error = null) => new(error);

    public static Result<T> Of<T>(T? value, Exception? error = null) => value is null ? (error ?? new NullReferenceException()) : value;
    public static Result<T> Of<T>(T? value, Func<Exception> error) => value is null ? error() : value;
    public static Result<T> Of<T>(T? value, Exception? error = null) where T : struct
        => value.HasValue ? value.Value : (error ?? new NullReferenceException());

    public static Result<T> Try<T>(Func<T> action)
    {
        try
        {
            return action();
        }
        catch (Exception e)
        {
            return e;
        }
    }

    public static Result<TResult> Try<TArg, TResult>(TArg arg, Func<TArg, TResult> action)
        where TArg : allows ref struct
    {
        try
        {
            return action(arg);
        }
        catch (Exception e)
        {
            return e;
        }
    }

    public static Result<TValue, TError> Success<TValue, TError>(TValue value)
        => value is null ? throw new ArgumentNullException(nameof(value), "Cannot create Success with null value") : new(value);
    public static Result<TValue, TError> Error<TValue, TError>(TError error) => new(error);

    public static ErrorState CombineErrors<T1, T2>(Result<T1> a, Result<T2> b)
        => ErrorState.CombineErrors(a.ToErrorState(), b.ToErrorState());
    public static ErrorState<E> CombineErrors<T1, T2, E>(Result<T1, E> a, Result<T2, E> b, Func<E, E, E> errorCombiner)
        => ErrorState.CombineErrors(a.ToErrorState(), b.ToErrorState(), errorCombiner);
    public static ErrorState<E> CombineErrors<T1, T2, E, TArg>(Result<T1, E> a, Result<T2, E> b, TArg arg, Func<E, E, TArg, E> errorCombiner)
        where TArg : allows ref struct
        => ErrorState.CombineErrors(a.ToErrorState(), b.ToErrorState(), arg, errorCombiner);
    public static ErrorState CombineErrors<T1, T2>(Result<T1> a, Result<T2> b, Result<T2> c)
        => ErrorState.CombineErrors(a.ToErrorState(), b.ToErrorState(), c.ToErrorState());
}