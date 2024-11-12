namespace Ametrin.Optional;

public static class OptionExtensions
{
    public static Option<TValue> ToOption<TValue>(this TValue? value) where TValue : class => Option.Of(value);
    public static Option<TValue> ToOption<TValue>(this TValue? value) where TValue : struct => Option.Of(value);
    public static Option<TValue> ToOption<TValue>(this object? value) => value is TValue t ? t : default(Option<TValue>);

    public static Result<TValue> ToResult<TValue>(this TValue? value) where TValue : class => Result.Of(value);
    public static Result<TValue> ToResult<TValue>(this TValue? value) where TValue : struct => Result.Of(value);
    public static Result<TValue> ToResult<TValue>(this object? value) => value.ToResult().Where<TValue>();

    public static Option<TValue> ToOption<TValue>(this Result<TValue> result)
        => result._hasValue ? result._value : default(Option<TValue>);
    public static Result<TValue> ToResult<TValue>(this Option<TValue> option, Exception? error = null) 
        => option._hasValue ? option._value : error;
    public static Result<TValue, TError> ToResult<TValue, TError>(this Option<TValue> option, TError error) 
        => option._hasValue ? option._value : error;
    
    public static Result<TValue> ToResult<TValue>(this Option<TValue> option, Func<Exception> error) 
        => option._hasValue ? option._value : error();
    public static Result<TValue, TError> ToResult<TValue, TError>(this Option<TValue> option, Func<TError> error)
        => option._hasValue ? option._value : error();
}
