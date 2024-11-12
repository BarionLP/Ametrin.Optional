using System.Diagnostics;

namespace Ametrin.Optional;

public static class DebugExtensions
{
    public static void AssertSuccess<T>(this Option<T> state, string message = "Option was failure") => Debug.Assert(state._hasValue == true, message);
    public static void AssertSuccess<T>(this Result<T> state, string message = "Result was failure") => Debug.Assert(state._hasValue == true, message);
    public static void AssertSuccess<T, TError>(this Result<T, TError> state, string message = "Result was failure") => Debug.Assert(state._hasValue == true, message);
    public static void AssertSuccess(this Option state, string message = "Option was failure") => Debug.Assert(state._success == true, message);
    public static void AssertSuccess<T>(this ErrorState<T> state, string message = "ErrorState was failure") => Debug.Assert(state._isFail == false, message);
}
