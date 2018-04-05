using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Refactoring.Helper;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Refactoring.DictionaryRefactorings
{
	public sealed class ClassNameRefactoring : IRefactoring
	{
		public string DiagnosticId => "SASKIA200";
		public string Title => DiagnosticId;
		public string Description => Title;

		public IEnumerable<SyntaxKind> GetSyntaxKindsToRecognize() =>
			new[] { SyntaxKind.ClassDeclaration };

		public DiagnosticInfo DoDiagnosis(SyntaxNode node)
		{
			var classNode = (ClassDeclarationSyntax)node;
			var identifierText = classNode.Identifier.Text;
			var hunspell = new HunspellEngine();
			var wordList = WordSplitter.GetSplittedWordList(identifierText);

			foreach (var word in wordList.Where(w => hunspell.HasTypo(w)))
			{
				string suggestions = GetSuggestions(word, hunspell);
				return DiagnosticInfo.CreateFailedResult($"Typo in class name. Word: {word}. {suggestions}", markableLocation: classNode.Identifier.GetLocation());
			}

			return DiagnosticInfo.CreateSuccessfulResult();
		}

		public IEnumerable<SyntaxNode> GetFixableNodes(SyntaxNode node)
		{
			var classNode = (ClassDeclarationSyntax)node;
			var identifierText = classNode.Identifier.Text;
			var hunspell = new HunspellEngine();
			var suggestion = GetSuggestionList(identifierText, hunspell).First();

			if (suggestion == null)
				return null;

			return new[] { classNode.ReplaceToken(classNode.Identifier, SyntaxFactory.Identifier(suggestion)) };
		}


		/*
			if (!new WordTypeChecker().IsNoun(identifierText))
{
	return DiagnosticInfo.CreateFailedResult("Class name must end with a noun");
}*/

		private IEnumerable<string> GetSuggestionList(string word, HunspellEngine hunspell)
		{
			const int displayWordLimit = 5;
			return hunspell.GetSuggestions(word).Take(displayWordLimit);
		}

		private string GetSuggestions(string word, HunspellEngine hunspell)
		{
			var suggestionsRefactored = GetSuggestionList(word, hunspell).Aggregate((x, y) => $"{x}\r\n{y}");
			if (!string.IsNullOrEmpty(suggestionsRefactored)) suggestionsRefactored = "Suggestions:\n" + suggestionsRefactored;
			return suggestionsRefactored;
		}

		public SyntaxNode GetReplaceableNode(SyntaxToken token)
		{
			return token.Parent;
		}
	}
}
