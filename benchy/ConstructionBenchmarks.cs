using BenchmarkDotNet.Attributes;

namespace Ametrin.Optional.Benchy;

[MemoryDiagnoser(false)]
public class ConstructionBenchmarks
{
    internal Option<string> Some = "Hello World";
    internal Option<string> None = default;
    internal static Exception PreAllocatedException = new();

    [GlobalSetup]
    public void Setup()
    {

    }

    // [Benchmark] public Option<int> CreateValueOption_Some() => 1;
    // [Benchmark] public Option<int> CreateValueOption_None() => default;
    // [Benchmark] public Option<string> CreateReferenceOption_Some() => "";
    // [Benchmark] public Option<string> CreateReferenceOption_None() => default;

    // [Benchmark] public Option<string> Option_Some_Select() => Some.Select(val => val + "The cake is a lie");
    // [Benchmark] public Option<string> Option_None_Select() => None.Select(val => val + "The cake is a lie");

    [Benchmark] public Result<int> CreateValueResult_Some() => 1;
    [Benchmark] public Result<int> CreateValueResult_None() => PreAllocatedException;
    [Benchmark] public Result<string> CreateReferenceResult_Some() => "";
    [Benchmark] public Result<string> CreateReferenceResult_None() => PreAllocatedException;
}
