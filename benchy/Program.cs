using Ametrin.Optional;
using Ametrin.Optional.Benchy;
using BenchmarkDotNet.Running;

"Hallo".ParseOrNone<UInt128>().Consume(
    error: () => Console.WriteLine("Failure"),
    success: s => Console.WriteLine(s)
);