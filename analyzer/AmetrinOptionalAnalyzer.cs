using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.CodeAnalysis.Operations;
using static Ametrin.Optional.Analyzer.Helper;

namespace Ametrin.Optional.Analyzer;

[DiagnosticAnalyzer(LanguageNames.CSharp)]
public sealed class AmetrinOptionalAnalyzer : DiagnosticAnalyzer
{

    public static readonly DiagnosticDescriptor ImpossibleRequire
        = new(id: "AmOptional001", title: "Impossible Require call", messageFormat: "{0} can never be {1}. If a conversion exists use .Map to explicitly use it.", category: "Usage", defaultSeverity: DiagnosticSeverity.Error, isEnabledByDefault: true);

    public static readonly DiagnosticDescriptor UnnecessaryRequire
        = new(id: "AmOptional002", title: "Unnecessary Require call", messageFormat: "type already is {0}", category: "Usage", defaultSeverity: DiagnosticSeverity.Warning, isEnabledByDefault: true);

    public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics { get; } = [ImpossibleRequire, UnnecessaryRequire];

    public override void Initialize(AnalysisContext context)
    {
        context.EnableConcurrentExecution();
        context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.Analyze | GeneratedCodeAnalysisFlags.ReportDiagnostics);

        context.RegisterCompilationStartAction(static context =>
        {
            // var targetType = context.Compilation.GetTypeByMetadataName("Ametrin.Optional.Option")!;
            // var methods = targetType.GetMembers("Map").OfType<IMethodSymbol>().ToImmutableArray();

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
                    else if (!conversion.Exists || conversion.IsUserDefined)
                    {
                        context.ReportDiagnostic(Diagnostic.Create(ImpossibleRequire, invocation.Syntax.GetLocation(), currentType.ToDisplayString(), targetType.ToDisplayString()));
                    }
                }

            }, OperationKind.Invocation);
        });
    }
}
