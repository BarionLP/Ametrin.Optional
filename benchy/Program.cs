using Ametrin.Optional;
using Ametrin.Optional.Benchy;
using Ametrin.Optional.Benchy.Examples;
using BenchmarkDotNet.Running;

var option = RefOption.Success(Span<byte>.Empty);
var other = option.Reject(span => span.IsEmpty).Map(span => Convert.ToHexString(span));
option.Or([]);

// var lol = Option.Success(new FileInfo("lol.txt"));
// var content = await lol.SelectAsync(async file => await File.ReadAllTextAsync(file.FullName));

// Console.WriteLine(content);

BenchmarkRunner.Run<TestBenchmarks>();
