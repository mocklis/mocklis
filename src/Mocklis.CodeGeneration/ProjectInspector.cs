// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ProjectInspector.cs">
//   Copyright © 2018 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.CodeGeneration
{
    #region Using Directives

    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp;
    using Microsoft.CodeAnalysis.CSharp.Syntax;
    using Microsoft.CodeAnalysis.Formatting;

    #endregion

    public static class ProjectInspector
    {
        private class MocklisClassEmptier : CSharpSyntaxRewriter
        {
            private readonly SemanticModel _model;
            private readonly MocklisSymbols _mocklisSymbols;
            public bool FoundMocklisClass { get; private set; }

            public MocklisClassEmptier(SemanticModel model, MocklisSymbols mocklisSymbols)
            {
                _model = model;
                _mocklisSymbols = mocklisSymbols;
            }

            public override SyntaxNode VisitClassDeclaration(ClassDeclarationSyntax node)
            {
                bool isMocklisClass = node.AttributeLists.Any(
                    al => al.Attributes.Any(
                        a => ModelExtensions.GetSymbolInfo(_model, a).Symbol.ContainingType == _mocklisSymbols.MocklisClassAttribute));

                if (isMocklisClass)
                {
                    FoundMocklisClass = true;
                    return MocklisClass.EmptyMocklisClass(node);
                }

                return base.VisitClassDeclaration(node);
            }
        }

        private class MocklisClassSyntaxRewriter : CSharpSyntaxRewriter
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
                        a => ModelExtensions.GetSymbolInfo(_model, a).Symbol.ContainingType == _mocklisSymbols.MocklisClassAttribute));

                if (isMocklisClass)
                {
                    return MocklisClass.UpdateMocklisClass(_model, node, _mocklisSymbols);
                }

                return base.VisitClassDeclaration(node);
            }
        }

        public static async Task<Project> GenerateMocklisClassContents(Project project, CancellationToken cancellationToken = default)
        {
            var compilation = await project.GetCompilationAsync(cancellationToken).ConfigureAwait(false);
            var symbols = new MocklisSymbols(compilation);

            // If we don't reference the right assemly, we could bail early.
            if (symbols.MocklisClassAttribute == null)
            {
                return project;
            }

            foreach (var documentId in project.DocumentIds)
            {
                cancellationToken.ThrowIfCancellationRequested();

                var document = project.GetDocument(documentId);
                if (document.SourceCodeKind != SourceCodeKind.Regular)
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
