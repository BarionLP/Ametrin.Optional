using System.Collections;
using System.Collections.Frozen;
using System.Collections.Immutable;

namespace Ametrin.Optional.Test.Extensions;

public sealed class OptionLinqExtensionsTests
{
    [Test]
    [MethodDataSource(nameof(GetBags))]
    public async Task ValuesIntoOrFirstError_Test(ICollection<Value> bag)
    {
        var option = GetResults().ValuesIntoOrFirstError(bag);

        await Assert.That(bag.Count).IsEqualTo(1);
        await Assert.That(option).IsError();
    }

    static IEnumerable<Result<Value, Error>> GetResults()
    {
        yield return new Value();
        yield return new Error();
    }

    public static IEnumerable<Func<ICollection<Value>>> GetBags()
    {
        yield return () => [];
        yield return () => new List<Value>();
        yield return () => new HashSet<Value>();
        yield return () => ImmutableArray.CreateBuilder<Value>();
        yield return () => ImmutableArray.CreateBuilder<Value>();
        yield return () => ImmutableHashSet.CreateBuilder<Value>();
    }
    public sealed class Error;
    public sealed class Value;
}
