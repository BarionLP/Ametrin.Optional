namespace Ametrin.Optional;

partial struct Option
{
    public override string ToString() => _success ? "Success" : "Error";
}

partial struct Option<TValue>
{
    public override string ToString() => _hasValue ? _value!.ToString() ?? "Success" : "Error";
}

partial struct Result<TValue>
{
    public override string ToString() => _hasValue ? _value!.ToString() ?? "Success" : _error.Message;
}

partial struct Result<TValue, TError>
{
    public override string ToString() => _hasValue ? _value!.ToString() ?? "Success" : _error!.ToString() ?? "Error";
}

partial struct ErrorState
{
    public override string ToString() => _isError ? _error.Message : "Success";
}

partial struct ErrorState<TError>
{
    public override string ToString() => _isError ? _error!.ToString() ?? "Error" : "Success";
}