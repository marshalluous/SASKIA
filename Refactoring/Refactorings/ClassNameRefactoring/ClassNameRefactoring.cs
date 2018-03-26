using Microsoft.CodeAnalysis; using Microsoft.CodeAnalysis.CSharp; using Microsoft.CodeAnalysis.CSharp.Syntax; using Refactoring.Helper;
using System.Collections.Generic;  namespace Refactoring.DictionaryRefactorings { 	public sealed class ClassNameRefactoring : IRefactoring 	{ 		public string DiagnosticId => "SASKIA200"; 		public string Title => DiagnosticId; 		public string Description => Title;  		public IEnumerable<SyntaxKind> GetSyntaxKindsToRecognize() => 			new[] { SyntaxKind.ClassDeclaration };  		public DiagnosticInfo DoDiagnosis(SyntaxNode node) 		{ 			var classNode = (ClassDeclarationSyntax)node; 			var className = classNode.Identifier.ValueText;  			if (new HunspellEngine().HasTypo(className)) {
				return DiagnosticInfo.CreateFailedResult("Typo in class name", markableLocation: classNode.Identifier.GetLocation());
			}  			if (!new WordTypeChecker().IsNoun(className))
			{
				return DiagnosticInfo.CreateFailedResult("Class name must end with a noun", markableLocation: classNode.Identifier.GetLocation());
			}  			return DiagnosticInfo.CreateSuccessfulResult(); 		}
		
		public IEnumerable<SyntaxNode> GetFixableNodes(SyntaxNode node) 		{ 			return null; 		}  		public SyntaxNode GetReplaceableNode(SyntaxToken token) 		{ 			return token.Parent; 		} 	} } 