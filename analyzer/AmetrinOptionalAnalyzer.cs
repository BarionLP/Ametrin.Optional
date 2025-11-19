using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.CodeAnalysis.Operations;

// when testing locally delete the package cache: 
// ps: Remove -Item "$env:USERPROFILE\.nuget\packages\ametrin.optional.analyzer" -Recurse -Force -ErrorAction SilentlyContinue
// bash: rm -r ~/.nuget/packages/ametrin.optional.analyzer

namespace Ametrin.Optional.Analyzer;

[DiagnosticAnalyzer(LanguageNames.CSharp)]
public sealed class AmetrinOptionalAnalyzer : DiagnosticAnalyzer
{
    // AmOptional000 means experimental
    public static readonly DiagnosticDescriptor ImpossibleRequire
        = new(id: "AmOptional001", title: "Impossible Require call", messageFormat: "{0} can never be {1}. If a conversion exists use .Map to explicitly use it.", category: "Usage", DiagnosticSeverity.Error, isEnabledByDefault: true);

    public static readonly DiagnosticDescriptor UnnecessaryRequire
        = new(id: "AmOptional002", title: "Unnecessary Require call", messageFormat: "type already is {0}", category: "Usage", DiagnosticSeverity.Warning, isEnabledByDefault: true);

    public static readonly DiagnosticDescriptor WrongConditionalType
        = new(id: "AmOptional003", title: "Wrong conditional return type", messageFormat: "default means {0} instead of {1}", category: "Usage", DiagnosticSeverity.Info, isEnabledByDefault: true);

    public static readonly DiagnosticDescriptor ImpossibleAs
        = new(id: "AmOptional004", title: "Impossible As call", messageFormat: "Cannot safely up-cast {0} to {1}. Use Require<T>.", category: "Usage", DiagnosticSeverity.Error, isEnabledByDefault: true);

    public static readonly DiagnosticDescriptor UnnecessaryAs
        = new(id: "AmOptional005", title: "Unnecessary As call", messageFormat: "type already is {0}", category: "Usage", DiagnosticSeverity.Warning, isEnabledByDefault: true);

    public static readonly DiagnosticDescriptor UseAsForUpCast
        = new(id: "AmOptional006", title: "Use As for up-casts", messageFormat: "Use As instead of Require for save up-casts", category: "Usage", DiagnosticSeverity.Info, isEnabledByDefault: true);

    public static readonly DiagnosticDescriptor DontUseDefaultForResult
        = new(id: "AmOptional007", title: "Do not use default for Result", messageFormat: "Do not create Result using the default keyword", category: "Usage", DiagnosticSeverity.Warning, isEnabledByDefault: true);

    public static readonly DiagnosticDescriptor GenerateParsingRequirements
        = new(id: "AmOptional008", title: "GenerateParsingAttribute requirements", messageFormat: "GenerateParsingAttribute requires IOptionSpanParsable to be implemented", category: "Usage", DiagnosticSeverity.Error, isEnabledByDefault: true);

    public static readonly DiagnosticDescriptor EmptyConsume
        = new(id: "AmOptional009", title: "Empty Consume call", messageFormat: "Your Consume call does not do anything {0}", category: "Usage", DiagnosticSeverity.Warning, isEnabledByDefault: true);

    public static readonly DiagnosticDescriptor DontUseDefaultForOption
        = new(id: "AmOptional010", title: "Do not use default for Option", messageFormat: "Do not create Option using the default keyword, use false instead", category: "Usage", DiagnosticSeverity.Info, isEnabledByDefault: true);

    public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics { get; } = [
        ImpossibleRequire, UnnecessaryRequire,
        WrongConditionalType,
        ImpossibleAs, UnnecessaryAs, UseAsForUpCast,
        DontUseDefaultForResult, DontUseDefaultForOption,
        EmptyConsume,
        GenerateParsingRequirements,
    ];

    public override void Initialize(AnalysisContext context)
    {
        context.EnableConcurrentExecution();
        context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.Analyze | GeneratedCodeAnalysisFlags.ReportDiagnostics);

