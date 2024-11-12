namespace Ametrin.Optional.Test;

public class OptionTests
{
    [Test]
    public async Task Equals()
    {
        await Assert.That(Option.Error()).IsEqualTo(false);
        await Assert.That(Option.Error()).IsNotEqualTo(true);
        await Assert.That(Option.Success()).IsEqualTo(true);
        await Assert.That(Option.Success()).IsNotEqualTo(false);
        await Assert.That(Option.Error() == Option.Error()).IsTrue();
        await Assert.That(Option.Success() != Option.Error()).IsTrue();
    }

    [Test]
    public async Task HashCode()
    {
        await Assert.That(Option.Error().GetHashCode()).IsEqualTo(Option.Error().GetHashCode());
        await Assert.That(Option.Error().GetHashCode()).IsNotEqualTo(Option.Success().GetHashCode());
    }
}
