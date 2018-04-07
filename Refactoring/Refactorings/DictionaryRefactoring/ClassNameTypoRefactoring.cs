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
		public string Description => "Typo in class name";

		public IEnumerable<SyntaxKind> GetSyntaxKindsToRecognize() =>
			new[] { SyntaxKind.ClassDeclaration, SyntaxKind.MethodDeclaration };

		public DiagnosticInfo DoDiagnosis(SyntaxNode node)
		{
            if (node is ClassDeclarationSyntax)
                return Diagnose(((ClassDeclarationSyntax)node).Identifier);
            else
                return Diagnose(((MethodDeclarationSyntax)node).Identifier);
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
            var hunspell = new HunspellEngine();
            if (node is ClassDeclarationSyntax)
            {
                var suggestion = hunspell.GetSuggestions(((ClassDeclarationSyntax)node).Identifier.Text).First();
                if (suggestion == null)
                    return null;
                return new[] { node.ReplaceToken(((ClassDeclarationSyntax)node).Identifier, SyntaxFactory.Identifier(suggestion)) };
            }
            else
            {
                var suggestion = hunspell.GetSuggestions(((MethodDeclarationSyntax)node).Identifier.Text).First();
                if (suggestion == null)
                    return null;
                return new[] { node.ReplaceToken(((MethodDeclarationSyntax)node).Identifier, SyntaxFactory.Identifier(suggestion)) };
            }
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
