using Ametrin.Optional;
using Ametrin.Optional.Benchy;
using BenchmarkDotNet.Running;

RefOption<Span<byte>> option = RefOption.Success(Span<byte>.Empty);
option.WhereNot(span => span.IsEmpty).Select(span => Convert.ToHexString(span));

BenchmarkRunner.Run<Benchmarks>();