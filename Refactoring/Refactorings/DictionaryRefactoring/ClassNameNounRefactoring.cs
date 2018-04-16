using System.Collections.Generic;
using System.Data.SQLite;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Refactoring.Helper;

namespace Refactoring.Refactorings.DictionaryRefactoring
{
    public sealed class ClassNameNounRefactoring : Dictionary, IRefactoring
	{
		public string DiagnosticId => "SASKIA220";
		public string Title => DiagnosticId;
		public string Description => "Class name must be a noun";

		public IEnumerable<SyntaxKind> GetSyntaxKindsToRecognize() =>
			new[] { SyntaxKind.ClassDeclaration };


		public SyntaxNode GetReplaceableRootNode(SyntaxToken token) =>
			GetReplaceableNode(token);

		public DiagnosticInfo DoDiagnosis(SyntaxNode node)
		{
			var classNode = (ClassDeclarationSyntax)node;
			var identifierText = classNode.Identifier.Text;
			var lastWord = WordSplitter.GetLastWord(identifierText);
            if (!new WordTypeChecker(Database).IsNoun(lastWord))
            {
				return DiagnosticInfo.CreateFailedResult($"{Description}: {lastWord}.", markableLocation: classNode.Identifier.GetLocation());
			}
			return DiagnosticInfo.CreateSuccessfulResult();
        }

        private bool IsNoun(SQLiteConnection dbConnection, string word)
        {
            const int WordTypeLocation = 1;
            using (SQLiteCommand getWordFromDatabase = new SQLiteCommand(dbConnection))
            {
                getWordFromDatabase.CommandText = $"select * from entries where word = '{word}'";
                var reader = getWordFromDatabase.ExecuteReader();
                while (reader.Read())
                {
                    if (reader.GetString(WordTypeLocation).StartsWith("n")) return true;
                }
                return false;
            }
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
