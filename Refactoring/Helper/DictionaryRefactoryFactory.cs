using System;
using System.Collections.Generic;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Refactoring.Helper.Strategies;

namespace Refactoring.Helper
{
	class DictionaryRefactoryFactory
	{
		private static Dictionary<Type, Func<Type, AbstractRefactoringStrategy>> TypeDictionary = new Dictionary<Type, Func<Type, AbstractRefactoringStrategy>> {
			{ typeof(ClassDeclarationSyntax), (Type bbaseType) => { return new ClassDeclarationSyntaxStrategy(bbaseType);} },
			{ typeof(InterfaceDeclarationSyntax), (Type bbaseType) => { return new InterfaceDeclarationSyntaxStrategy(bbaseType);} },
			{ typeof(MethodDeclarationSyntax), (Type bbaseType) => { return new MethodDeclarationSyntaxStrategy(bbaseType);} },
			{ typeof(VariableDeclaratorSyntax), (Type bbaseType) => { return new VariableDeclaratorSyntaxStrategy(bbaseType);} },
			{ typeof(StructDeclarationSyntax), (Type bbaseType) => { return new StructDeclarationSyntaxStrategy(bbaseType);} },
			{ typeof(EnumDeclarationSyntax), (Type bbaseType) => { return new EnumDeclarationSyntaxStrategy(bbaseType);} },
			{ typeof(FieldDeclarationSyntax), (Type bbaseType) => { return new FieldDeclarationSyntaxStrategy(bbaseType);} },
			{ typeof(PropertyDeclarationSyntax), (Type bbaseType) => { return new PropertyDeclarationSyntaxStrategy(bbaseType);} }
		};

		public static IRefactoringBaseStrategy GetStrategy(Type strategyType, Type baseType)
		{
			if (!TypeDictionary.ContainsKey(strategyType)) throw new ArgumentException("StrategyType not existing");
			var strategy = TypeDictionary[strategyType].Invoke(baseType);
			var baseClass = (IRefactoringBaseStrategy)Activator.CreateInstance(strategy.BaseType);
			baseClass.RegisterStrategy(strategy);
			return baseClass;
		}
	}
}
