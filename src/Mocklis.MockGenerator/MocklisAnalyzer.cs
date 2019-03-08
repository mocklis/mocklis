// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MocklisAnalyzer.cs">
//   Copyright © 2019 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.MockGenerator
{
    #region Using Directives

    using System.Collections.Immutable;
    using System.Linq;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp;
    using Microsoft.CodeAnalysis.CSharp.Syntax;
    using Microsoft.CodeAnalysis.Diagnostics;

    #endregion

    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class MocklisAnalyzer : DiagnosticAnalyzer
    {
        public const string DiagnosticId = "MocklisAnalyzer";

        private static readonly DiagnosticDescriptor Rule = new DiagnosticDescriptor(
            DiagnosticId,
            title: "Regenerate mocklis code",
            messageFormat: "Mocklis code can be regenerated",
            category: "Code Updating",
            DiagnosticSeverity.Info,
            isEnabledByDefault: true,
            description: "Mocklis code can be regenerated");

        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => ImmutableArray.Create(Rule);

        public override void Initialize(AnalysisContext context)
        {
            context.RegisterSyntaxNodeAction(AnalyzeNode, SyntaxKind.ClassDeclaration);
        }

        private static void AnalyzeNode(SyntaxNodeAnalysisContext context)
        {
            if (context.Node is ClassDeclarationSyntax classDecl && MightBeMocklisClass(classDecl))
            {
                var diagnostic = Diagnostic.Create(Rule, context.Node.GetLocation());
                context.ReportDiagnostic(diagnostic);
            }
        }

        private static bool MightBeMocklisClass(ClassDeclarationSyntax classDecl)
        {
            var hasMocklisAttribute = classDecl.AttributeLists.SelectMany(al => al.Attributes).Any(a =>
                a.Name.DescendantTokens().Any(t => t.Text == "MocklisClass" || t.Text == "MocklisClassAttribute"));

            var isPartial = classDecl.Modifiers.Any(m => m.IsKind(SyntaxKind.PartialKeyword));

            return hasMocklisAttribute && !isPartial;
        }
    }
}
