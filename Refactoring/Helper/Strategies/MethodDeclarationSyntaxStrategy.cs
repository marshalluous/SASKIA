using System;
using System.Collections.Generic;
using System.Data.SQLite;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Refactoring.Helper.Strategies
{
	class MethodDeclarationSyntaxStrategy : AbstractRefactoringStrategy
	{
		internal override IEnumerable<string> IgnorableWords => new List<string>();
		internal override IDictionary<string, List<string>> DefaultSuggestions => new Dictionary<string, List<string>> {
			{ "_", new List<string> { "" } }
		};

		internal override string NamePrefix => string.Empty;

		internal override Type BaseType { get; }

		public MethodDeclarationSyntaxStrategy(Type baseType)
		{
			BaseType = baseType;
		}

		internal override SyntaxToken GetSyntaxToken(SyntaxNode syntaxNode)
		{
			return ((MethodDeclarationSyntax)syntaxNode).Identifier;
		}

		internal override DiagnosticInfo DiagnoseWordType(SQLiteConnection database, string identifierText, SyntaxToken syntaxToken, string description)
		{
			var wordList = WordSplitter.GetSplittedWordList(identifierText);
			foreach (var word in wordList)
			{
				if (new WordTypeChecker(database).IsVerb(word))
					return DiagnosticInfo.CreateSuccessfulResult();
			}
			return DiagnosticInfo.CreateFailedResult($"{description}.", markableLocation: syntaxToken.GetLocation());
		}
	}
}
