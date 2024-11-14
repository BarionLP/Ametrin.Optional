using System.Linq;

namespace Ametrin.Optional;

public static class LinqExtensions
{
    public static Option<IEnumerable<T>> WhereNotEmpty<T>(this IEnumerable<T> source)
        => source is not null && source.Any() ? Option.Success(source) : default;
    public static Option<IEnumerable<T>> WhereNotEmpty<T>(this Option<IEnumerable<T>> option)
        => option.Where(static collection => collection.Any());
    public static Result<IEnumerable<T>> WhereNotEmpty<T>(this Result<IEnumerable<T>> option)
        => option.WhereNotEmpty(static value => new ArgumentException("Sequence was empty"));
    public static Result<IEnumerable<T>> WhereNotEmpty<T>(this Result<IEnumerable<T>> option, Func<IEnumerable<T>, Exception> error)
        => option.Where(static collection => collection.Any(), error);

    public static Option<T> FirstOrNone<T>(this IEnumerable<T> source)
        => source.Any() ? Option.Success(source.First()) : default;

    public static IEnumerable<T> WhereSuccess<T>(this IEnumerable<Option<T>> source)
        => source.Where(static option => option._hasValue).Select(static option => option._value);
    public static IEnumerable<Option<TResult>> Select<T, TResult>(this IEnumerable<Option<T>> source, Func<T, TResult> selector)
        => source.Select(option => option.Select(selector));
}
