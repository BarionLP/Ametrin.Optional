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
        await Assert.That(Option.Fail().GetHashCode()).IsEqualTo(Option.Fail().GetHashCode());
        await Assert.That(Option.Fail().GetHashCode()).IsNotEqualTo(Option.Success().GetHashCode());
    }
}
