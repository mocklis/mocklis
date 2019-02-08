// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MocklisAnalyzer.cs">
//   Copyright © 2019 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.Analyzer
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

        // You can change these strings in the Resources.resx file. If you do not want your analyzer to be localize-able, you can use regular strings for Title and MessageFormat.
        // See https://github.com/dotnet/roslyn/blob/master/docs/analyzers/Localizing%20Analyzers.md for more on localization
        private static readonly LocalizableString Title =
            new LocalizableResourceString(nameof(Resources.AnalyzerTitle), Resources.ResourceManager, typeof(Resources));

        private static readonly LocalizableString MessageFormat =
            new LocalizableResourceString(nameof(Resources.AnalyzerMessageFormat), Resources.ResourceManager, typeof(Resources));

        private static readonly LocalizableString Description =
            new LocalizableResourceString(nameof(Resources.AnalyzerDescription), Resources.ResourceManager, typeof(Resources));

        private const string Category = "Code Updating";

        private static readonly DiagnosticDescriptor Rule = new DiagnosticDescriptor(DiagnosticId, Title, MessageFormat, Category,
            DiagnosticSeverity.Info, isEnabledByDefault: true, description: Description);

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
