using Microsoft.CodeAnalysis;

namespace Ametrin.Optional.Analyzer;

internal static class Helper
{
    internal static bool IsRequireCastMethod(IMethodSymbol method) => method is { Name: "Require", ContainingType: { Name: "Option" or "Result", ContainingAssembly.Name: "Ametrin.Optional" }, Parameters.Length: 0 or 1, IsGenericMethod: true };
}