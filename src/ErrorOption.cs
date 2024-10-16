namespace Ametrin.Optional;

public readonly struct ErrorOption
{
    internal readonly bool _success;
    internal ErrorOption(bool success){
        _success = success;
    }
}
