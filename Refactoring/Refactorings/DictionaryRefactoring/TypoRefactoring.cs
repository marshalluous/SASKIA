﻿using System.Collections.Generic;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Refactoring.Helper;
using Refactoring.Refactorings.DictionaryRefactoring.Strategies;
using Refactoring.SyntaxTreeHelper;
using Refactoring.WordHelper;

namespace Refactoring.Refactorings.DictionaryRefactoring
{
	public sealed class TypoRefactoring : IRefactoring
	{
		public string DiagnosticId => RefactoringId.TypoChecker.GetDiagnosticId();
        public string Title => DiagnosticId;
		public string Description => "Typo in name";

		public IEnumerable<SyntaxKind> GetSyntaxKindsToRecognize() =>
			new[] { SyntaxKind.ClassDeclaration, SyntaxKind.InterfaceDeclaration, SyntaxKind.MethodDeclaration,
				SyntaxKind.VariableDeclarator, SyntaxKind.PropertyDeclaration, SyntaxKind.FieldDeclaration };

		public DiagnosticInfo DoDiagnosis(SyntaxNode node) => 
		    Strategy(node).Diagnose(node, Description);

	    public IEnumerable<SyntaxNode> GetFixableNodes(SyntaxNode node) => 
		    Strategy(node).EvaluateNodes(node);

	    private static IRefactoringBaseStrategy Strategy(SyntaxNode node) => 
	        DictionaryRefactoringFactory.GetStrategy(node.GetType(), typeof(TypoRefactoringStrategy));

	    public SyntaxNode GetReplaceableNode(SyntaxToken token) => 
	        token.Parent;

	    public SyntaxNode GetReplaceableRootNode(SyntaxToken token) =>
			SyntaxNodeHelper.FindAncestorOfType<ClassDeclarationSyntax>(token);
	}
}
