﻿using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeActions;
using Microsoft.CodeAnalysis.CodeFixes;
using Refactoring;
using System.Collections.Immutable;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.Formatting;
using Microsoft.CodeAnalysis.CSharp;
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
            const string refactoringMessage = "Apply refactoring";

            var codeAction = CodeAction.Create(refactoringMessage,
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
            var root = await document.GetSyntaxRootAsync(cancellationToken).ConfigureAwait(false);
            
            var diagnostic = context.Diagnostics.First();
            var token = root.FindToken(diagnostic.Location.SourceSpan.Start);
            var replaceableNode = refactoring.GetReplaceableNode(token);

            var replaceNodes = refactoring
                .ApplyFix(replaceableNode)
                .ToArray();

            replaceNodes = replaceNodes
                .Select(node => node
                    .NormalizeWhitespace()
                    .WithTrailingTrivia(SyntaxFactory.CarriageReturnLineFeed))
                    .ToArray();
            
            root = FormatRoot(replaceNodes.Length == 1 ?
                root.ReplaceNode(replaceableNode, replaceNodes.First()) : 
                root.ReplaceNode(replaceableNode, replaceNodes));

            return document.WithSyntaxRoot(root);
        }
    }
}