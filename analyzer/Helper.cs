using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Operations;

namespace Ametrin.Optional.Analyzer;

internal static class Helper
{
    internal static bool IsRequireCastMethod(IMethodSymbol method) => method is { Name: "Require", ContainingType: { Name: "Option" or "Result", ContainingAssembly.Name: "Ametrin.Optional" }, Parameters.Length: 0 or 1, IsGenericMethod: true };
    internal static bool IsAsMethod(IMethodSymbol method) => method is { Name: "As", ContainingType: { Name: "Option" or "Result", ContainingAssembly.Name: "Ametrin.Optional" }, Parameters.Length: 0, IsGenericMethod: true };
    internal static bool IsConsumeMethod(IMethodSymbol method) => method is { Name: "Consume", ContainingType.ContainingAssembly.Name: "Ametrin.Optional", Parameters.Length: 2 or 3 or 4 };
    internal static bool IsDirectConsumeMethod(IMethodSymbol method) => method is { Name: "Consume", ContainingType: { Name: "Option" or "Result" or "ErrorState" or "RefOption", ContainingAssembly.Name: "Ametrin.Optional" }, Parameters.Length: 2 };
    internal static bool IsDirectArgsConsumeMethod(IMethodSymbol method) => method is { Name: "Consume", ContainingType: { Name: "Option" or "Result" or "ErrorState" or "RefOption", ContainingAssembly.Name: "Ametrin.Optional" }, Parameters.Length: 3 };
    internal static bool IsExtensionConsumeMethod(IMethodSymbol method) => method is { Name: "Consume", ContainingType.ContainingAssembly.Name: "Ametrin.Optional", Parameters.Length: 3, IsExtensionMethod: true };
    internal static bool IsExtensionArgsConsumeMethod(IMethodSymbol method) => method is { Name: "Consume", ContainingType.ContainingAssembly.Name: "Ametrin.Optional", Parameters.Length: 4, IsExtensionMethod: true };
    internal static bool IsOptionalType(ITypeSymbol type) => type is { Name: "Option" or "Result" or "ErrorState" or "RefOption", ContainingAssembly.Name: "Ametrin.Optional" };
    internal static bool IsResultType(ITypeSymbol type) => type is { Name: "Result", ContainingAssembly.Name: "Ametrin.Optional" };
    internal static bool IsDefaultLiteral(IOperation operation) => operation.Syntax.IsKind(SyntaxKind.DefaultLiteralExpression);
    internal static bool HasAttribute(ISymbol symbol, Func<INamedTypeSymbol?, bool> predicate) => symbol.GetAttributes().Any(data => predicate(data.AttributeClass));
    internal static bool IsGenerateISpanParsableAttribute(INamedTypeSymbol? attribute) => attribute is { Name: "GenerateISpanParsableAttribute", ContainingAssembly.Name: "Ametrin.Optional" };
    internal static bool IsIOptionSpanParsable(INamedTypeSymbol? attribute) => attribute is { Name: "IOptionSpanParsable", TypeArguments.Length: 1, ContainingAssembly.Name: "Ametrin.Optional" };
    internal static bool IsNull(IOperation? argument) => (argument is IConversionOperation co && IsNull(co.Operand)) || argument is IDefaultValueOperation { ConstantValue: { HasValue: true, Value: null } } or ILiteralOperation { ConstantValue: { HasValue: true, Value: null } };
}