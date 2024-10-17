using TUnit.Assertions.Extensions.Generic;

namespace Ametrin.Optional.Test;

public class OptionTests
{
    [Test]
    public async Task Equals()
    {
        await Assert.That(Option.Fail()).IsEqualTo(false);
        await Assert.That(Option.Fail()).IsNotEqualTo(true);
        await Assert.That(Option.Success()).IsEqualTo(true);
        await Assert.That(Option.Success()).IsNotEqualTo(false);
        await Assert.That(Option.Fail() == Option.Fail()).IsTrue();
        await Assert.That(Option.Success() != Option.Fail()).IsTrue();
    }

    [Test]
    public async Task HashCode()
    {
        await NumberIsExtensions.IsEqualTo(Assert.That(Option.Fail().GetHashCode()), Option.Fail().GetHashCode());
        await NumberIsNotExtensions.IsNotEqualTo(Assert.That(Option.Fail().GetHashCode()), Option.Success().GetHashCode());
    }
}
