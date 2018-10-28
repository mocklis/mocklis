// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ProjectInspector.cs">
//   Copyright © 2018 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.CodeGeneration
{
    #region Using Directives

    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp.Syntax;
    using Microsoft.CodeAnalysis.Formatting;

    #endregion

    public static class ProjectInspector
    {
        public static async Task<Project> GenerateMocklisClassContents(Project project)
        {
            var compilation = await project.GetCompilationAsync();
            var symbols = new MocklisSymbols(compilation);

            // If we don't reference the right assemly, we could bail early.
            if (symbols.MocklisClassAttribute == null)
            {
                return project;
            }

            foreach (var documentId in project.DocumentIds)
            {
                var document = project.GetDocument(documentId);
                if (document.SourceCodeKind != SourceCodeKind.Regular)
                {
                    continue;
                }

                var syntaxTree = await document.GetSyntaxTreeAsync();
                var model = compilation.GetSemanticModel(syntaxTree);
                var rewriter = new MocklisClassSyntaxRewriter(model, symbols);

                var newBaseNode = rewriter.Visit(syntaxTree.GetRoot());
                document = document.WithSyntaxRoot(newBaseNode);

                var modifiedSpans = newBaseNode.DescendantNodes().OfType<ClassDeclarationSyntax>().Where(n => n.HasAnnotation(Formatter.Annotation))
                    .Select(a => a.Span);

                document = await Formatter.FormatAsync(document, modifiedSpans);

                project = document.Project;
            }

            return project;
        }
    }
}
