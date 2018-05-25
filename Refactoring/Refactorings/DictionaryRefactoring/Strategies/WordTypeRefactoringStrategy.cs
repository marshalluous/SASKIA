using System.Collections.Generic;
using Microsoft.CodeAnalysis;
using Refactoring.Refactorings.DictionaryRefactoring.Strategies.AbstractClasses;
using Refactoring.WordHelper;

namespace Refactoring.Refactorings.DictionaryRefactoring.Strategies
{
	internal sealed class WordTypeRefactoringStrategy : DictionaryClass, IRefactoringBaseStrategy
	{
		private AbstractRefactoringStrategy strategy;

		public void RegisterStrategy(AbstractRefactoringStrategy newStrategy)
		{
			strategy = newStrategy;
		}

		public DiagnosticInfo Diagnose(SyntaxNode syntaxNode, string description)
		{
			var syntaxToken = strategy.GetSyntaxToken(syntaxNode);
			var identifierText = syntaxToken.Text;
			return strategy.DiagnoseWordType(Database, identifierText, syntaxToken, description);
		}

		public IEnumerable<SyntaxNode> EvaluateNodes(SyntaxNode syntaxNode)
		{
			return strategy.EvaluateWordType(syntaxNode, Database);
		}
	}
}
