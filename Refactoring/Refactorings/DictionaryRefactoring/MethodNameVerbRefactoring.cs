using System.Collections.Generic;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Refactoring.Helper;

namespace Refactoring.Refactorings.DictionaryRefactoring
{
    public sealed class MethodNameVerbRefactoring : Dictionary, IRefactoring
	{
		public string DiagnosticId => "SASKIA220";
		public string Title => DiagnosticId;
		public string Description => "Method name must consist of a verb";

		public IEnumerable<SyntaxKind> GetSyntaxKindsToRecognize() =>
			new[] { SyntaxKind.MethodDeclaration };


		public SyntaxNode GetReplaceableRootNode(SyntaxToken token) =>
			GetReplaceableNode(token);


		public DiagnosticInfo DoDiagnosis(SyntaxNode node)
		{
		    var methodNode = (MethodDeclarationSyntax)node;
			var identifierText = methodNode.Identifier.Text;
            if (identifierText == "Main") return DiagnosticInfo.CreateSuccessfulResult();
            var wordList = WordSplitter.GetSplittedWordList(identifierText);
            foreach (var word in wordList)
            {
                if (new WordTypeChecker(Database).IsVerb(word))
                    return DiagnosticInfo.CreateSuccessfulResult();
            }
            return DiagnosticInfo.CreateFailedResult($"{Description}.", markableLocation: methodNode.Identifier.GetLocation());
        }

        public IEnumerable<SyntaxNode> GetFixableNodes(SyntaxNode node)
		{
            return new[] { node };
		}

		public SyntaxNode GetReplaceableNode(SyntaxToken token)
		{
			return token.Parent;
		}
	}
}
