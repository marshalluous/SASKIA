using System;
using System.Collections.Generic;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeActions;
using Microsoft.CodeAnalysis.CodeFixes;
using Refactoring;
using System.Collections.Immutable;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.Formatting;
using Microsoft.CodeAnalysis.MSBuild;

namespace SASKIA
{
    public abstract class CodeSmellCodeFixProvider : CodeFixProvider
    {
        private readonly IRefactoring refactoring;

        protected CodeSmellCodeFixProvider(IRefactoring refactoring)
        {
            this.refactoring = refactoring;
        }
        
        public override Task RegisterCodeFixesAsync(CodeFixContext context)
        {
            var codeAction = CodeAction.Create(refactoring.Description,
                token => ApplyFix(context, token), string.Empty);

            context.RegisterCodeFix(codeAction, context.Diagnostics);
            return Task.Delay(0);
        }

        public override ImmutableArray<string> FixableDiagnosticIds =>
            ImmutableArray.Create(refactoring.DiagnosticId);

        public sealed override FixAllProvider GetFixAllProvider()
        {
            return WellKnownFixAllProviders.BatchFixer;
        }

        private static SyntaxNode FormatRoot(SyntaxNode root)
        {
            return Formatter.Format(root, MSBuildWorkspace.Create());
        }

        private async Task<Document> ApplyFix(CodeFixContext context, CancellationToken cancellationToken)
        {
            var document = context.Document;

            try
            {
                var root = await document.GetSyntaxRootAsync(cancellationToken).ConfigureAwait(false);
                var replaceableNode = GetReplaceableNode(context, root);

                if (replaceableNode == null)
                    return document.WithSyntaxRoot(root);

                var replaceNodes = refactoring
                    .GetFixableNodes(replaceableNode)
                    .ToArray();

                if (replaceNodes.Length == 0)
                    return document.WithSyntaxRoot(root);

                replaceNodes = replaceNodes
                    .ToArray();

                root = CreateNewRoot(root, replaceableNode, replaceNodes);
                return document.WithSyntaxRoot(root);
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
                return document;
            }
        }

        private SyntaxNode GetReplaceableNode(CodeFixContext context, SyntaxNode root)
        {
            var token = GetSelectedToken(context, root);
            return refactoring.GetReplaceableNode(token);
        }

        private static SyntaxToken GetSelectedToken(CodeFixContext context, SyntaxNode root)
        {
            var diagnostic = context.Diagnostics.First();
            return root.FindToken(diagnostic.Location.SourceSpan.Start);
        }

        private static SyntaxNode CreateNewRoot(SyntaxNode root, SyntaxNode replaceableNode, IReadOnlyCollection<SyntaxNode> replaceNodes)
        {
            return FormatRoot(replaceNodes.Count == 1
                            ? root.ReplaceNode(replaceableNode, replaceNodes.First())
                            : root.ReplaceNode(replaceableNode, replaceNodes));
        }
    }
}