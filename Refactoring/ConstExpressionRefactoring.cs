﻿using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CSharp;

namespace Refactoring
{
    public sealed class ConstExpressionRefactoring : IRefactoring
    {
        public string DiagnosticId => "SASKIA003";

        public string Title => "Constant expression detected";

        public string Description => Title;
        
        public IEnumerable<SyntaxNode> ApplyFix(SyntaxNode node)
        {
            var value = VisitExpressionSyntaxNodes(node).Item2;
            return new[] { CreateLiteralNode(value) };   
        }

        private static SyntaxNode CreateLiteralNode(object value)
        {
            var type = typeof(SyntaxFactory);

            if (value is bool booleanValue)
            {
                return SyntaxFactory.LiteralExpression(booleanValue ?
                    SyntaxKind.TrueLiteralExpression :
                    SyntaxKind.FalseLiteralExpression);
            }

            var method = type.GetMethod("Literal", new[] { value.GetType() });
            var token = (SyntaxToken) method.Invoke(null, new[] { value });
            return SyntaxFactory.LiteralExpression(SyntaxKind.NumericLiteralExpression, token);
        }

        public DiagnosticInfo DoDiagnosis(SyntaxNode node)
        {
            return VisitExpressionSyntaxNodes(node).Item1;
        }

        public SyntaxNode GetReplaceableNode(SyntaxToken token)
        {
            var node = token.Parent;

            while (node is ExpressionSyntax && node.Parent is ExpressionSyntax)
            {
                node = node.Parent;
            }

            return node;
        }
        
        public IEnumerable<SyntaxKind> GetSyntaxKindsToRecognize() =>
            typeof(SyntaxKind)
                .GetEnumNames()
                .Where(name => name.EndsWith("Expression"))
                .Select(name => (SyntaxKind)Enum.Parse(typeof(SyntaxKind), name));

        private static Tuple<DiagnosticInfo, object> VisitExpressionSyntaxNodes(SyntaxNode node)
        {
            if (node is ExpressionSyntax expression)
            {
                var previousExpression = expression.GetText().ToString();
                var simplifiedExpression = Evaluate(previousExpression);

                if (simplifiedExpression != null && previousExpression != simplifiedExpression.ToString())
                {
                    return Tuple.Create(DiagnosticInfo.CreateFailedResult("const expr found"), simplifiedExpression);
                }
            }
            
            foreach (var childNode in node.ChildNodes())
            {
                var r = VisitExpressionSyntaxNodes(childNode);

                if (r.Item1.DiagnosticFound)
                {
                    return r;
                }
            }

            return Tuple.Create(DiagnosticInfo.CreateSuccessfulResult(), (object) null);
        }
        
        private static string CreateCodeTemplate(string code)
        {
            var codeTemplate = new StringBuilder("namespace A{");
            codeTemplate.Append("public class B{");
            codeTemplate.Append("public object C(){");
            codeTemplate.Append("return " + code + ";");
            codeTemplate.Append("}}}");
            return codeTemplate.ToString();
        }

        private static CompilerResults Compile(string code)
        {
            var codeProvider = new CSharpCodeProvider();
            var compilerParameters = new CompilerParameters
            {
                CompilerOptions = "/t:library",
                GenerateInMemory = true
            };

            return codeProvider.CompileAssemblyFromSource(compilerParameters, CreateCodeTemplate(code));
        }
        
        private static object Evaluate(string code)
        {
            var compilerResult = Compile(code);

            if (compilerResult.Errors.Count > 0)
            {
                return null;
            }

            var assembly = compilerResult.CompiledAssembly;
            var instance = assembly.CreateInstance("A.B");
            var method = instance?.GetType().GetMethod("C");
            return method?.Invoke(instance, null);
        }
    }
}