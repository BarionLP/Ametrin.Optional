namespace Ametrin.Optional.Test.Extensions;

public sealed class TryIndexOfTests
{
    [Test]
    [Arguments("_", 0)]
    [Arguments(" _", 1)]
    [Arguments(" __", 1)]
    public async Task Char_Success(string? value, int result)
    {
        await Assert.That(value.TryIndexOf('_')).IsSuccess(result);
    }

    [Test]
    [Arguments(null)]
    [Arguments("")]
    [Arguments(" ")]
    [Arguments("hello")]
    public async Task Char_Error(string? value)
    {
        await Assert.That(value.TryIndexOf('_')).IsError();
    }

    [Test]
    [Arguments("hello", 0)]
    [Arguments(" hello", 1)]
    [Arguments("_*hello hello", 2)]
    public async Task Sequence_Success(string? value, int result)
    {
        await Assert.That(value.TryIndexOf("hello")).IsSuccess(result);
    }

    [Test]
    [Arguments(null)]
    [Arguments("")]
    [Arguments("hell")]
    [Arguments("hella")]
    public async Task Sequence_Error(string? value)
    {
        await Assert.That(value.TryIndexOf("hello")).IsError();
    }
}
