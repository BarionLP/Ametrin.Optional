namespace Ametrin.Optional;

public static class DisposableExtensions
{
    public static void Dispose<T>(this Option<T> option) where T : IDisposable
    {
        if (option._hasValue)
        {
            option._value.Dispose();
        }
    }

    public static void Dispose<TValue>(this Result<TValue> option) where TValue : IDisposable
    {
        if (option._hasValue)
        {
            option._value.Dispose();
        }
    }

    public static void Dispose<TValue, TError>(this Result<TValue, TError> option) where TValue : IDisposable
    {
        if (option._hasValue)
        {
            option._value.Dispose();
        }
    }

    public static void Dispose<T>(this ErrorState<T> option) where T : IDisposable
    {
        if (option._isFail)
        {
            option._error.Dispose();
        }
    }
}
