using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Refactoring.ConditionalComplexity
{
    internal sealed class ConditionalComplexityVisitor : CSharpSyntaxVisitor<int>
    {
        public override int VisitBinaryExpression(BinaryExpressionSyntax node)
        {
            var kind = node.OperatorToken.Kind();
            var previousValue = base.VisitBinaryExpression(node);

            if (kind == SyntaxKind.LogicalAndExpression || kind == SyntaxKind.LogicalOrExpression)
            {
                return 1 + previousValue;
            }

            return 0 + previousValue;
        }

        public override int VisitMethodDeclaration(MethodDeclarationSyntax node)
        {
            return 1 + base.VisitMethodDeclaration(node);
        }

        public override int VisitBreakStatement(BreakStatementSyntax node)
        {
            return 1 + base.VisitBreakStatement(node);
        }

        public override int VisitCasePatternSwitchLabel(CasePatternSwitchLabelSyntax node)
        {
            return 1 + base.VisitCasePatternSwitchLabel(node);
        }

        public override int VisitCaseSwitchLabel(CaseSwitchLabelSyntax node)
        {
            return 1 + base.VisitCaseSwitchLabel(node);
        }
        
        public override int VisitCatchDeclaration(CatchDeclarationSyntax node)
        {
            return 1 + base.VisitCatchDeclaration(node);
        }

        public override int VisitCatchFilterClause(CatchFilterClauseSyntax node)
        {
            return 1 + base.VisitCatchFilterClause(node);
        }
        
        public override int VisitConditionalAccessExpression(ConditionalAccessExpressionSyntax node)
        {
            return 1 + base.VisitConditionalAccessExpression(node);
        }

        public override int VisitConditionalExpression(ConditionalExpressionSyntax node)
        {
            return 1 + base.VisitConditionalExpression(node);
        }
        
        public override int VisitElifDirectiveTrivia(ElifDirectiveTriviaSyntax node)
        {
            return 1 + base.VisitElifDirectiveTrivia(node);
        }
        
        public override int VisitForEachStatement(ForEachStatementSyntax node)
        {
            return 1 + base.VisitForEachStatement(node);
        }
        
        public override int VisitForStatement(ForStatementSyntax node)
        {
            return 1 + base.VisitForStatement(node);
        }
        
        public override int VisitIfStatement(IfStatementSyntax node)
        {
            return 1 + base.VisitIfStatement(node);
        }
        
        public override int VisitThrowStatement(ThrowStatementSyntax node)
        {
            return 1 + base.VisitThrowStatement(node);
        }
        
        public override int VisitWhileStatement(WhileStatementSyntax node)
        {
            return 1 + base.VisitWhileStatement(node);
        }
    }
}