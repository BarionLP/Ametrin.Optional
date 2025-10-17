using System.Collections.Generic;
using System.Linq;

namespace Ametrin.Optional;

public static class OptionLinqExtensions
{
    // any already tries to get the non-enumerated count before calling the enumerator
    public static Option<IEnumerable<T>> RejectEmpty<T>(this IEnumerable<T> source)
        => source is not null && source.Any() ? Option.Success(source) : default;
    public static Option<IEnumerable<T>> RejectEmpty<T>(this Option<IEnumerable<T>> option)
        => option.Require(Enumerable.Any);
    public static Result<IEnumerable<T>> RejectEmpty<T>(this Result<IEnumerable<T>> option)
        => option.RejectEmpty(static source => new ArgumentException("Sequence was empty"));
    public static Result<IEnumerable<T>> RejectEmpty<T>(this Result<IEnumerable<T>> option, Func<IEnumerable<T>, Exception> error)
        => option.Require(Enumerable.Any, error);

    public static Option<T> FirstOrError<T>(this IEnumerable<T> source)
    {
        using var enumerator = source.GetEnumerator();
        return enumerator.MoveNext() ? Option.Success(enumerator.Current) : default;
    }

    public static IEnumerable<T> WhereSuccess<T>(this IEnumerable<Option<T>> source)
        => source.Where(static option => option._hasValue).Select(static option => option._value);
    public static IEnumerable<T> WhereSuccess<T>(this IEnumerable<Result<T>> source)
        => source.Where(static option => option._hasValue).Select(static option => option._value);
    public static IEnumerable<T> WhereSuccess<T, E>(this IEnumerable<Result<T, E>> source)
        => source.Where(static option => option._hasValue).Select(static option => option._value);

    public static IEnumerable<Exception> WhereError<T>(this IEnumerable<Result<T>> source)
        => source.Where(static option => !option._hasValue).Select(static option => option._error);
    public static IEnumerable<E> WhereError<T, E>(this IEnumerable<Result<T, E>> source)
        => source.Where(static option => !option._hasValue).Select(static option => option._error);
    public static IEnumerable<Exception> WhereError<T>(this IEnumerable<ErrorState> source)
        => source.Where(static option => option._isError).Select(static option => option._error);
    public static IEnumerable<E> WhereError<T, E>(this IEnumerable<ErrorState<E>> source)
        => source.Where(static option => option._isError).Select(static option => option._error);

    public static Result<IReadOnlyList<T>> ValuesOrFirstError<T>(this IEnumerable<Result<T>> results)
    {
        var count = results.TryGetNonEnumeratedCount(out var c) ? c : -1;
        if (count is 0) return Result.Success<IReadOnlyList<T>>([]);
        var values = CreateBag<T>(count);

        foreach (var result in results)
        {
            if (result.Branch(out var value, out var error))
            {
                values.Add(value);
            }
            else
            {
                values.Clear();
                return error;
            }
        }

        return values;
    }

    public static void Split<T>(this IEnumerable<Result<T>> results, IList<T> values, IList<Exception> errors)
    {
        foreach (var result in results)
        {
            if (result.Branch(out var value, out var error))
            {
                values.Add(value);
            }
            else
            {
                errors.Add(error);
            }
        }
    }

    public static void Split<T, E>(this IEnumerable<Result<T, E>> results, IList<T> values, IList<E> errors)
    {
        foreach (var result in results)
        {
            if (result.Branch(out var value, out var error))
            {
                values.Add(value);
            }
            else
            {
                errors.Add(error);
            }
        }
    }

    public static (IReadOnlyList<T> values, IReadOnlyList<Exception> errors) Split<T>(this IEnumerable<Result<T>> results)
    {
        ArgumentNullException.ThrowIfNull(results);

        var count = results.TryGetNonEnumeratedCount(out var c) ? c : -1;
        if (count is 0) return ([], []);

        var (values, errors) = CreateBags<T, Exception>(count);
        results.Split(values, errors);

        return (values, errors);
    }

    public static (IReadOnlyList<T> values, IReadOnlyList<E> errors) Split<T, E>(this IEnumerable<Result<T, E>> results)
    {
        ArgumentNullException.ThrowIfNull(results);

        var count = results.TryGetNonEnumeratedCount(out var c) ? c : -1;
        if (count is 0) return ([], []);

        var (values, errors) = CreateBags<T, E>(count);
        results.Split(values, errors);

        return (values, errors);
    }

    private static (List<T> values, List<E> errors) CreateBags<T, E>(int count, double expectedErrorRate = 0.01)
    {
        // we assume most of the incoming values will be successes so we preallocate the full size
        var values = count > 0 ? new List<T>(capacity: count) : [];
        // in most cases only a fraction of values will be errors (this has not been benchmarked yet! first me must figure out what a common error rate is)
        var errors = count > 0 ? new List<E>(capacity: (int)double.Round(count * expectedErrorRate)) : [];

        return (values, errors);
    }

    private static List<T> CreateBag<T>(int count)
    {
        // we assume most of the incoming values will be successes so we preallocate the full size
        return count > 0 ? new List<T>(capacity: count) : [];
    }

    [Obsolete("use Select(option => option.Map(map)). this method made things confusing")]
    public static IEnumerable<Option<TResult>> Select<T, TResult>(this IEnumerable<Option<T>> source, Func<T, TResult> map)
        => source.Select(option => option.Map(map));
}
