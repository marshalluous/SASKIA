using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeFixes;
using System.Composition;
using Refactoring.DictionaryRefactorings;

namespace SASKIA.CodeFixProviders
{
	[ExportCodeFixProvider(LanguageNames.CSharp), Shared]
	public sealed class ClassNameCodeFixProvider : CodeSmellCodeFixProvider
	{
		public ClassNameCodeFixProvider() 
			: base(new ClassNameRefactoring())
		{
		}
		//	public override ImmutableArray<string> FixableDiagnosticIds => ImmutableArray.Create(ClassNameDiagnosticAnalyzer.DiagnosticId);

		//	public override async Task RegisterCodeFixesAsync(CodeFixContext context)
		//	{
		//		var root = await context.Document.GetSyntaxRootAsync(context.CancellationToken).ConfigureAwait(false);
		//		var diagnostic = context.Diagnostics.First();
		//		var diagnosticSpan = diagnostic.Location.SourceSpan;
		//		var declaration = root.FindToken(diagnosticSpan.Start).Parent.AncestorsAndSelf().OfType<TypeDeclarationSyntax>().First();

		//		context.RegisterCodeFix(
		//			CodeAction.Create(
		//				title: "Rename " + "SASKIA4000",
		//				createChangedSolution: cancellationToken => ProvideNewName(context.Document,declaration, cancellationToken)
		//			),
		//			diagnostic
		//		);
		//	}

		//	private async Task<Solution> ProvideNewName(Document document, TypeDeclarationSyntax declSyntax, CancellationToken cancellationToken)
		//	{
		//		var identifierToken = declSyntax.Identifier;
		//		var newName = identifierToken.Text.ToUpperInvariant(); // do Hunspell magic here
		//		var semanticModel = await document.GetSemanticModelAsync(cancellationToken);
		//		var typeSymbol = semanticModel.GetDeclaredSymbol(declSyntax, cancellationToken);
		//		var original = document.Project.Solution;
		//		var optionSet = original.Workspace.Options;
		//		var newSolution = await Renamer.RenameSymbolAsync(original, typeSymbol, newName, optionSet, cancellationToken).ConfigureAwait(false);
		//		return newSolution;
		//	}
	}
}