        context.RegisterOperationAction(static context =>
        {
            var invocation = (IInvocationOperation)context.Operation;
            var targetMethod = invocation.TargetMethod;

            if (IsRequireCastMethod(targetMethod))
            {
                var currentType = targetMethod.ContainingType.TypeArguments[0];
                var targetType = targetMethod.TypeArguments[0];

                var effectiveCurrent = currentType;
                var hasCurrentValueConstraint = false;
                var hasCurrentReferenceConstraint = false;
                var isCurrentGeneric = false;

                var effectiveTarget = targetType;
                var hasTargetValueConstraint = false;
                var hasTargetReferenceConstraint = false;
                var isTargetGeneric = false;

                if (effectiveCurrent is ITypeParameterSymbol typeParameter)
                {
                    if (typeParameter is { ConstraintTypes.Length: 0, HasReferenceTypeConstraint: false, HasValueTypeConstraint: false })
                    {
                        return; // the type could be anything
                    }

                    isCurrentGeneric = true;
                    hasCurrentValueConstraint = typeParameter.HasValueTypeConstraint;
                    hasCurrentReferenceConstraint = typeParameter.HasReferenceTypeConstraint;
                    if (typeParameter.ConstraintTypes.Length is 1)
                    {
                        effectiveCurrent = typeParameter.ConstraintTypes[0];
                    }
                }

                if (effectiveTarget is ITypeParameterSymbol typeParameter2)
                {
                    if (typeParameter2 is { ConstraintTypes.Length: 0, HasReferenceTypeConstraint: false, HasValueTypeConstraint: false })
                    {
                        return; // the type could be anything
                    }

                    isTargetGeneric = true;
                    hasTargetValueConstraint = typeParameter2.HasValueTypeConstraint;
                    hasTargetReferenceConstraint = typeParameter2.HasReferenceTypeConstraint;
                    if (typeParameter2.ConstraintTypes.Length is 1)
                    {
                        effectiveTarget = typeParameter2.ConstraintTypes[0];
                    }
                }

                if ((hasTargetReferenceConstraint && hasCurrentValueConstraint) ||
                    (hasTargetValueConstraint && hasCurrentReferenceConstraint) ||
                    (hasTargetReferenceConstraint && effectiveCurrent.IsValueType) ||
                    (hasTargetValueConstraint && effectiveCurrent.IsReferenceType) ||
                    (hasCurrentReferenceConstraint && effectiveTarget.IsValueType) ||
                    (hasCurrentValueConstraint && effectiveTarget.IsReferenceType)
                )
                {
                    context.ReportDiagnostic(Diagnostic.Create(ImpossibleRequire, invocation.Syntax.GetLocation(), currentType.ToDisplayString(), targetType.ToDisplayString()));
                    return;
                }

                if ((effectiveCurrent.IsValueType && hasTargetValueConstraint) ||
                    (effectiveCurrent.IsReferenceType && hasTargetReferenceConstraint) ||
                    (effectiveTarget.IsValueType && hasCurrentValueConstraint) ||
                    (effectiveTarget.IsReferenceType && hasCurrentReferenceConstraint)
                )
                {
                    return; // might work
                }

                var conversion = context.Compilation.ClassifyConversion(effectiveCurrent, effectiveTarget);
                if (conversion.IsIdentity)
                {
                    context.ReportDiagnostic(Diagnostic.Create(UnnecessaryRequire, invocation.Syntax.GetLocation(), targetType.ToDisplayString()));
                }
                else if (conversion.IsImplicit && !isCurrentGeneric && !isTargetGeneric)
                {
                    context.ReportDiagnostic(Diagnostic.Create(UseAsForUpCast, invocation.Syntax.GetLocation()));
                }
                else if (!conversion.Exists || conversion.IsUserDefined)
                {
                    // TODO: check more than one constraint
                    if (currentType is ITypeParameterSymbol { ConstraintTypes.Length: > 1 }) return;
                    if (targetType is ITypeParameterSymbol { ConstraintTypes.Length: > 1 }) return;
                    context.ReportDiagnostic(Diagnostic.Create(ImpossibleRequire, invocation.Syntax.GetLocation(), currentType.ToDisplayString(), targetType.ToDisplayString()));
                }
            }

            if (IsAsMethod(targetMethod))
            {
                var currentType = targetMethod.ContainingType.TypeArguments[0];
                var targetType = targetMethod.TypeArguments[0];

                var conversion = context.Compilation.ClassifyConversion(currentType, targetType);

                if (!conversion.IsImplicit)
                {
                    context.ReportDiagnostic(Diagnostic.Create(ImpossibleAs, invocation.Syntax.GetLocation(), currentType.ToDisplayString(), targetType.ToDisplayString()));
                }
                else if (conversion.IsIdentity)
                {
                    context.ReportDiagnostic(Diagnostic.Create(UnnecessaryAs, invocation.Syntax.GetLocation(), currentType.ToDisplayString()));
                }
            }

            if (IsConsumeMethod(targetMethod))
            {
                if (IsDirectConsumeMethod(targetMethod) && IsNull(invocation.Arguments[0].Value) && IsNull(invocation.Arguments[1].Value))
                {
                    context.ReportDiagnostic(Diagnostic.Create(EmptyConsume, invocation.Syntax.GetLocation(), invocation.Arguments[0].Value));
                }
                else if (IsDirectArgsConsumeMethod(targetMethod) && IsNull(invocation.Arguments[1].Value) && IsNull(invocation.Arguments[2].Value))
                {
                    context.ReportDiagnostic(Diagnostic.Create(EmptyConsume, invocation.Syntax.GetLocation(), invocation.Arguments[0]));
                }
                else if (IsExtensionConsumeMethod(targetMethod) && IsNull(invocation.Arguments[1].Value) && IsNull(invocation.Arguments[2].Value))
                {
                    context.ReportDiagnostic(Diagnostic.Create(EmptyConsume, invocation.Syntax.GetLocation(), invocation.Arguments[1].Value));
                }
                else if (IsExtensionArgsConsumeMethod(targetMethod) && IsNull(invocation.Arguments[2].Value) && IsNull(invocation.Arguments[3].Value))
                {
                    context.ReportDiagnostic(Diagnostic.Create(EmptyConsume, invocation.Syntax.GetLocation(), invocation.Arguments.Length));
                }
            }

        }, OperationKind.Invocation);

