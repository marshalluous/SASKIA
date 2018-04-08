using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Refactoring.Helper;
using System.Collections.Generic;
using System.Linq;

namespace Refactoring.DictionaryRefactorings
{
    public sealed class TypoRefactoring : IRefactoring
	{
		public string DiagnosticId => "SASKIA200";
		public string Title => DiagnosticId;
		public string Description => "Typo";

		public IEnumerable<SyntaxKind> GetSyntaxKindsToRecognize() =>
			new[] { SyntaxKind.ClassDeclaration, SyntaxKind.MethodDeclaration, SyntaxKind.VariableDeclarator, SyntaxKind.PropertyDeclaration };

		public DiagnosticInfo DoDiagnosis(SyntaxNode node)
		{
            if (node is ClassDeclarationSyntax)
                return Diagnose(((ClassDeclarationSyntax)node).Identifier);
            else if (node is MethodDeclarationSyntax)
				return Diagnose(((MethodDeclarationSyntax)node).Identifier);
			else if(node is VariableDeclaratorSyntax)
				return Diagnose(((VariableDeclaratorSyntax)node).Identifier);
			else
				return Diagnose(((PropertyDeclarationSyntax)node).Identifier);

		}

        private DiagnosticInfo Diagnose(SyntaxToken syntaxToken)
        {
            var hunspell = new HunspellEngine();
            var wordList = WordSplitter.GetSplittedWordList(syntaxToken.Text);

            foreach (var word in wordList.Where(w => hunspell.HasTypo(w)))
            {
                string suggestions = GetSuggestions(word, hunspell);
                return DiagnosticInfo.CreateFailedResult($"{Description}: {word}.\n{suggestions}", markableLocation: syntaxToken.GetLocation());
            }
            return DiagnosticInfo.CreateSuccessfulResult();
        }

		public IEnumerable<SyntaxNode> GetFixableNodes(SyntaxNode node)
        {
            if (node is ClassDeclarationSyntax)
				return EvaluateNodes((ClassDeclarationSyntax)node, ((ClassDeclarationSyntax)node).Identifier);
            else if (node is MethodDeclarationSyntax)
				return EvaluateNodes((MethodDeclarationSyntax)node, ((MethodDeclarationSyntax)node).Identifier);
			else if (node is VariableDeclaratorSyntax)
				return EvaluateNodes((VariableDeclaratorSyntax)node, ((VariableDeclaratorSyntax)node).Identifier);
			else
				return EvaluateNodes((PropertyDeclarationSyntax)node, ((PropertyDeclarationSyntax)node).Identifier);
		}

		private IEnumerable<SyntaxNode> EvaluateNodes(SyntaxNode node, SyntaxToken identifier)
		{
			var suggestion = new HunspellEngine().GetSuggestions(identifier.Text).First();
			if (suggestion == null)
				return null;
			return new[] { node.ReplaceToken(identifier, SyntaxFactory.Identifier(suggestion)) };
		}

		private string GetSuggestions(string word, HunspellEngine hunspell)
		{
			var suggestionsRefactored = hunspell.GetSuggestions(word).Aggregate((x, y) => $"{x}\r\n{y}");
			if (!string.IsNullOrEmpty(suggestionsRefactored)) suggestionsRefactored = "Suggestions:\n" + suggestionsRefactored;
			return suggestionsRefactored;
		}

		public SyntaxNode GetReplaceableNode(SyntaxToken token)
		{
			return token.Parent;
		}
	}
}
