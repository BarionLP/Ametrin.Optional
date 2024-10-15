using Ametrin.Optional;
using Ametrin.Optional.Benchy;
using BenchmarkDotNet.Running;

Option<int> lolo1 = 1;
Result<int> lolr1 = 1;
Result<int, Exception> lole1 = 1;

Option<int> lolo = Option.None<int>();
Result<int> lolr = Result.Fail<int>();
Result<int, Exception> lole = Result.Fail<int, Exception>(new Exception());

Option<int> lolo2 = default;
Result<int> lolrd = default; //this is bad
Result<int> lolr2 = null;
Result<int, Exception> lole2 = new Exception();

(lolo, lolo2).Consume(
    (a, b) => Console.WriteLine($"{a} {b}"),
    () => Console.WriteLine("Fail")
);

BenchmarkRunner.Run<Benchmarks>();