using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.CodeAnalysis;
using Refactoring.DictionaryRefactorings;
using System.Collections.Immutable;
using System.Resources;
using System.Linq;

namespace SASKIA.DiagnosticAnalyzers
{
	[DiagnosticAnalyzer(LanguageNames.CSharp)]
	public sealed class ClassNameDiagnosticAnalyzer : DiagnosticAnalyzer
	{
		public const string DiagnosticId = "ClassNameDiagnostic";

		internal static string Title = "Blabla";
		internal static string MessageFormat = "Typo in Class Name";
		internal static string Description = "Refactoring Class Name";
		internal const string Category = "Naming";
		internal static DiagnosticDescriptor Rule = new DiagnosticDescriptor(DiagnosticId, Title, MessageFormat, Category, DiagnosticSeverity.Warning, true, Description);

		public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => ImmutableArray.Create(Rule);

		public override void Initialize(AnalysisContext context)
		{
			// TODO: Consider registering other actions that act on syntax instead of or in addition to symbols
			context.RegisterSymbolAction(AnalyzeSymbol, SymbolKind.NamedType);
		}

		private static void AnalyzeSymbol(SymbolAnalysisContext context)
		{
			// TODO: Replace the following code with your own analysis, generating Diagnostic objects for any issues you find
			var namedTypeSymbol = (INamedTypeSymbol)context.Symbol;

			// Find just those named type symbols with names containing lowercase letters.
			if (namedTypeSymbol.Name.ToCharArray().Any(char.IsLower))
			{
				// For all such symbols, produce a diagnostic.
				var diagnostic = Diagnostic.Create(Rule, namedTypeSymbol.Locations[0], namedTypeSymbol.Name);

				context.ReportDiagnostic(diagnostic);
			}
		}
	}
}
