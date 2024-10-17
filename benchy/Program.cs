using Ametrin.Optional;
using Ametrin.Optional.Benchy;
using BenchmarkDotNet.Running;

ErrorState<int> a = 1;
ErrorState<int> b = 2;

Console.WriteLine(a.Equals(b));