using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Refactoring.Helper
{
    internal sealed class PureExpressionCheckerVisitor : CSharpSyntaxVisitor<bool>
    {
        private readonly CompilationUnitSyntax compilationUnitSyntax;

        private bool IsProperty(SyntaxNode node)
        {
            var symbol = GetSymbol(node);
            return symbol != null &&
                symbol.Kind == SymbolKind.Property;
        }

        private ISymbol GetSymbol(SyntaxNode node)
        {
            if (compilationUnitSyntax == null)
                return null;

            var syntaxTree = compilationUnitSyntax.SyntaxTree;
            var compilation = CSharpCompilation.Create("CompilationUnit", new[] { syntaxTree });
            var model = compilation.GetSemanticModel(syntaxTree);
            return model.GetSymbolInfo(node).Symbol;
        }

        public PureExpressionCheckerVisitor(CompilationUnitSyntax compilationUnitSyntax)
        {
            this.compilationUnitSyntax = compilationUnitSyntax;
        }

        public override bool DefaultVisit(SyntaxNode node)
        {
            return false;
        }

        public override bool VisitMemberAccessExpression(MemberAccessExpressionSyntax node)
        {
            return node.Expression.Accept(this);
        }

        public override bool VisitLiteralExpression(LiteralExpressionSyntax node)
        {
            return true;
        }
        
        public override bool VisitIdentifierName(IdentifierNameSyntax node)
        {
            return node != null &&
                   !IsProperty(node);
        }

        public override bool VisitBinaryExpression(BinaryExpressionSyntax node)
        {
            return node.Left.Accept(this) &&
                   node.Right.Accept(this);
        }

        public override bool VisitPrefixUnaryExpression(PrefixUnaryExpressionSyntax node)
        {
            var operatorKind = node.OperatorToken.Kind();
            
            if (operatorKind == SyntaxKind.PlusPlusToken || operatorKind == SyntaxKind.MinusEqualsToken)
            {
                return false;
            }

            return node.Operand.Accept(this);
        }

        public override bool VisitPostfixUnaryExpression(PostfixUnaryExpressionSyntax node)
        {
            return false;
        }
        
        public override bool VisitParenthesizedExpression(ParenthesizedExpressionSyntax node)
        {
            return node.Expression.Accept(this);
        }
    }
}
