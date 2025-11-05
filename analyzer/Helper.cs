using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace Ametrin.Optional.Analyzer;

internal static class Helper
{
    internal static bool IsRequireCastMethod(IMethodSymbol method) => method is { Name: "Require", ContainingType: { Name: "Option" or "Result", ContainingAssembly.Name: "Ametrin.Optional" }, Parameters.Length: 0 or 1, IsGenericMethod: true };
    internal static bool IsAsMethod(IMethodSymbol method) => method is { Name: "As", ContainingType: { Name: "Option" or "Result", ContainingAssembly.Name: "Ametrin.Optional" }, Parameters.Length: 0, IsGenericMethod: true };
    internal static bool IsOptionalType(ITypeSymbol type) => type is { Name: "Option" or "Result" or "ErrorState" or "RefOption", ContainingAssembly.Name: "Ametrin.Optional" };
    internal static bool IsResultType(ITypeSymbol type) => type is { Name: "Result", ContainingAssembly.Name: "Ametrin.Optional" };
    internal static bool IsDefaultLiteral(IOperation operation) => operation.Syntax.IsKind(SyntaxKind.DefaultLiteralExpression);
    internal static bool HasAttribute(ISymbol symbol, Func<INamedTypeSymbol?, bool> predicate) => symbol.GetAttributes().Any(data => predicate(data.AttributeClass));
    internal static bool IsGenerateParsingAttribute(INamedTypeSymbol? attribute) => attribute is { Name: "GenerateParsingAttribute", ContainingAssembly.Name: "Ametrin.Optional" };
    internal static bool IsIOptionSpanParsable(INamedTypeSymbol? attribute) => attribute is { Name: "IOptionSpanParsable", TypeArguments.Length: 1, ContainingAssembly.Name: "Ametrin.Optional" };
}