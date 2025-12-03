namespace Ametrin.Optional;

/// <summary>
/// providing unrecommended and/or unsafe access to all option types for edge case scenarios
/// </summary>
public static partial class OptionsMarshall
{
    public static bool IsSuccess<TValue>(Option<TValue> option) => option._hasValue;
    public static bool IsSuccess<TValue>(Result<TValue> result) => result._hasValue;
    public static bool IsSuccess<TValue, TError>(Result<TValue, TError> result) => result._hasValue;
    public static bool IsSuccess(ErrorState errorState) => !errorState._isError;
    public static bool IsSuccess<TError>(ErrorState<TError> errorState) => !errorState._isError;
    public static bool IsSuccess<TValue>(RefOption<TValue> option) where TValue : struct, allows ref struct => option._hasValue;

    public static TValue? GetValueUnsafe<TValue>(Option<TValue> option) => option._value;
    public static TValue? GetValueUnsafe<TValue>(Result<TValue> result) => result._value;
    public static TValue? GetValueUnsafe<TValue, TError>(Result<TValue, TError> result) => result._value;
    public static TValue GetValueUnsafe<TValue>(RefOption<TValue> option)
        where TValue : struct, allows ref struct
        => option._value;

    public static Exception? GetErrorOrNull<TValue>(Result<TValue> result)
        => result.Branch(out _, out var error) ? null : error;
    public static Exception GetError<TValue>(Result<TValue> result)
        => result.Branch(out _, out var error) ? throw new InvalidOperationException() : error;
    public static Exception? GetErrorUnsafe<TValue>(Result<TValue> result)
        => result._error;

    public static TError GetError<TValue, TError>(Result<TValue, TError> result)
        => result.Branch(out _, out var error) ? throw new InvalidOperationException() : error;
    public static TError? GetErrorUnsafe<TValue, TError>(Result<TValue, TError> result)
        => result._error;

    public static Exception? GetErrorOrNull(ErrorState errorState)
        => errorState.Branch(out var error) ? null : error;
    public static Exception GetError(ErrorState errorState)
        => errorState.Branch(out var error) ? throw new InvalidOperationException() : error;
    public static Exception? GetErrorUnsafe(ErrorState result)
        => result._error;

    public static TError GetError<TError>(ErrorState<TError> errorState)
        => errorState.Branch(out var error) ? throw new InvalidOperationException() : error;
    public static TError? GetErrorUnsafe<TError>(ErrorState<TError> result)
        => result._error;
}