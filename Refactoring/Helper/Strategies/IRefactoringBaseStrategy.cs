using System.Collections.Generic;
using Microsoft.CodeAnalysis;

namespace Refactoring.Helper.Strategies
{
	interface IRefactoringBaseStrategy
	{
		void RegisterStrategy(AbstractRefactoringStrategy strategy);
		DiagnosticInfo Diagnose(SyntaxNode syntaxNode, string description);
		IEnumerable<SyntaxNode> EvaluateNodes(SyntaxNode syntaxNode);
	}
}
