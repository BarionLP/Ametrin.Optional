using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;
using System.Diagnostics;
using static Ametrin.Optional.Analyzer.Helper;

namespace Ametrin.Optional.Analyzer;

[DiagnosticAnalyzer(LanguageNames.CSharp)]
public sealed class AmetrinOptionalAnalyzer : DiagnosticAnalyzer
{

    public static readonly DiagnosticDescriptor InvalidConverter
        = new(id: "Option001", title: "Invalid Converter", messageFormat: "Invalid Converter, all converters have to implement ISerializationConverter", category: "Usage", defaultSeverity: DiagnosticSeverity.Error, isEnabledByDefault: true);

    public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics { get; } = [InvalidConverter];

    public override void Initialize(AnalysisContext context)
    {
        context.EnableConcurrentExecution();
        context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.Analyze | GeneratedCodeAnalysisFlags.ReportDiagnostics);

        context.RegisterSymbolAction(static context =>
        {
            context.ReportDiagnostic(Diagnostic.Create(InvalidConverter, context.Symbol.Locations[0]));
        }, SymbolKind.Field, SymbolKind.Property);
    }
}

internal static class SymbolExtensions
{
    public static bool HasAttribute(this ISymbol symbol, Func<INamedTypeSymbol, bool> condition)
        => symbol.GetAttributes().Any(attributeData => attributeData.AttributeClass is not null && condition(attributeData.AttributeClass));
    public static AttributeData? GetAttribute(this ISymbol symbol, Func<INamedTypeSymbol, bool> condition)
        => symbol.GetAttributes().FirstOrDefault(attributeData => attributeData.AttributeClass is not null && condition(attributeData.AttributeClass));
}

internal static class Helper
{
    internal static bool IsSerializablePropertyType(ITypeSymbol typeSymbol)
    {
        return IsTypeSupportedByWriter(typeSymbol) || typeSymbol.TypeKind is TypeKind.Enum || typeSymbol.HasAttribute(IsGenerateSerializerAttribute);
    }

    internal static bool IsTypeSupportedByWriter(ITypeSymbol type) => type.SpecialType is SpecialType.System_String or SpecialType.System_Int32 or SpecialType.System_Single or SpecialType.System_Double or SpecialType.System_Boolean or SpecialType.System_DateTime;

    internal static bool IsGenerateSerializerAttribute(INamedTypeSymbol attribute) => attribute is { Name: "GenerateSerializerAttribute", ContainingAssembly.Name: "Ametrin.Serializer" };
    internal static bool IsSerializeAttribute(INamedTypeSymbol attribute) => attribute is { Name: "SerializeAttribute", ContainingAssembly.Name: "Ametrin.Serializer" };
    internal static bool IsSerializationConverter(ITypeSymbol type) => type.AllInterfaces.Any(i => i is { Name: "ISerializationConverter" });
    internal static ITypeSymbol GetMemberType(ISymbol member) => member switch
    {
        IPropertySymbol property => property.Type,
        IFieldSymbol property => property.Type,
        _ => throw new InvalidOperationException($"Tried to deserialize a {member}"),
    };
}