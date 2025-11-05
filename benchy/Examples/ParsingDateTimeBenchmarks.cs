using BenchmarkDotNet.Attributes;

namespace Ametrin.Optional.Benchy.Examples;

[MemoryDiagnoser(false)]
public class ParsingDateTimeBenchmarks
{
    const string Valid = "09/01/2025 16:16:32";
    const string Invalid = "dfhalskdjfhdf(/&)";
    [Benchmark]
    public DateTime Option_Success()
    {
        return DateTime.TryParse(Valid).Or(DateTime.MinValue);
    }

    [Benchmark]
    public DateTime Option_Error()
    {
        return DateTime.TryParse(Invalid).Or(DateTime.MinValue);
    }
    
    [Benchmark]
    public DateTime Default_Success()
    {
        if(DateTime.TryParse(Valid, out var result))
        {
            return result;
        }

        return DateTime.MinValue;    
    }

    [Benchmark]
    public DateTime Default_Error()
    {
        if(DateTime.TryParse(Invalid, out var result))
        {
            return result;
        }

        return DateTime.MinValue;
    }
}
