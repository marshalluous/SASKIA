using System.Collections.Generic;
using Microsoft.CodeAnalysis;

namespace Refactoring.Helper.Strategies
{
	public sealed class WordTypeRefactoringStrategy : Dictionary, IRefactoringBaseStrategy
	{
		private AbstractRefactoringStrategy strategy;

		public void RegisterStrategy(AbstractRefactoringStrategy strategy)
		{
			this.strategy = strategy;
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
