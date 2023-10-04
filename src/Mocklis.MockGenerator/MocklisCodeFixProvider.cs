// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MocklisCodeFixProvider.cs">
//   SPDX-License-Identifier: MIT
//   Copyright © 2019-2023 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.MockGenerator;

#region Using Directives

using System.Collections.Immutable;
using System.Composition;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeActions;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Mocklis.MockGenerator.CodeGeneration;

#endregion

[ExportCodeFixProvider(LanguageNames.CSharp, Name = nameof(MocklisCodeFixProvider))]
[Shared]
public class MocklisCodeFixProvider : CodeFixProvider
{
    private const string MocklisEmptiedClass = "mocklis_emptied_class";

    private const string Title = "Update Mocklis Class";

    public sealed override ImmutableArray<string> FixableDiagnosticIds =>
        ImmutableArray.Create(MocklisAnalyzer.CreateDiagnosticId, MocklisAnalyzer.UpdateDiagnosticId);

    public sealed override FixAllProvider? GetFixAllProvider() => null;

    public sealed override async Task RegisterCodeFixesAsync(CodeFixContext context)
    {
        var root = await context.Document.GetSyntaxRootAsync(context.CancellationToken).ConfigureAwait(false);

        if (root != null)
        {
            var diagnostic = context.Diagnostics.First();
            var diagnosticSpan = diagnostic.Location.SourceSpan;

            // Find the type declaration identified by the diagnostic.
            var parentSyntaxNode = root.FindToken(diagnosticSpan.Start).Parent;
            if (parentSyntaxNode != null)
            {
                var declaration = parentSyntaxNode.AncestorsAndSelf().OfType<TypeDeclarationSyntax>().First();

                // Register a code action that will invoke the fix.
                context.RegisterCodeFix(
                    CodeAction.Create(
                        title: Title,
                        createChangedSolution: c => UpdateMocklisClassAsync(context.Document, declaration, c),
                        equivalenceKey: Title),
                    diagnostic);
            }
        }
    }

    private static async Task<Solution> UpdateMocklisClassAsync(Document document, TypeDeclarationSyntax typeDecl,
        CancellationToken cancellationToken)
    {
        if (!(typeDecl is ClassDeclarationSyntax classDecl))
        {
            return document.Project.Solution;
        }

        var semanticModel = await document.GetSemanticModelAsync(cancellationToken).ConfigureAwait(false);

        if (semanticModel == null)
        {
            return document.Project.Solution;
        }

        MocklisSymbols mocklisSymbols = new MocklisSymbols(semanticModel.Compilation);

        bool isMocklisClass = classDecl.AttributeLists.SelectMany(al => al.Attributes)
            .Any(a =>
            {
                var attrSymbol = semanticModel.GetSymbolInfo(a).Symbol;
                return attrSymbol != null &&
                       attrSymbol.ContainingType.Equals(mocklisSymbols.MocklisClassAttribute, SymbolEqualityComparer.Default);
            });

        if (!isMocklisClass)
        {
            return document.Project.Solution;
        }

        var oldRoot = await document.GetSyntaxRootAsync(cancellationToken).ConfigureAwait(false);
        if (oldRoot == null)
        {
            return document.Project.Solution;
        }

        var emptyClassDecl = MocklisClass.EmptyMocklisClass(classDecl).WithAdditionalAnnotations(new SyntaxAnnotation(MocklisEmptiedClass));
        var emptyRoot = oldRoot.ReplaceNode(classDecl, emptyClassDecl);
        var emptyDoc = document.WithSyntaxRoot(emptyRoot);
        emptyRoot = await emptyDoc.GetSyntaxRootAsync(cancellationToken).ConfigureAwait(false);

        if (emptyRoot == null)
        {
            return document.Project.Solution;
        }

        semanticModel = await emptyDoc.GetSemanticModelAsync(cancellationToken).ConfigureAwait(false);

        if (semanticModel == null)
        {
            return document.Project.Solution;
        }

        // And find the class again
        emptyClassDecl = emptyRoot.DescendantNodesAndSelf().OfType<ClassDeclarationSyntax>()
            .FirstOrDefault(n => n.GetAnnotations(MocklisEmptiedClass).Any());

        if (emptyClassDecl == null)
        {
            return document.Project.Solution;
        }

        var newClassDecl = MocklisClass.UpdateMocklisClass(semanticModel, emptyClassDecl, mocklisSymbols);

        var newRoot = emptyRoot.ReplaceNode(emptyClassDecl, newClassDecl);
        return emptyDoc.WithSyntaxRoot(newRoot).Project.Solution;
    }
}
