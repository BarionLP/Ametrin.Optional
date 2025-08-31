using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace Ametrin.Optional;

public static class OptionLinqExtensions
{
    public static Option<IEnumerable<T>> RejectEmpty<T>(this IEnumerable<T> source)
        => source is not null && source.Any() ? Option.Success(source) : default;
    public static Option<IEnumerable<T>> RejectEmpty<T>(this Option<IEnumerable<T>> option)
        => option.Require(static collection => collection.Any());
    public static Result<IEnumerable<T>> RejectEmpty<T>(this Result<IEnumerable<T>> option)
        => option.RejectEmpty(static value => new ArgumentException("Sequence was empty"));
    public static Result<IEnumerable<T>> RejectEmpty<T>(this Result<IEnumerable<T>> option, Func<IEnumerable<T>, Exception> error)
        => option.Require(static collection => collection.Any(), error);

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

    [Experimental("AO001")]
    public static (IReadOnlyList<T> values, IReadOnlyList<Exception> errors) Split<T>(this IEnumerable<Result<T>> results)
    {
        ArgumentNullException.ThrowIfNull(results);

        var count =  results.TryGetNonEnumeratedCount(out var c) ? c : -1;
        if (count is 0) return ([], []);

        // we assume most of the incoming values will be success so we preallocate the full bucket
        var values = count > 0 ? new List<T>(capacity: count) : [];
        // in most cases only a fraction of values are errors (this has not been benchmarked yet! first me must figure out what a common error rate is)
        var errors = count > 0 ? new List<Exception>(capacity: count / 10) : [];

        foreach (var result in results)
        {
            if (OptionsMarshall.TryGetError(result, out var e))
            {
                errors.Add(e);
            }
            else
            {
                values.Add(result.OrThrow());
            }
        }

        return (values, errors);
    }

    [Experimental("AO001")]
    public static (IReadOnlyList<T> values, IReadOnlyList<E> errors) Split<T, E>(this IEnumerable<Result<T, E>> results)
    {
        ArgumentNullException.ThrowIfNull(results);

        var count = results.TryGetNonEnumeratedCount(out var c) ? c : -1;
        if (count is 0) return ([], []);

        // we assume most of the incoming values will be success so we preallocate the full bucket
        var values = count > 0 ? new List<T>(capacity: count) : [];
        // in most cases only a fraction of values are errors (this has not been benchmarked yet! first me must figure out what a common error rate is)
        var errors = count > 0 ? new List<E>(capacity: count / 10) : [];

        foreach (var result in results)
        {
            // TODO: use Branch
            if (OptionsMarshall.TryGetError(result, out var e))
            {
                errors.Add(e);
            }
            else
            {
                values.Add(result.OrThrow());
            }
        }

        return (values, errors);
    }

    [Obsolete("use Select(option => option.Map). this methods made things confusing")]
    public static IEnumerable<Option<TResult>> Select<T, TResult>(this IEnumerable<Option<T>> source, Func<T, TResult> map)
        => source.Select(option => option.Map(map));
}
