using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.CodeAnalysis.Operations;
using static Ametrin.Optional.Analyzer.Helper;

namespace Ametrin.Optional.Analyzer;

[DiagnosticAnalyzer(LanguageNames.CSharp)]
public sealed class AmetrinOptionalAnalyzer : DiagnosticAnalyzer
{

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

    public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics { get; } = [ImpossibleRequire, UnnecessaryRequire, WrongConditionalType, ImpossibleAs, UnnecessaryAs, UseAsForUpCast, DontUseDefaultForResult];

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

                var conversion = context.Compilation.ClassifyConversion(currentType, targetType);
                if (conversion.IsIdentity)
                {
                    context.ReportDiagnostic(Diagnostic.Create(UnnecessaryRequire, invocation.Syntax.GetLocation(), targetType.ToDisplayString()));
                }
                else if (conversion.IsImplicit)
                {
                    context.ReportDiagnostic(Diagnostic.Create(UseAsForUpCast, invocation.Syntax.GetLocation()));
                }
                else if (!conversion.Exists || conversion.IsUserDefined)
                {
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

        }, OperationKind.Invocation);

        context.RegisterOperationAction(static context =>
        {
            var conversion = (IConversionOperation)context.Operation;

            if (!conversion.IsImplicit) return;

            if (conversion.Type is not INamedTypeSymbol target || !IsOptionalType(target)) return;

            if (conversion.Operand is not IConditionalOperation condition || IsOptionalType(condition.Type!)) return;

            var hasDefault = IsDefaultLiteral(condition.WhenTrue) || IsDefaultLiteral(condition.WhenFalse!);
            if (!hasDefault) return;

            context.ReportDiagnostic(Diagnostic.Create(WrongConditionalType, condition.Syntax.GetLocation(), condition.Type!.ToDisplayString(), conversion.Type.ToDisplayString()));

        }, OperationKind.Conversion);

        context.RegisterOperationAction(static context =>
        {
            var operation = (IDefaultValueOperation)context.Operation;

            if (IsResultType(operation.Type!))
            {
                context.ReportDiagnostic(Diagnostic.Create(DontUseDefaultForResult, operation.Syntax.GetLocation()));
            }
        }, OperationKind.DefaultValue);
    }
}
