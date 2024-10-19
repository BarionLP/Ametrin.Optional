namespace Ametrin.Optional.Test;

public sealed class ErrorStateTests
{
    [Test]
    [MethodDataSource(typeof(ErrorStateTestDataSource), nameof(ErrorStateTestDataSource.EqualsTestData))]
    public async Task Equals(ErrorState<int> a, ErrorState<int> b)
    {
        await Assert.That(a == b).IsTrue();
    }

    [Test]
    [MethodDataSource(typeof(ErrorStateTestDataSource), nameof(ErrorStateTestDataSource.NotEqualsTestData))]
    public async Task Not_Equals(ErrorState<int> a, ErrorState<int> b)
    {
        await Assert.That(a != b).IsTrue();
    }

    [Test]
    [MethodDataSource(typeof(ErrorStateTestDataSource), nameof(ErrorStateTestDataSource.EqualsTestData))]
    public async Task HashCode_Equals(ErrorState<int> a, ErrorState<int> b)
    {
        await Assert.That(a.GetHashCode()).IsEqualTo(b.GetHashCode());
    }

    [Test]
    [MethodDataSource(typeof(ErrorStateTestDataSource), nameof(ErrorStateTestDataSource.NotEqualsTestData))]
    public async Task HashCode_Not_Equals(ErrorState<int> a, ErrorState<int> b)
    {
        await Assert.That(a.GetHashCode()).IsNotEqualTo(b.GetHashCode());
    }
}

public static class ErrorStateTestDataSource
{
    public static IEnumerable<(ErrorState<int>, ErrorState<int>)> EqualsTestData()
    {
        yield return (ErrorState.Fail(1), ErrorState.Fail(1));
        yield return (ErrorState.Success<int>(), new());
    }

    public static IEnumerable<(ErrorState<int>, ErrorState<int>)> NotEqualsTestData()
    {
        yield return (ErrorState.Fail(1), ErrorState.Fail(2));
        yield return (ErrorState.Fail(0), ErrorState.Success<int>());
    }
}