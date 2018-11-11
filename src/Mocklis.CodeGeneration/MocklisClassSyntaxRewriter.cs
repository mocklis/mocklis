// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MocklisClassSyntaxRewriter.cs">
//   Copyright © 2018 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.CodeGeneration
{
    #region Using Directives

    using System.Linq;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp;
    using Microsoft.CodeAnalysis.CSharp.Syntax;
    using Microsoft.CodeAnalysis.Formatting;

    #endregion

    public class MocklisClassSyntaxRewriter : CSharpSyntaxRewriter
    {
        private readonly SemanticModel _model;
        private readonly MocklisSymbols _mocklisSymbols;

        public MocklisClassSyntaxRewriter(SemanticModel model, MocklisSymbols mocklisSymbols)
        {
            _model = model;
            _mocklisSymbols = mocklisSymbols;
        }

        public override SyntaxNode VisitClassDeclaration(ClassDeclarationSyntax node)
        {
            bool isMocklisClass = node.AttributeLists.Any(
                al => al.Attributes.Any(
                    a => _model.GetSymbolInfo(a).Symbol.ContainingType == _mocklisSymbols.MocklisClassAttribute));

            if (isMocklisClass)
            {
                var mocklisClass = new MocklisClass(_model, node, _mocklisSymbols);
                return node.WithMembers(SyntaxFactory.List(mocklisClass.GenerateMembers()))
                    .WithOpenBraceToken(SyntaxFactory.Token(SyntaxKind.OpenBraceToken))
                    .WithCloseBraceToken(SyntaxFactory.Token(SyntaxKind.CloseBraceToken))
                    .WithAdditionalAnnotations(Formatter.Annotation);
            }

            return base.VisitClassDeclaration(node);
        }
    }
}
