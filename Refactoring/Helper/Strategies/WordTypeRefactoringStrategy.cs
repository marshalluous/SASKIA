using System.Collections.Generic;
using Microsoft.CodeAnalysis;

namespace Refactoring.Helper.Strategies
{
	public abstract class WordTypeRefactoringStrategy : Dictionary, IRefactoringBaseStrategy
	{
		protected abstract IEnumerable<string> IgnorableWords { get; }
		protected abstract IDictionary<string, List<string>> DefaultSuggestions { get; }
		protected abstract SyntaxToken GetSyntaxToken(SyntaxNode syntaxNode);
		protected virtual string NamePrefix => string.Empty;
		private AbstractRefactoringStrategy strategy;

		public void RegisterStrategy(AbstractRefactoringStrategy strategy)
		{
			this.strategy = strategy;
		}

		public DiagnosticInfo Diagnose(SyntaxNode syntaxNode, string description)
		{
			var syntaxToken = strategy.GetSyntaxToken(syntaxNode);
			var identifierText = syntaxToken.Text;
			var lastWord = WordSplitter.GetLastWord(identifierText);


			if (!new WordTypeChecker(Database).IsNoun(lastWord))
				return DiagnosticInfo.CreateFailedResult($"{description}: Class name must be a noun.", markableLocation: syntaxToken.GetLocation());
			return DiagnosticInfo.CreateSuccessfulResult();
		}

		public IEnumerable<SyntaxNode> EvaluateNodes(SyntaxNode syntaxNode)
		{
			return new[] { syntaxNode };
		}
	}
}
