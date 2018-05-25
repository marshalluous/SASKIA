using System;
using System.Collections.Generic;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Refactoring.Refactorings.DictionaryRefactoring.Strategies;
using Refactoring.Refactorings.DictionaryRefactoring.Strategies.AbstractClasses;

namespace Refactoring.WordHelper
{
    internal sealed class DictionaryRefactoringFactory
	{
		private static readonly Dictionary<Type, Func<Type, AbstractRefactoringStrategy>> TypeDictionary = new Dictionary<Type, Func<Type, AbstractRefactoringStrategy>> {
			{ typeof(ClassDeclarationSyntax), baseType => new ClassDeclarationSyntaxStrategy(baseType)},
			{ typeof(InterfaceDeclarationSyntax), baseType => new InterfaceDeclarationSyntaxStrategy(baseType)},
			{ typeof(MethodDeclarationSyntax), baseType => new MethodDeclarationSyntaxStrategy(baseType)},
			{ typeof(VariableDeclaratorSyntax), baseType => new VariableDeclaratorSyntaxStrategy(baseType)},
			{ typeof(StructDeclarationSyntax), baseType => new StructDeclarationSyntaxStrategy(baseType)},
			{ typeof(EnumDeclarationSyntax), baseType => new EnumDeclarationSyntaxStrategy(baseType)},
			{ typeof(FieldDeclarationSyntax), baseType => new FieldDeclarationSyntaxStrategy(baseType)},
			{ typeof(PropertyDeclarationSyntax), baseType => new PropertyDeclarationSyntaxStrategy(baseType)}
		};

		public static IRefactoringBaseStrategy GetStrategy(Type strategyType, Type baseType)
		{
			if (!TypeDictionary.ContainsKey(strategyType))
			    throw new ArgumentException("StrategyType not existing");

			var strategy = TypeDictionary[strategyType].Invoke(baseType);
			var baseClass = (IRefactoringBaseStrategy)Activator.CreateInstance(strategy.BaseType);
			baseClass.RegisterStrategy(strategy);
			return baseClass;
		}
	}
}