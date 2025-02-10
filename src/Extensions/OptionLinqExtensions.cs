using System.Linq;

namespace Ametrin.Optional;

public static class OptionLinqExtensions
{
    public static Option<IEnumerable<T>> RequireNotEmpty<T>(this IEnumerable<T> source)
        => source is not null && source.Any() ? Option.Success(source) : default;
    public static Option<IEnumerable<T>> RequireNotEmpty<T>(this Option<IEnumerable<T>> option)
        => option.Require(static collection => collection.Any());
    public static Result<IEnumerable<T>> RequireNotEmpty<T>(this Result<IEnumerable<T>> option)
        => option.RequireNotEmpty(static value => new ArgumentException("Sequence was empty"));
    public static Result<IEnumerable<T>> RequireNotEmpty<T>(this Result<IEnumerable<T>> option, Func<IEnumerable<T>, Exception> error)
        => option.Require(static collection => collection.Any(), error);

    public static Option<T> FirstOrNone<T>(this IEnumerable<T> source)
    {
        using var enumerator = source.GetEnumerator();
        return enumerator.MoveNext() ? Option.Success(enumerator.Current) : default;
    }

    public static IEnumerable<T> WhereSuccess<T>(this IEnumerable<Option<T>> source)
        => source.Where(static option => option._hasValue).Select(static option => option._value);
    public static IEnumerable<Option<TResult>> Select<T, TResult>(this IEnumerable<Option<T>> source, Func<T, TResult> map)
        => source.Select(option => option.Map(map));
}
