namespace Ametrin.Optional.Exceptions;

// They inherit from NullReferenceException because before they existed I used NullReferenceExceptions (backwards compatibility)
// and technically it is form of a null reference

[Serializable]
public sealed class OptionIsErrorException(string message = "Option is Error state") : NullReferenceException(message);

[Serializable]
public sealed class ResultIsErrorException(string message, Exception error) : NullReferenceException(message, error)
{
    public Exception Error => InnerException!;
}

[Serializable]
public sealed class ResultIsErrorException<TError>(string message, TError error) : NullReferenceException(message)
{
    public TError Error { get; } = error;
}