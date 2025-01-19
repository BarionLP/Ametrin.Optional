using BenchmarkDotNet.Attributes;

namespace Ametrin.Optional.Benchy;

[MemoryDiagnoser(false)]
public class TestBenchmarks
{
    public static Task<string> faultedTask = Task.FromException<string>(new Exception());

    
    [Benchmark]
    public async Task<Exception> ConfigureAwait()
    {
        await ((Task)faultedTask).ConfigureAwait(ConfigureAwaitOptions.SuppressThrowing);
        return faultedTask.Exception!;
    }
    
    [Benchmark]
    public async Task<Exception> WhenAny()
    {
        await Task.WhenAny(faultedTask);
        return faultedTask.Exception!;
    }


    static Task ThrowingTask() => Task.Run(() => throw new Exception());
}
