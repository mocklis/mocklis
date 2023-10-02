// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MocklisAnalyzer.cs">
//   SPDX-License-Identifier: MIT
//   Copyright © 2019-2021 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.MockGenerator
{
    #region Using Directives

    using System.Collections.Immutable;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp;
    using Microsoft.CodeAnalysis.CSharp.Syntax;
    using Microsoft.CodeAnalysis.Diagnostics;

    #endregion

    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class MocklisAnalyzer : DiagnosticAnalyzer
    {
        public const string CreateDiagnosticId = "MocklisAnalyzerCreate";

        public const string UpdateDiagnosticId = "MocklisAnalyzerUpdate";

        private static readonly DiagnosticDescriptor CreateRule = new DiagnosticDescriptor(
            CreateDiagnosticId,
            title: "Generate mocklis code",
            messageFormat: "Mocklis code can be generated",
            category: "Code Generation",
            DiagnosticSeverity.Error,
            isEnabledByDefault: true,
            description: "Mocklis code can be generated.");


        private static readonly DiagnosticDescriptor UpdateRule = new DiagnosticDescriptor(
            UpdateDiagnosticId,
            title: "Regenerate mocklis code",
            messageFormat: "Mocklis code can be regenerated",
            category: "Code Generation",
            DiagnosticSeverity.Info,
            isEnabledByDefault: true,
            description: "Mocklis code can be regenerated.");

        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => ImmutableArray.Create(CreateRule, UpdateRule);

        public override void Initialize(AnalysisContext context)
        {
            context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.Analyze | GeneratedCodeAnalysisFlags.ReportDiagnostics);
            context.EnableConcurrentExecution();
            context.RegisterSyntaxNodeAction(AnalyzeNode, SyntaxKind.ClassDeclaration);
        }

        private static void AnalyzeNode(SyntaxNodeAnalysisContext context)
        {
            if (context.Node is ClassDeclarationSyntax classDecl && MightBeMocklisClass(classDecl, out var mocklisAttribute))
            {
                var regenerate = classDecl.Members.Any()
                                 || classDecl.OpenBraceToken.TrailingTrivia.Any(x => !string.IsNullOrWhiteSpace(x.ToString()))
                                 || classDecl.CloseBraceToken.LeadingTrivia.Any(x => !string.IsNullOrWhiteSpace(x.ToString()));

                // We know that if MightBeMocklisClass returns true, then mocklisAttribute is not null.
                var diagnostic = Diagnostic.Create(regenerate ? UpdateRule : CreateRule, mocklisAttribute!.GetLocation());
                context.ReportDiagnostic(diagnostic);
            }
        }

        private static bool MightBeMocklisClass(ClassDeclarationSyntax classDecl, out AttributeSyntax? mocklisAttribute)
        {
            mocklisAttribute = classDecl.AttributeLists.SelectMany(al => al.Attributes).FirstOrDefault(a =>
                a.Name.DescendantTokens().Any(t => t.Text == "MocklisClass" || t.Text == "MocklisClassAttribute"));

            var isPartial = classDecl.Modifiers.Any(m => m.IsKind(SyntaxKind.PartialKeyword));

            return mocklisAttribute != null && !isPartial;
        }
    }
}
