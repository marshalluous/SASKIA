using System;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Refactoring.Helper.Strategies;

namespace Refactoring.Helper
{
	class TypoRefactoryFactory
	{
		public static TypoRefactoringStrategy GetStrategy(Type type)
		{
			if (type == typeof(ClassDeclarationSyntax))
				return new ClassDeclarationSyntaxStrategy();
			if (type == typeof(InterfaceDeclarationSyntax))
				return new InterfaceDeclarationSyntaxStrategy();
			else if (type == typeof(MethodDeclarationSyntax))
				return new MethodDeclarationSyntaxStrategy();
			else if (type == typeof(VariableDeclaratorSyntax))
				return new VariableDeclaratorSyntaxStrategy();
			//else if (type == typeof(StructDeclarationSyntax))
			//	return new StructDeclaratorSyntaxStrategy();
			//else if (type == typeof(EnumDeclarationSyntax))
			//	return new EnumDeclaratorSyntaxStrategy();
			else
				return new PropertyDeclarationSyntaxStrategy();
		}
	}
}
