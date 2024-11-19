namespace Ametrin.Optional.Test;

public sealed class OptionTests
{
    [Test]
    [MethodDataSource(typeof(OptionTestDataSource), nameof(OptionTestDataSource.EqualsTestData))]
    public async Task Equals(Option<int> a, Option<int> b)
    {
        await Assert.That(a == b).IsTrue();
    }

    [Test]
    [MethodDataSource(typeof(OptionTestDataSource), nameof(OptionTestDataSource.NotEqualsTestData))]
    public async Task Not_Equals(Option<int> a, Option<int> b)
    {
        await Assert.That(a != b).IsTrue();
    }

    [Test]
    [MethodDataSource(typeof(OptionTestDataSource), nameof(OptionTestDataSource.EqualsTestData))]
    public async Task HashCode_Equals(Option<int> a, Option<int> b)
    {
        await Assert.That(a.GetHashCode()).IsEqualTo(b.GetHashCode());
    }

    [Test]
    [MethodDataSource(typeof(OptionTestDataSource), nameof(OptionTestDataSource.NotEqualsTestData))]
    public async Task HashCode_Not_Equals(Option<int> a, Option<int> b)
    {
        await Assert.That(a.GetHashCode()).IsNotEqualTo(b.GetHashCode());
    }
}

public static class OptionTestDataSource
{
    public static IEnumerable<(Option<int>, Option<int>)> EqualsTestData()
    {
        yield return (Option.Of(1), Option.Of(1));
        yield return (Option.Error<int>(), default);
    }

    public static IEnumerable<(Option<int>, Option<int>)> NotEqualsTestData()
    {
        yield return (Option.Of(1), Option.Of(2));
        yield return (Option.Of(0), Option.Error<int>());
    }
}