using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeActions;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Immutable;
using System.Composition;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace RoslynVsixSandbox
{
    [ExportCodeFixProvider(LanguageNames.CSharp), Shared]
    public sealed class BooleanComparisonCodeFixProvider : CodeFixProvider
    {
        public override ImmutableArray<string> FixableDiagnosticIds =>
            ImmutableArray.Create(BooleanComparisonDiagnosticAnalyzer.DiagnosticId);

        public override Task RegisterCodeFixesAsync(CodeFixContext context)
        {
            context.RegisterCodeFix(
                CodeAction.Create("test", token => ApplyFix(context, token)),
                context.Diagnostics);

            return Task.Delay(0);
        }

        private static bool IsBooleanLiteralNode(ExpressionSyntax expressionSyntax,
            string literal)
        {
            return expressionSyntax is LiteralExpressionSyntax &&
                expressionSyntax.GetText().ToString().Trim() == literal;
        }

        private static BinaryExpressionSyntax GetParentBinaryExpressionNode(SyntaxNode syntaxNode)
        {
            while (!(syntaxNode is BinaryExpressionSyntax))
            {
                syntaxNode = syntaxNode.Parent;
            }

            return (BinaryExpressionSyntax) syntaxNode;
        }

        private static async Task<Document> ApplyFix(CodeFixContext context, CancellationToken cancellationToken)
        {
            var document = context.Document;
            var root = await document.GetSyntaxRootAsync(cancellationToken)
                .ConfigureAwait(false);

            var diagnostic = context.Diagnostics.First();
            var position = diagnostic.Location.SourceSpan.Start;

            var equalsEqualsNode =
                GetParentBinaryExpressionNode(root.FindToken(position).Parent.Parent);

            ExpressionSyntax replaceNode = null;
            bool isNot;

            if (IsBooleanLiteralNode(equalsEqualsNode.Left, "true"))
            {
                replaceNode = equalsEqualsNode.Right;
                isNot = false;
            }
            else if (IsBooleanLiteralNode(equalsEqualsNode.Left, "false"))
            {
                replaceNode = equalsEqualsNode.Right;
                isNot = true;
            }
            else if (IsBooleanLiteralNode(equalsEqualsNode.Right, "true"))
            {
                replaceNode = equalsEqualsNode.Left;
                isNot = false;
            }
            else
            {
                replaceNode = equalsEqualsNode.Left;
                isNot = true;
            }

            if (equalsEqualsNode.OperatorToken.Text.Trim() == "!=")
            {
                isNot = !isNot;
            }

            if (isNot)
            {
                replaceNode = SyntaxFactory.PrefixUnaryExpression(SyntaxKind.LogicalNotExpression, replaceNode);
            }

            replaceNode = replaceNode.NormalizeWhitespace();
            
            root = root.ReplaceNode(equalsEqualsNode, replaceNode);
            
            return document.WithSyntaxRoot(root);
        }

        public sealed override FixAllProvider GetFixAllProvider()
        {
            return WellKnownFixAllProviders.BatchFixer;
        }
    }
}