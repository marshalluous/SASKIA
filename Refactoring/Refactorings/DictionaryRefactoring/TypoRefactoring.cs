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
			bool hasTypo;
			string affectedWord;
			List<string> suggestions;
			EvaluateTypo(WordSplitter.GetSplittedWordList(syntaxToken.Text), out affectedWord, out hasTypo, out suggestions);
			if (hasTypo)
			{
				var suggestionsAsString = "";
				if (suggestions != null) 
					suggestionsAsString = "Suggestions:\n" + suggestions.Aggregate((x, y) => $"{x}\r\n{y}");
				return DiagnosticInfo.CreateFailedResult($"{Description}: {affectedWord}.\n{suggestionsAsString}", markableLocation: syntaxToken.GetLocation());
			}
			else
				return DiagnosticInfo.CreateSuccessfulResult();
        }

		private void EvaluateTypo(List<string> wordList, out string affectedWord, out bool hasTypo, out List<string> suggestions)
		{
			suggestions = null;
			hasTypo = false;
			affectedWord = "";
			var hunspell = new HunspellEngine();
			foreach (var word in wordList.Where(w => hunspell.HasTypo(w)))
			{
				hasTypo = true;
				affectedWord = word;
				suggestions = hunspell.GetSuggestions(word);
				return;
			}
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
			bool hasTypo;
			string affectedWord = "";
			List<string> suggestions;
			var allWords = WordSplitter.GetSplittedWordList(identifier.Text);
			EvaluateTypo(allWords, out affectedWord, out hasTypo, out suggestions);
			if (suggestions == null)
				return null;
			int affectedIndex = allWords.FindIndex(w => w == affectedWord);
			var suggestion = suggestions.First();
			var newIdentifier = "";
			foreach (var word in allWords)
			{
				var temp = word;
				if (allWords.FindIndex(w => w == temp) == affectedIndex)
				{
					temp = suggestion;
				}
				newIdentifier += temp;
			}
			return new[] { node.ReplaceToken(identifier, SyntaxFactory.Identifier(newIdentifier)) };
		}

		public SyntaxNode GetReplaceableNode(SyntaxToken token)
		{
			return token.Parent;
		}
	}
}
