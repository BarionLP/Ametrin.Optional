namespace Ametrin.Optional;

partial struct Option<T>
{
    public void Consume(Action<T>? success = null, Action? error = null)
    {
        if (_hasValue)
        {
            success?.Invoke(_value);
        }
        else
        {
            error?.Invoke();
        }
    }
}
