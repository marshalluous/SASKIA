﻿using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RHW
{
    public class RoslynClassVisitor : CSharpSyntaxRewriter
    {
        public List<string> Classes { get; set; } = new List<string>();

        public override SyntaxNode VisitClassDeclaration(ClassDeclarationSyntax node)
        {
            node = (ClassDeclarationSyntax)base.VisitClassDeclaration(node);
        
            string className = node.Identifier.ValueText;
            Classes.Add(className); // save your visited classes
        
            return node;
        }
    }
}
