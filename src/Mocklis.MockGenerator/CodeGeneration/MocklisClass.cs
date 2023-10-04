// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MocklisClass.cs">
//   SPDX-License-Identifier: MIT
//   Copyright © 2019-2023 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.MockGenerator.CodeGeneration
{
    #region Using Directives

    using System.Collections.Generic;
    using System.Threading;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp;
    using Microsoft.CodeAnalysis.CSharp.Syntax;
    using Microsoft.CodeAnalysis.Formatting;
    using F = Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

    #endregion

    public static class MocklisClass
    {
        private static readonly SyntaxTrivia[] Comments =
        {
            F.CarriageReturnLineFeed,
            F.Comment("// The contents of this class were created by the Mocklis code-generator."),
            F.CarriageReturnLineFeed,
            F.Comment("// Any changes you make will be overwritten if the contents are re-generated."),
            F.CarriageReturnLineFeed,
            F.CarriageReturnLineFeed
        };

        public static ClassDeclarationSyntax EmptyMocklisClass(ClassDeclarationSyntax classDecl)
        {
            return classDecl.WithMembers(F.List<MemberDeclarationSyntax>())
                .WithOpenBraceToken(F.Token(SyntaxKind.OpenBraceToken))
                .WithCloseBraceToken(F.Token(SyntaxKind.CloseBraceToken));
        }

        public static ClassDeclarationSyntax UpdateMocklisClass(SemanticModel semanticModel, ClassDeclarationSyntax classDecl,
            MocklisSymbols mocklisSymbols)
        {
            var classInformation = ExtractedClassInformation.BuildFromClassSymbol(classDecl, semanticModel, CancellationToken.None);

            var typesForSymbols = new MocklisTypesForSymbols(semanticModel, classDecl, mocklisSymbols, classInformation.NullableEnabled, classInformation.Settings, classInformation.ClassSymbol);

            return classDecl.WithMembers(F.List(classInformation.GenerateMembers(typesForSymbols)))
                .WithOpenBraceToken(F.Token(SyntaxKind.OpenBraceToken).WithTrailingTrivia(Comments))
                .WithCloseBraceToken(F.Token(SyntaxKind.CloseBraceToken))
                .WithAdditionalAnnotations(Formatter.Annotation)
                .WithAttributeLists(AddOrUpdateCodeGeneratedAttribute(typesForSymbols, semanticModel, mocklisSymbols, classDecl.AttributeLists));
        }

        private static SyntaxList<AttributeListSyntax> AddOrUpdateCodeGeneratedAttribute(MocklisTypesForSymbols typesForSymbols,
            SemanticModel semanticModel, MocklisSymbols mocklisSymbols, in SyntaxList<AttributeListSyntax> classDeclAttributeLists)
        {
            bool found = false;

            AttributeListSyntax FindInList(AttributeListSyntax originalAttributeList, bool add)
            {
                if (found)
                {
                    return originalAttributeList;
                }

                List<AttributeSyntax> attributes = new List<AttributeSyntax>();
                foreach (var attribute in originalAttributeList.Attributes)
                {
                    if (found)
                    {
                        attributes.Add(attribute);
                        continue;
                    }

                    var p = semanticModel.GetSymbolInfo(attribute).Symbol;
                    if (p != null && p.ContainingType.Equals(mocklisSymbols.GeneratedCodeAttribute, SymbolEqualityComparer.Default))
                    {
                        found = true;
                        attributes.Add(typesForSymbols.GeneratedCodeAttribute());
                    }
                    else if (add && p != null && p.ContainingType.Equals(mocklisSymbols.MocklisClassAttribute, SymbolEqualityComparer.Default))
                    {
                        found = true;
                        attributes.Add(attribute);
                        attributes.Add(typesForSymbols.GeneratedCodeAttribute());
                    }
                    else
                    {
                        attributes.Add(attribute);
                    }
                }

                if (found)
                {
                    var newAttributeList = F.AttributeList(F.SeparatedList(attributes));
                    if (originalAttributeList.HasLeadingTrivia)
                    {
                        newAttributeList = newAttributeList.WithLeadingTrivia(originalAttributeList.GetLeadingTrivia());
                    }

                    if (originalAttributeList.HasTrailingTrivia)
                    {
                        newAttributeList = newAttributeList.WithTrailingTrivia(originalAttributeList.GetTrailingTrivia());
                    }

                    return newAttributeList;
                }

                return found ? F.AttributeList(F.SeparatedList(attributes)) : originalAttributeList;
            }

            var result = new List<AttributeListSyntax>();
            foreach (var l in classDeclAttributeLists)
            {
                result.Add(FindInList(l, false));
            }

            if (!found)
            {
                result = new List<AttributeListSyntax>();
                foreach (var l in classDeclAttributeLists)
                {
                    result.Add(FindInList(l, true));
                }
            }

            return F.List(result);
        }
    }
}
