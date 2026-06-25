using System.Collections.Immutable;
using System.Linq;

namespace Ametrin.Optional.Test.Extensions;

public sealed class OptionLinqExtensionsTests
{
    [Test]
    [NotInParallel]
    [MatrixDataSource]
    public async Task ValuesIntoOrFirstError_Test([MatrixMethod<OptionLinqExtensionsTests>(nameof(GetCollections))] IEnumerable<Result<Value, Error>> source, [MatrixMethod<OptionLinqExtensionsTests>(nameof(GetBags))] ICollection<Value> bag)
    {
        var startCount = bag.Count; // bags are reused
        var option = source.ValuesIntoOrFirstError(bag);

        await Assert.That(bag.Count).IsEqualTo(startCount + 1);
        await Assert.That(option).IsError();
    }

    static IEnumerable<Result<Value, Error>> GetResults()
    {
        yield return new Value();
        yield return new Error();
    }

    public static IEnumerable<ICollection<Value>> GetBags()
    {
        yield return [];
        yield return new List<Value>();
        yield return new HashSet<Value>();
        yield return ImmutableArray.CreateBuilder<Value>();
        yield return ImmutableArray.CreateBuilder<Value>();
        yield return ImmutableHashSet.CreateBuilder<Value>();
    }
    public static IEnumerable<IEnumerable<Result<Value, Error>>> GetCollections()
    {
        yield return GetResults();
        yield return GetResults().ToList();
        yield return GetResults().ToArray();
        yield return GetResults().ToImmutableArray();
        yield return new ArraySegment<Result<Value, Error>>([.. GetResults()]);
    }

    public sealed class Error;
    public sealed class Value;
}
