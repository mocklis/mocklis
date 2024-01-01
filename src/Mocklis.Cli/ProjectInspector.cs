// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ProjectInspector.cs">
//   SPDX-License-Identifier: MIT
//   Copyright © 2019-2023 Esbjörn Redmo and contributors. All rights reserved.
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
    using Mocklis.MockGenerator.CodeGeneration;
    using Mocklis.MockGenerator.CodeGeneration.Compatibility;

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
                        a =>
                        {
                            var attrSymbol = Model.GetSymbolInfo(a).Symbol;
                            return attrSymbol != null &&
                                   attrSymbol.ContainingType.Equals(MocklisSymbols.MocklisClassAttribute, SymbolEqualityComparer.Default);
                        }));

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

            public override SyntaxNode? VisitClassDeclaration(ClassDeclarationSyntax node)
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

            public override SyntaxNode? VisitClassDeclaration(ClassDeclarationSyntax node)
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

            foreach (var documentId in project.DocumentIds)
            {
                cancellationToken.ThrowIfCancellationRequested();

                var document = project.GetDocument(documentId);
                if (!(document is { SourceCodeKind: SourceCodeKind.Regular }))
                {
                    continue;
                }

                var syntaxTree = await document.GetSyntaxTreeAsync(cancellationToken).ConfigureAwait(false);
                if (syntaxTree == null)
                {
                    continue;
                }

                var semanticModel = compilation.GetSemanticModel(syntaxTree);
                var emptier = new MocklisClassEmptier(semanticModel, symbols);

                var root = await syntaxTree.GetRootAsync(cancellationToken).ConfigureAwait(false);
                var syntaxRoot = emptier.Visit(root);
                if (!emptier.FoundMocklisClass)
                {
                    continue;
                }

                document = document.WithSyntaxRoot(syntaxRoot);
                syntaxTree = await document.GetSyntaxTreeAsync(cancellationToken).ConfigureAwait(false);

                if (syntaxTree == null)
                {
                    continue;
                }

                semanticModel = await document.GetSemanticModelAsync(cancellationToken).ConfigureAwait(false);

                if (semanticModel == null)
                {
                    continue;
                }

                var rewriter = new MocklisClassSyntaxRewriter(semanticModel, symbols);

                root = await syntaxTree.GetRootAsync(cancellationToken).ConfigureAwait(false);
                syntaxRoot = rewriter.Visit(root);
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
