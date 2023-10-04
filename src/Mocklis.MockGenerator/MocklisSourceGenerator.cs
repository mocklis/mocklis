// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MocklisSourceGenerator.cs">
//   SPDX-License-Identifier: MIT
//   Copyright © 2019-2023 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.MockGenerator;

#region Using Directives

using System.Threading;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Mocklis.MockGenerator.CodeGeneration;

#endregion

[Generator(LanguageNames.CSharp)]
public class MocklisSourceGenerator : IIncrementalGenerator
{
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        IncrementalValuesProvider<ExtractedClassInformation> x = context.SyntaxProvider
            .ForAttributeWithMetadataName("Mocklis.Core.MocklisClassAttribute", Predicate, Transform)
            .Where(static a => a != null)
            .Select(static (a, _) => a!)
            .WithTrackingName("Syntax");

        context.RegisterSourceOutput(x, Execute);
    }

    private bool Predicate(SyntaxNode syntaxNode, CancellationToken cancellationToken)
    {
        return syntaxNode switch
        {
            // bail out quickly if the class isn't partial...
            ClassDeclarationSyntax cds when !cds.Modifiers.Any(SyntaxKind.PartialKeyword) => false,
            // Or if it is static
            ClassDeclarationSyntax cds when cds.Modifiers.Any(SyntaxKind.StaticKeyword) => false,
            // Otherwise return true;
            _ => true
        };
    }

    private ExtractedClassInformation? Transform(GeneratorAttributeSyntaxContext context, CancellationToken cancellationToken)
    {
        if (context.TargetNode is ClassDeclarationSyntax cds)
        {
            return ExtractedClassInformation.BuildFromClassSymbol(cds, context.SemanticModel, cancellationToken);
        }

        return null;
    }

    private void Execute(SourceProductionContext context, ExtractedClassInformation classInformation)
    {
        classInformation.AddSourceToContext(context);
    }
}
