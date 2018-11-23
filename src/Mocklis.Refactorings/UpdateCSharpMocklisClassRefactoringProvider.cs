// --------------------------------------------------------------------------------------------------------------------
// <copyright file="UpdateCSharpMocklisClassRefactoringProvider.cs">
//   Copyright © 2018 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.Refactorings
{
    #region Using Directives

    using System.Composition;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CodeActions;
    using Microsoft.CodeAnalysis.CodeRefactorings;
    using Microsoft.CodeAnalysis.CSharp;
    using Microsoft.CodeAnalysis.CSharp.Syntax;
    using Mocklis.CodeGeneration;

    #endregion

    [ExportCodeRefactoringProvider(LanguageNames.CSharp, Name = nameof(UpdateCSharpMocklisClassRefactoringProvider))]
    [Shared]
    internal class UpdateCSharpMocklisClassRefactoringProvider : CodeRefactoringProvider
    {
        private const string MocklisEmptiedClass = "mocklis_emptied_class";

        public override async Task ComputeRefactoringsAsync(CodeRefactoringContext context)
        {
            var root = await context.Document.GetSyntaxRootAsync(context.CancellationToken).ConfigureAwait(false);

            var node = root.FindNode(context.Span);

            if (node is ClassDeclarationSyntax classDecl && MightBeMocklisClass(classDecl))
            {
                context.RegisterRefactoring(CodeAction.Create("Update Mocklis Class", c => UpdateMocklisClassAsync(context.Document, classDecl, c)));
            }
        }

        private bool MightBeMocklisClass(ClassDeclarationSyntax classDecl)
        {
            return classDecl.AttributeLists.SelectMany(al => al.Attributes).Any(a =>
                a.Name.DescendantTokens().Any(t => t.Text == "MocklisClass" || t.Text == "MocklisClassAttribute"));
        }

        private async Task<Document> UpdateMocklisClassAsync(Document document, ClassDeclarationSyntax classDecl, CancellationToken cancellationToken)
        {
            var semanticModel = await document.GetSemanticModelAsync(cancellationToken).ConfigureAwait(false);
            MocklisSymbols mocklisSymbols = new MocklisSymbols(semanticModel.Compilation);

            bool isMocklisClass = classDecl.AttributeLists.SelectMany(al => al.Attributes)
                .Any(a => semanticModel.GetSymbolInfo(a).Symbol.ContainingType == mocklisSymbols.MocklisClassAttribute);

            if (!isMocklisClass)
            {
                return document;
            }

            var oldRoot = await document.GetSyntaxRootAsync(cancellationToken).ConfigureAwait(false);

            var emptyClassDecl = MocklisClass.EmptyMocklisClass(classDecl).WithAdditionalAnnotations(new SyntaxAnnotation(MocklisEmptiedClass));
            var emptyRoot = oldRoot.ReplaceNode(classDecl, emptyClassDecl);
            var emptyDoc = document.WithSyntaxRoot(emptyRoot);
            emptyRoot = await emptyDoc.GetSyntaxRootAsync(cancellationToken).ConfigureAwait(false);

            semanticModel = await emptyDoc.GetSemanticModelAsync(cancellationToken).ConfigureAwait(false);

            // And find the class again
            emptyClassDecl = emptyRoot.DescendantNodesAndSelf().OfType<ClassDeclarationSyntax>()
                .FirstOrDefault(n => n.GetAnnotations("mocklis_emptied_class").Any());

            if (emptyClassDecl == null)
            {
                return document;
            }

            var newClassDecl = MocklisClass.UpdateMocklisClass(semanticModel, emptyClassDecl, mocklisSymbols);

            var newRoot = emptyRoot.ReplaceNode(emptyClassDecl, newClassDecl);
            return emptyDoc.WithSyntaxRoot(newRoot);
        }
    }
}
