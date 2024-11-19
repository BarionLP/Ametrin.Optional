namespace Ametrin.Optional;

public static class DisposableExtensions
{
    public static void Dispose<TValue>(this Option<TValue> option) where TValue : IDisposable
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

    public static void Dispose<TError>(this ErrorState<TError> option) where TError : IDisposable
    {
        if (option._isError)
        {
            option._error.Dispose();
        }
    }
    
    public static void Dispose<TValue>(this RefOption<TValue> option) 
        where TValue : struct, IDisposable, allows ref struct
    {
        if (option._hasValue)
        {
            option._value.Dispose();
        }
    }
}
