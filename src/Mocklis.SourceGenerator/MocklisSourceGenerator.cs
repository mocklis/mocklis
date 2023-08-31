// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MocklisSourceGenerator.cs">
//   SPDX-License-Identifier: MIT
//   Copyright © 2019-2020 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.SourceGenerator;

#region Using Directives

using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Mocklis.CodeGeneration;
using Mocklis.CodeGeneration.Compatibility;

#endregion


[Generator(LanguageNames.CSharp)]
public class MocklisSourceGenerator : IIncrementalGenerator
{


    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        IncrementalValuesProvider<ExtractedClassInformation> x = context.SyntaxProvider
            .ForAttributeWithMetadataName("Mocklis.Core.MocklisClassAttribute", Predicate, Transform)
            .Where(static a => a != null)
            .Select(static (a, _) => a!);
        // .WithComparer(EqualityComparer<MocklisClassInformation>.Default);

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
            return ExtractedClassInformation.BuildFromClassSymbol(cds, context.SemanticModel);
        }

        return null;
    }

    private void Execute(SourceProductionContext context, ExtractedClassInformation classInformation)
    {
        classInformation.AddSourceToContext(context);
    }
 

//        public void Execute(GeneratorExecutionContext context)
//        {
//            if (context.SyntaxContextReceiver is Blah blah)
//            {
//                foreach (var classInformation in blah.Populators)
//                {
//                    // begin creating the source we'll inject into the users compilation
//                    var sourceBuilder = new StringBuilder(@"using global::Mocklis.Core;

//namespace Mocklis.SourceGenerator.TestApp;

//public partial class Test : global::Mocklis.SourceGenerator.TestApp.ITest
//{
//    public Test()
//    {
//        GetAndSet = new PropertyMock<int>(this, ""TestClass"", ""ITest"", ""GetAndSet"", ""GetAndSet"", Strictness.Lenient);
//        SetOnly = new PropertyMock<int>(this, ""TestClass"", ""ITest"", ""SetOnly"", ""SetOnly"", Strictness.Lenient);
//        GetOnly = new PropertyMock<int>(this, ""TestClass"", ""ITest"", ""GetOnly"", ""GetOnly"", Strictness.Lenient);
//    }

//    public PropertyMock<int> GetAndSet { get; }
//    int global::Mocklis.SourceGenerator.TestApp.ITest.GetAndSet { get => GetAndSet.Value; set => GetAndSet.Value = value; }
//    public PropertyMock<int> SetOnly { get; }
//    int global::Mocklis.SourceGenerator.TestApp.ITest.SetOnly { set => SetOnly.Value = value; }
//    public PropertyMock<int> GetOnly { get; }
//    int global::Mocklis.SourceGenerator.TestApp.ITest.GetOnly => GetOnly.Value;
//}
//");

//                    // inject the created source into the users compilation
//                    context.AddSource("Test.g", SourceText.From(sourceBuilder.ToString(), Encoding.UTF8));

//                }


//            }
//        }
}

