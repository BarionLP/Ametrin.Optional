namespace Ametrin.Optional;

internal static class Sentinel
{
    internal static readonly Exception Error = new();
    internal static readonly Exception Null = new NullReferenceException();
}