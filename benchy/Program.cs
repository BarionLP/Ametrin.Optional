using Ametrin.Optional;
using Ametrin.Optional.Benchy;
using Ametrin.Optional.Benchy.Examples;
using BenchmarkDotNet.Running;

var option = RefOption.Success(Span<byte>.Empty);
var other = option.WhereNot(span => span.IsEmpty).Map(span => Convert.ToHexString(span));
option.Or([]);

BenchmarkRunner.Run<ParsingDateTimeBenchmarks>();