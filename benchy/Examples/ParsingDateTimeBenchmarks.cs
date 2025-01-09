using BenchmarkDotNet.Attributes;

namespace Ametrin.Optional.Benchy.Examples;

[MemoryDiagnoser(false)]
public class ParsingDateTimeBenchmarks
{
    const string Valid = "09/01/2025 16:16:32";
    const string Invalid = "dfhalskdjfhdf";
    [Benchmark]
    public DateTime Option_Success()
    {
        return Option.Success(Valid).Parse<DateTime>().Or(DateTime.MaxValue);
    }

    [Benchmark]
    public DateTime Option_Error()
    {
        return Option.Success(Invalid).Parse<DateTime>().Or(DateTime.MaxValue);
    }
    
    [Benchmark]
    public DateTime RefOption_Success()
    {
        return RefOption.Success<ReadOnlySpan<char>>(Valid).Parse<DateTime>().Or(DateTime.MaxValue);
    }

    [Benchmark]
    public DateTime RefOption_Error()
    {
        return RefOption.Success<ReadOnlySpan<char>>(Invalid).Parse<DateTime>().Or(DateTime.MaxValue);
    }
    
    [Benchmark]
    public DateTime Default_Success()
    {
        if(DateTime.TryParse(Valid, out var result))
        {
            return result;
        }

        return DateTime.MaxValue;    
    }

    [Benchmark]
    public DateTime Default_Error()
    {
        if(DateTime.TryParse(Invalid, out var result))
        {
            return result;
        }

        return DateTime.MaxValue;
    }
}
