// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ProjectInspector.cs">
//   SPDX-License-Identifier: MIT
//   Copyright © 2019 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.Cli
{
    #region Using Directives

    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp;
    using Microsoft.CodeAnalysis.CSharp.Syntax;
    using Microsoft.CodeAnalysis.Formatting;
    using Mocklis.CodeGeneration;
    using Mocklis.CodeGeneration.Compatibility;

    #endregion

    public static class ProjectInspector
    {
        private abstract class MocklisRewriterBase : CSharpSyntaxRewriter
        {
            protected SemanticModel Model { get; }
            protected MocklisSymbols MocklisSymbols { get; }

            protected MocklisRewriterBase(SemanticModel model, MocklisSymbols mocklisSymbols)
            {
                Model = model;
                MocklisSymbols = mocklisSymbols;
            }

            protected bool ShouldRewriteClass(ClassDeclarationSyntax node)
            {
                bool hasMocklisAttribute = node.AttributeLists.Any(
                    al => al.Attributes.Any(
                        a => ModelExtensions.GetSymbolInfo(Model, a).Symbol.ContainingType.Equals(MocklisSymbols.MocklisClassAttribute)));

                if (!hasMocklisAttribute)
                {
                    return false;
                }

                bool isPartial = node.Modifiers.Any(m => m.IsKind(SyntaxKind.PartialKeyword));

                return !isPartial;
            }
        }

        private class MocklisClassEmptier : MocklisRewriterBase
        {
            public bool FoundMocklisClass { get; private set; }

            public MocklisClassEmptier(SemanticModel model, MocklisSymbols mocklisSymbols) : base(model, mocklisSymbols)
            {
            }

            public override SyntaxNode VisitClassDeclaration(ClassDeclarationSyntax node)
            {
                if (ShouldRewriteClass(node))
                {
                    FoundMocklisClass = true;
                    return MocklisClass.EmptyMocklisClass(node);
                }

                return base.VisitClassDeclaration(node);
            }
        }

        private class MocklisClassSyntaxRewriter : MocklisRewriterBase
        {
            public MocklisClassSyntaxRewriter(SemanticModel model, MocklisSymbols mocklisSymbols) : base(model, mocklisSymbols)
            {
            }

            public override SyntaxNode VisitClassDeclaration(ClassDeclarationSyntax node)
            {
                if (ShouldRewriteClass(node))
                {
                    return MocklisClass.UpdateMocklisClass(Model, node, MocklisSymbols, Model.ClassIsInNullableContext(node));
                }

                return base.VisitClassDeclaration(node);
            }
        }

        public static async Task<Project> GenerateMocklisClassContents(Project project, CancellationToken cancellationToken = default)
        {
            var compilation = await project.GetCompilationAsync(cancellationToken).ConfigureAwait(false);
            if (compilation == null)
            {
                return project;
            }

            var symbols = new MocklisSymbols(compilation);

            // If we don't reference the right assembly, we could bail early.
            if (symbols.MocklisClassAttribute == null)
            {
                return project;
            }

            foreach (var documentId in project.DocumentIds)
            {
                cancellationToken.ThrowIfCancellationRequested();

                var document = project.GetDocument(documentId);
                if (document == null || document.SourceCodeKind != SourceCodeKind.Regular)
                {
                    continue;
                }

                var syntaxTree = await document.GetSyntaxTreeAsync(cancellationToken).ConfigureAwait(false);
                var semanticModel = compilation.GetSemanticModel(syntaxTree);
                var emptier = new MocklisClassEmptier(semanticModel, symbols);

                var syntaxRoot = emptier.Visit(syntaxTree.GetRoot());
                if (!emptier.FoundMocklisClass)
                {
                    continue;
                }

                document = document.WithSyntaxRoot(syntaxRoot);
                syntaxTree = await document.GetSyntaxTreeAsync(cancellationToken).ConfigureAwait(false);
                semanticModel = await document.GetSemanticModelAsync(cancellationToken).ConfigureAwait(false);

                var rewriter = new MocklisClassSyntaxRewriter(semanticModel, symbols);

                syntaxRoot = rewriter.Visit(syntaxTree.GetRoot());
                document = document.WithSyntaxRoot(syntaxRoot);

                var modifiedSpans = syntaxRoot.DescendantNodes().OfType<ClassDeclarationSyntax>().Where(n => n.HasAnnotation(Formatter.Annotation))
                    .Select(a => a.Span);

                document = await Formatter.FormatAsync(document, modifiedSpans, cancellationToken: cancellationToken).ConfigureAwait(false);

                project = document.Project;
            }

            return project;
        }
    }
}
