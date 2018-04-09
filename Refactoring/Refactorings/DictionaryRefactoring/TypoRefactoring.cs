using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Refactoring.Helper;
using Refactoring.Refactorings.DictionaryRefactoring;
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
			new[] { SyntaxKind.ClassDeclaration, SyntaxKind.MethodDeclaration, SyntaxKind.VariableDeclarator, SyntaxKind.PropertyDeclaration, SyntaxKind.FieldDeclaration };

		private string CreateNewIdentifier(List<string> allWords, string affectedWord, List<string> suggestions)
		{
			if (suggestions == null) return "";
			var newIdentifier = "";
			int affectedIndex = allWords.FindIndex(w => w == affectedWord);
			var suggestion = suggestions.First();
			foreach (var word in allWords)
			{
				var temp = word;
				if (allWords.FindIndex(w => w == temp) == affectedIndex)
					temp = suggestion;
				newIdentifier += temp;
			}
			return newIdentifier;
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

		private DiagnosticInfo EffectiveDiagnosis(SyntaxToken syntaxToken)
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

		public DiagnosticInfo DoDiagnosis(SyntaxNode node)
		{
            if (node is ClassDeclarationSyntax)
                return EffectiveDiagnosis(((ClassDeclarationSyntax)node).Identifier);
            else if (node is MethodDeclarationSyntax)
				return EffectiveDiagnosis(((MethodDeclarationSyntax)node).Identifier);
			else if (node is VariableDeclaratorSyntax)
				return EffectiveDiagnosis(((VariableDeclaratorSyntax)node).Identifier);
			//else if (node is FieldDeclarationSyntax)
			//	return EffectiveDiagnosis(((FieldDeclarationSyntax)node).Modifiers.First());
			else
				return EffectiveDiagnosis(((PropertyDeclarationSyntax)node).Identifier);
			// interfaces -> starting with I
			// delegates
			// enums
			// struct
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
			string newIdentifier = CreateNewIdentifier(allWords, affectedWord, suggestions);
			if (newIdentifier == "") return null;

			var classNode = SyntaxNodeHelper.FindAncestorOfType<ClassDeclarationSyntax>(identifier);

			var semanticModel = SemanticSymbolBuilder.GetSemanticModel(classNode);
			var typeSymbol = SemanticSymbolBuilder.GetTypeSymbol(classNode, semanticModel);
			var memberSymbol = semanticModel.GetDeclaredSymbol(node);

			return new[] { X(classNode, semanticModel, memberSymbol) };

			//return new[] { node.ReplaceToken(identifier, SyntaxFactory.Identifier(newIdentifier)) };
		}

		private SyntaxNode X(SyntaxNode node, SemanticModel semanticModel, ISymbol memberSymbol)
		{
			var v = new TypoRefactoringVisitor(semanticModel, memberSymbol);
			return v.Visit(node);
		}

		public SyntaxNode GetReplaceableNode(SyntaxToken token)
		{
			return token.Parent;
		}


		public SyntaxNode GetReplaceableRootNode(SyntaxToken token) =>
			SyntaxNodeHelper.FindAncestorOfType<ClassDeclarationSyntax>(token);
	}
}
