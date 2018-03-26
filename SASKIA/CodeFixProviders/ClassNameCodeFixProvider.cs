using System;
using System.Collections.Immutable;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeActions;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Rename;
using SASKIA.DiagnosticAnalyzers;

namespace SASKIA.CodeFixProviders
{
	public sealed class ClassNameCodeFixProvider : CodeFixProvider
	{
		public override ImmutableArray<string> FixableDiagnosticIds => ImmutableArray.Create(ClassNameDiagnosticAnalyzer.DiagnosticId);

		public override async Task RegisterCodeFixesAsync(CodeFixContext context)
		{
			var root = await context.Document.GetSyntaxRootAsync(context.CancellationToken).ConfigureAwait(false);
			var diagnostic = context.Diagnostics.First();
			var diagnosticSpan = diagnostic.Location.SourceSpan;
			var declaration = root.FindToken(diagnosticSpan.Start).Parent.AncestorsAndSelf().OfType<TypeDeclarationSyntax>().First();

			context.RegisterCodeFix(
				CodeAction.Create(
					title: "Rename",
					createChangedSolution: cancellationToken => ProvideNewName(context.Document,declaration, cancellationToken)
				),
				diagnostic
			);
		}

		private async Task<Solution> ProvideNewName(Document document, TypeDeclarationSyntax declSyntax, CancellationToken cancellationToken)
		{
			var identifierToken = declSyntax.Identifier;
			var newName = identifierToken.Text.ToUpperInvariant(); // do Hunspell magic here
			var semanticModel = await document.GetSemanticModelAsync(cancellationToken);
			var typeSymbol = semanticModel.GetDeclaredSymbol(declSyntax, cancellationToken);
			var original = document.Project.Solution;
			var optionSet = original.Workspace.Options;
			var newSolution = await Renamer.RenameSymbolAsync(original, typeSymbol, newName, optionSet, cancellationToken).ConfigureAwait(false);
			return newSolution;
		}
	}
}
