using System.Diagnostics;

namespace Ametrin.Optional;

public static class OptionDebugExtensions
{
    public static void AssertSuccess<TValue>(this Option<TValue> state, string message = "Option was error") 
        => Debug.Assert(state._hasValue == true, message);
    public static void AssertSuccess<TValue>(this Result<TValue> state, string message = "Result was error") 
        => Debug.Assert(state._hasValue == true, message);
    public static void AssertSuccess<TValue, TError>(this Result<TValue, TError> state, string message = "Result was error") 
        => Debug.Assert(state._hasValue == true, message);
    public static void AssertSuccess(this Option state, string message = "Option was error") 
        => Debug.Assert(state._success == true, message);
    public static void AssertSuccess<TError>(this ErrorState<TError> state, string message = "ErrorState was error") 
        => Debug.Assert(state._isError == false, message);
    public static void AssertSuccess<TValue>(this RefOption<TValue> option, string message = "Option was error")
        where TValue : struct, allows ref struct
        => Debug.Assert(option._hasValue == true, message);
}
