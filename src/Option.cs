namespace Ametrin.Optional;

public readonly partial struct Option : IEquatable<Option>
{
    internal readonly bool _success;

    public Option() : this(true) { }
    internal Option(bool success)
    {
        _success = success;
    }

    public static Option Success() => new(true);
    public static Option Fail() => new(false);

    public static Option<T> None<T>() => default;
    public static Option<T> Some<T>(T value)
        => value is null ? throw new ArgumentNullException(nameof(value), "Cannot create Option with null value") : new(value, true);

    public static Option<T> Of<T>(T? value)
        => value is null ? default : new(value, true);
    public static Option<T> Of<T>(T? value) where T : struct
        => value.HasValue ? new(value.Value, true) : default;

    public TResult Select<TResult>(Func<TResult> success, Func<TResult> fail) => _success ? success() : fail();

    public void Consume(Action? success = null, Action? error = null)
    {
        if (_success)
        {
            success?.Invoke();
        }
        else
        {
            error?.Invoke();
        }
    }

    [Obsolete]
    public void IfSuccess(Action action)
    {
        if (_success)
        {
            action();
        }
    }

    [Obsolete]
    public void IfFail(Action action)
    {
        if (!_success)
        {
            action();
        }
    }

    public override string ToString() => _success ? "Success" : "Error";
    public override int GetHashCode() => _success.GetHashCode();
    public override bool Equals(object? obj) => obj is Option s && Equals(s);
    public bool Equals(Option other) => _success == other._success;
    public static bool operator ==(Option left, Option right) => left.Equals(right);
    public static bool operator !=(Option left, Option right) => !(left == right);

    public static implicit operator bool(Option state) => state._success;
    public static implicit operator Option(bool state) => new(state);
}

public readonly partial struct Option<T> 
{
    internal readonly T _value;
    internal readonly bool _hasValue = false;

    public Option() : this(default!, false) { }
    internal Option(T value, bool hasValue)
        => (_value, _hasValue) = (value, hasValue);
    public Option(Option<T> other)
        : this(other._value, other._hasValue) { }

    public override string ToString() => _hasValue ? _value!.ToString() ?? "NullString" : "None";

    public static implicit operator Option<T>(T? value) => Option.Of(value);
}