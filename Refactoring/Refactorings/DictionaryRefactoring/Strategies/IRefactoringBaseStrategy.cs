using System.Collections.Generic;
using Microsoft.CodeAnalysis;
using Refactoring.Refactorings.DictionaryRefactoring.Strategies.AbstractClasses;

namespace Refactoring.Refactorings.DictionaryRefactoring.Strategies
{
    internal interface IRefactoringBaseStrategy
	{
		void RegisterStrategy(AbstractRefactoringStrategy strategy);
		DiagnosticInfo Diagnose(SyntaxNode syntaxNode, string description);
		IEnumerable<SyntaxNode> EvaluateNodes(SyntaxNode syntaxNode);
	}
}
