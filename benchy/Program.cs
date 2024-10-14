using Ametrin.Optional;
using Ametrin.Optional.Benchy;
using BenchmarkDotNet.Running;


Option<int> lolo = Option.None<int>();
Result<int> lolr = Result.Fail<int>("Error");
Result<int, Exception> lole = Result.Fail<int, Exception>(new Exception());

Option<int> lolo2 = default;
Result<int> lolrd = default;
Result<int> lolr2 = "";
Result<int, Exception> lole2 = new Exception();

BenchmarkRunner.Run<Benchmarks>();