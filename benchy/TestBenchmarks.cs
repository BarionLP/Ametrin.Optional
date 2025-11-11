using BenchmarkDotNet.Attributes;

namespace Ametrin.Optional.Benchy;

[MemoryDiagnoser(false)]
public class TestBenchmarks
{
    [Params(.1f, .5f)]
    public float ErrorRate;

    [Params(500)]
    public int Count;

    private Result<int>[] array = [];
    private List<Result<int>> list = [];

    [GlobalSetup]
    public void Setup()
    {
        var random = new Random(69);
        array = [.. Enumerable.Range(0, Count).Select(_ => random.NextSingle() > ErrorRate ? Result.Success(random.Next()) : Result.Error<int>())];
        list = [.. array];
    }

    [Benchmark]
    public void Split_Array()
    {
        var r = array.Branch();
    }

    [Benchmark]
    public void Split_List()
    {
        var r = list.Branch();
    }
}

// | Method      | ErrorRate | Count | Mean       | Error    | StdDev    | Median     | Allocated |
// |------------ |---------- |------ |-----------:|---------:|----------:|-----------:|----------:|
// | Split_Array | 0         | 100   |   134.6 ns |  0.95 ns |   0.89 ns |   134.6 ns |     624 B |
// | Split_List  | 0         | 100   |   581.5 ns |  3.32 ns |   2.94 ns |   580.5 ns |     640 B |
// | Split_Array | 0         | 1000  | 1,299.7 ns |  6.21 ns |   5.81 ns | 1,297.8 ns |    4944 B |
// | Split_List  | 0         | 1000  | 5,632.6 ns | 23.29 ns |  21.79 ns | 5,634.6 ns |    4960 B |
// | Split_Array | 0.1       | 100   |   169.7 ns |  1.31 ns |   1.16 ns |   169.7 ns |     808 B |
// | Split_List  | 0.1       | 100   |   606.3 ns |  2.76 ns |   2.58 ns |   606.2 ns |     824 B |
// | Split_Array | 0.1       | 1000  | 1,462.7 ns | 11.68 ns |  10.35 ns | 1,464.1 ns |    6568 B |
// | Split_List  | 0.1       | 1000  | 5,819.1 ns | 24.95 ns |  22.12 ns | 5,811.4 ns |    6584 B |
// | Split_Array | 0.2       | 100   |   168.3 ns |  0.98 ns |   0.91 ns |   168.3 ns |     808 B |
// | Split_List  | 0.2       | 100   |   607.0 ns |  2.07 ns |   1.83 ns |   606.8 ns |     824 B |
// | Split_Array | 0.2       | 1000  | 1,507.0 ns | 24.71 ns |  23.12 ns | 1,505.8 ns |    6568 B |
// | Split_List  | 0.2       | 1000  | 5,905.5 ns | 19.03 ns |  17.80 ns | 5,903.0 ns |    6584 B |
// | Split_Array | 0.5       | 100   |   229.5 ns |  0.73 ns |   0.68 ns |   229.4 ns |    1816 B |
// | Split_List  | 0.5       | 100   |   671.7 ns |  1.76 ns |   1.65 ns |   672.1 ns |    1832 B |
// | Split_Array | 0.5       | 1000  | 2,081.3 ns | 72.14 ns | 210.42 ns | 1,992.3 ns |   16216 B |
// | Split_List  | 0.5       | 1000  | 6,421.6 ns | 28.57 ns |  25.32 ns | 6,420.8 ns |   16232 B |