        context.RegisterOperationAction(static context =>
        {
            var conversion = (IConversionOperation)context.Operation;

            if (!conversion.IsImplicit) return;

            if (conversion.Type is not INamedTypeSymbol target || !IsOptionalType(target)) return;

            if (conversion.Operand is not IConditionalOperation condition || IsOptionalType(condition.Type!)) return;

            if (IsDefaultLiteral(condition.WhenTrue))
            {
                context.ReportDiagnostic(Diagnostic.Create(WrongConditionalType, condition.WhenTrue.Syntax.GetLocation(), condition.Type!.ToDisplayString(), conversion.Type.ToDisplayString()));
            }

            if (condition.WhenFalse is not null && IsDefaultLiteral(condition.WhenFalse))
            {
                context.ReportDiagnostic(Diagnostic.Create(WrongConditionalType, condition.WhenFalse.Syntax.GetLocation(), condition.Type!.ToDisplayString(), conversion.Type.ToDisplayString()));
            }

        }, OperationKind.Conversion);

        context.RegisterOperationAction(static context =>
        {
            var operation = (IDefaultValueOperation)context.Operation;

            if (IsResultType(operation.Type!))
            {
                context.ReportDiagnostic(Diagnostic.Create(DontUseDefaultForResult, operation.Syntax.GetLocation()));
            }

            if (IsNonGenericOptionType(operation.Type!))
            {
                context.ReportDiagnostic(Diagnostic.Create(DontUseDefaultForOption, operation.Syntax.GetLocation()));
            }
        }, OperationKind.DefaultValue);

        context.RegisterSymbolAction(static context =>
        {
            var type = (INamedTypeSymbol)context.Symbol;

            if (HasAttribute(type, IsGenerateISpanParsableAttribute) && !type.Interfaces.Any(IsIOptionSpanParsable))
            {
                context.ReportDiagnostic(Diagnostic.Create(GenerateParsingRequirements, type.Locations[0]));
            }
        }, SymbolKind.NamedType);
    }
}
