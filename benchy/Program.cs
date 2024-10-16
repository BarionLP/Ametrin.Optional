using Ametrin.Optional;
using Ametrin.Optional.Benchy;
using BenchmarkDotNet.Running;

Option<int> lolo1 = 1;
Result<int> lolr1 = 1;
Result<int, Exception> lole1 = 1;

Option<int> lolo = Option.None<int>();
Result<int> lolr = Result.Fail<int>("Error");
Result<int, Exception> lole = Result.Fail<int, Exception>(new Exception());

Option<int> lolo2 = default;
Result<int> lolr2 = "Error";
Result<int, Exception> lole2 = new Exception();

// Result<int> lolrd = default; // this is bad

(lolo, lolo2).Consume(
    (a, b) => Console.WriteLine($"{a} {b}"),
    () => Console.WriteLine("Fail")
);

BenchmarkRunner.Run<Benchmarks>();