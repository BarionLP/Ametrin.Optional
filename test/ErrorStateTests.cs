using TUnit.Assertions.Extensions.Generic;

namespace Ametrin.Optional.Test;

public sealed class ErrorStateTests
{
    [Test]
    public async Task Equals()
    {
        await Assert.That(ErrorState.Fail()).IsEqualTo(false);
        await Assert.That(ErrorState.Fail()).IsNotEqualTo(true);
        await Assert.That(ErrorState.Success()).IsEqualTo(true);
        await Assert.That(ErrorState.Success()).IsNotEqualTo(false);
        await Assert.That(ErrorState.Fail() == ErrorState.Fail()).IsTrue();
        await Assert.That(ErrorState.Success() != ErrorState.Fail()).IsTrue();
    }

    [Test]
    public async Task HashCode()
    {
        await NumberIsExtensions.IsEqualTo(Assert.That(ErrorState.Fail().GetHashCode()), ErrorState.Fail().GetHashCode());
        await NumberIsNotExtensions.IsNotEqualTo(Assert.That(ErrorState.Fail().GetHashCode()), ErrorState.Success().GetHashCode());
    }

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
        await NumberIsExtensions.IsEqualTo(Assert.That(a.GetHashCode()), b.GetHashCode());
    }

    [Test]
    [MethodDataSource(typeof(ErrorStateTestDataSource), nameof(ErrorStateTestDataSource.NotEqualsTestData))]
    public async Task HashCode_Not_Equals(ErrorState<int> a, ErrorState<int> b)
    {
        await NumberIsNotExtensions.IsNotEqualTo(Assert.That(a.GetHashCode()), b.GetHashCode());
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