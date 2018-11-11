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
            return classDecl.AttributeLists.SelectMany(al => al.Attributes).Any(a => a.Name.DescendantTokens().Any(t => t.Text == "MocklisClass"));
        }

        private async Task<Document> UpdateMocklisClassAsync(Document document, ClassDeclarationSyntax classDecl, CancellationToken cancellationToken)
        {
            var semanticModel = await document.GetSemanticModelAsync(cancellationToken);

            MocklisSymbols mocklisSymbols = new MocklisSymbols(semanticModel.Compilation);

            bool isMocklisClass = classDecl.AttributeLists.SelectMany(al => al.Attributes)
                .Any(a => semanticModel.GetSymbolInfo(a).Symbol.ContainingType == mocklisSymbols.MocklisClassAttribute);

            if (!isMocklisClass)
            {
                return document;
            }

            var newClassDecl = MocklisClass.UpdateMocklisClass(semanticModel, classDecl, mocklisSymbols);
            var oldRoot = await document.GetSyntaxRootAsync(cancellationToken).ConfigureAwait(false);
            var newRoot = oldRoot.ReplaceNode(classDecl, newClassDecl);
            return document.WithSyntaxRoot(newRoot);
        }
    }
}
