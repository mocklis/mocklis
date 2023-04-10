// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MocklisClass.cs">
//   SPDX-License-Identifier: MIT
//   Copyright © 2019-2021 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.CodeGeneration
{
    #region Using Directives

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp;
    using Microsoft.CodeAnalysis.CSharp.Syntax;
    using Microsoft.CodeAnalysis.Formatting;
    using Mocklis.CodeGeneration.UniqueNames;
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
            MocklisSymbols mocklisSymbols, bool nullableContextEnabled)
        {
            var typesForSymbols = new MocklisTypesForSymbols(semanticModel, classDecl, mocklisSymbols, nullableContextEnabled);

            var populator = new MocklisClassPopulator(typesForSymbols, semanticModel, classDecl, mocklisSymbols);
            return classDecl.WithMembers(F.List(populator.GenerateMembers(typesForSymbols)))
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

        private class MocklisClassPopulator
        {
            private readonly INamedTypeSymbol _classSymbol;
            private readonly MocklisTypesForSymbols _typesForSymbols;
            private readonly IMemberMock[] _mocks;

            private readonly bool _strict;
            private readonly bool _veryStrict;

            public MocklisClassPopulator(MocklisTypesForSymbols typesForSymbols, SemanticModel semanticModel, ClassDeclarationSyntax classDeclaration,
                MocklisSymbols mocklisSymbols)
            {
                _classSymbol = semanticModel.GetDeclaredSymbol(classDeclaration) ?? throw new ArgumentException("symbol for class was not found in semantic model.", nameof(classDeclaration));
                _typesForSymbols = typesForSymbols;
                bool mockReturnsByRef = false;
                bool mockReturnsByRefReadonly = true;
                bool strict = false;
                bool veryStrict = false;
                var attribute = _classSymbol.GetAttributes().SingleOrDefault(a => a.AttributeClass != null && a.AttributeClass.Equals(mocklisSymbols.MocklisClassAttribute, SymbolEqualityComparer.Default));

                if (attribute != null)
                {
                    var attributeSyntaxReference = attribute.ApplicationSyntaxReference ?? throw new ArgumentException("MocklisClass attribute did not have syntax reference.");

                    var attributeArguments = classDeclaration.FindNode(attributeSyntaxReference.Span).DescendantNodes().OfType<AttributeArgumentSyntax>();

                    foreach (var k in attributeArguments)
                    {
                        if (k.Expression is LiteralExpressionSyntax les)
                        {
                            var name = k.NameEquals?.Name.Identifier.Text;
                            bool value;
                            if (les.IsKind(SyntaxKind.TrueLiteralExpression))
                            {
                                value = true;
                            }
                            else if (les.IsKind(SyntaxKind.FalseLiteralExpression))
                            {
                                value = false;
                            }
                            else
                            {
                                break;
                            }

                            switch (name)
                            {
                                case "MockReturnsByRef":
                                    mockReturnsByRef = value;
                                    break;
                                case "MockReturnsByRefReadonly":
                                    mockReturnsByRefReadonly = value;
                                    break;
                                case "Strict":
                                    strict = value;
                                    break;
                                case "VeryStrict":
                                    veryStrict = value;
                                    break;
                            }
                        }
                    }
                }

                // 'Very strict' implies 'strict'
                strict |= veryStrict;

                _strict = strict;
                _veryStrict = veryStrict;

                _mocks = CreateMocks(mocklisSymbols, _classSymbol, _typesForSymbols, mockReturnsByRef, mockReturnsByRefReadonly);
            }

            private static IMemberMock[] CreateMocks(MocklisSymbols mocklisSymbols, INamedTypeSymbol classSymbol,
                MocklisTypesForSymbols typesForSymbols, bool mockReturnsByRef,
                bool mockReturnsByRefReadonly)
            {
                var members = GetMembers(classSymbol).ToArray();

                // make sure to reserve and use all names defined by the base types, and the class itself
                var namesToReserveAndUse = new List<string>(classSymbol.BaseType?.GetUsableNames() ?? Array.Empty<string>()) { classSymbol.Name };
                var uniquifier = new Uniquifier(namesToReserveAndUse);

                // Then reserve all names used by new members
                foreach (var (_, memberSymbol) in members)
                {
                    uniquifier.ReserveName(memberSymbol.MetadataName);
                }

                return members.Select(m =>
                    CreateMock(mocklisSymbols, classSymbol, m.memberSymbol, m.interfaceSymbol, uniquifier,
                        typesForSymbols, mockReturnsByRef, mockReturnsByRefReadonly)).ToArray();
            }

            private static IEnumerable<(INamedTypeSymbol interfaceSymbol, ISymbol memberSymbol)> GetMembers(ITypeSymbol classSymbol)
            {
                var baseTypeSymbol = classSymbol.BaseType;
                foreach (var interfaceSymbol in classSymbol.AllInterfaces)
                {
                    foreach (var memberSymbol in interfaceSymbol.GetMembers())
                    {
                        if (!memberSymbol.IsAbstract)
                        {
                            continue;
                        }

                        if (memberSymbol is IMethodSymbol && !memberSymbol.CanBeReferencedByName)
                        {
                            continue;
                        }

                        if (baseTypeSymbol?.FindImplementationForInterfaceMember(memberSymbol) != null)
                        {
                            continue;
                        }

                        yield return (interfaceSymbol, memberSymbol);
                    }
                }
            }

            private static IMemberMock CreateMock(MocklisSymbols mocklisSymbols, INamedTypeSymbol classSymbol, ISymbol memberSymbol,
                INamedTypeSymbol interfaceSymbol,
                Uniquifier uniquifier, MocklisTypesForSymbols typesForSymbols, bool mockReturnsByRef, bool mockReturnsByRefReadonly)
            {
                string mockMemberName = uniquifier.GetUniqueName(memberSymbol.MetadataName);

                switch (memberSymbol)
                {
                    case IPropertySymbol memberPropertySymbol:
                    {
                        var hasRestrictedParameter = memberPropertySymbol.Parameters.Any(p => !mocklisSymbols.HasImplicitConversionToObject(p.Type));
                        var hasRestrictedReturnType = !mocklisSymbols.HasImplicitConversionToObject(memberPropertySymbol.Type);

                        bool useVirtualMethod = hasRestrictedParameter || hasRestrictedReturnType ||
                                                memberPropertySymbol.ReturnsByRef && !mockReturnsByRef ||
                                                memberPropertySymbol.ReturnsByRefReadonly && !mockReturnsByRefReadonly;

                        if (memberPropertySymbol.IsIndexer)
                        {
                            if (useVirtualMethod)
                            {
                                return new VirtualMethodBasedIndexerMock(classSymbol, interfaceSymbol, memberPropertySymbol,
                                    mockMemberName);
                            }

                            return new PropertyBasedIndexerMock(typesForSymbols, classSymbol, interfaceSymbol, memberPropertySymbol, mockMemberName);
                        }

                        if (useVirtualMethod)
                        {
                            return new VirtualMethodBasedPropertyMock(classSymbol, interfaceSymbol, memberPropertySymbol,
                                mockMemberName);
                        }

                        return new PropertyBasedPropertyMock(typesForSymbols, classSymbol, interfaceSymbol, memberPropertySymbol, mockMemberName);
                    }

                    case IEventSymbol memberEventSymbol:
                        return new PropertyBasedEventMock(typesForSymbols, classSymbol, interfaceSymbol, memberEventSymbol, mockMemberName);
                    case IMethodSymbol memberMethodSymbol:
                    {
                        var hasRestrictedParameter = memberMethodSymbol.Parameters.Any(p => !mocklisSymbols.HasImplicitConversionToObject(p.Type));
                        var hasRestrictedReturnType = !memberMethodSymbol.ReturnsVoid &&
                                                      !mocklisSymbols.HasImplicitConversionToObject(memberMethodSymbol.ReturnType);

                        bool useVirtualMethod = hasRestrictedParameter || hasRestrictedReturnType || memberMethodSymbol.IsVararg;

                        useVirtualMethod = useVirtualMethod || memberMethodSymbol.ReturnsByRef && !mockReturnsByRef;
                        useVirtualMethod = useVirtualMethod || memberMethodSymbol.ReturnsByRefReadonly && !mockReturnsByRefReadonly;

                        if (useVirtualMethod)
                        {
                            return new VirtualMethodBasedMethodMock(classSymbol, interfaceSymbol, memberMethodSymbol,
                                mockMemberName);
                        }

                        if (memberMethodSymbol.Arity > 0)
                        {
                            var mockProviderName = uniquifier.GetUniqueName(CreateMockProviderName(memberSymbol.MetadataName));
                            return new PropertyBasedMethodMockWithTypeParameters(typesForSymbols, classSymbol, interfaceSymbol, memberMethodSymbol,
                                mockMemberName, mockProviderName);
                        }

                        return new PropertyBasedMethodMock(typesForSymbols, classSymbol, interfaceSymbol, memberMethodSymbol, mockMemberName);
                    }

                    default:
                        return NullMemberMock.Instance;
                }
            }

            private static string CreateMockProviderName(string memberName)
            {
                return "_" + char.ToLowerInvariant(memberName[0]) + memberName.Substring(1);
            }

            public SyntaxList<MemberDeclarationSyntax> GenerateMembers(MocklisTypesForSymbols typesForSymbols)
            {
                var declarationList = new List<MemberDeclarationSyntax>();

                GenerateConstructors(declarationList, typesForSymbols);

                foreach (var mock in _mocks)
                {
                    mock.AddMembersToClass(declarationList, typesForSymbols, _strict, _veryStrict);
                }

                return new SyntaxList<MemberDeclarationSyntax>(declarationList);
            }

            private void GenerateConstructors(IList<MemberDeclarationSyntax> declarationList, MocklisTypesForSymbols typesForSymbols)
            {
                static bool CanAccessConstructor(IMethodSymbol constructor)
                {
                    switch (constructor.DeclaredAccessibility)
                    {
                        case Accessibility.Protected:
                        case Accessibility.ProtectedOrInternal:
                        case Accessibility.Public:
                        {
                            return true;
                        }

                        default:
                        {
                            return false;
                        }
                    }
                }

                var constructorStatements = new List<StatementSyntax>();

                foreach (var mock in _mocks)
                {
                    mock.AddInitialisersToConstructor(constructorStatements, typesForSymbols, _strict, _veryStrict);
                }

                var baseTypeConstructors = _classSymbol.BaseType?.Constructors.Where(c => !c.IsStatic && !c.IsVararg && CanAccessConstructor(c))
                    .ToArray() ?? Array.Empty<IMethodSymbol>();

                if (baseTypeConstructors.Length == 1 && baseTypeConstructors[0].Parameters.Length == 0 && constructorStatements.Count == 0)
                {
                    // This would correspond to emitting just a default constructor, which we don't really need to do.
                    return;
                }

                foreach (var constructor in baseTypeConstructors)
                {
                    var parameterNames = constructor.Parameters.Select(p => p.Name).ToArray();

                    var constructorStatementsWithThisWhereRequired = constructorStatements.Select(constructorStatement =>
                    {
                        if (constructorStatement is ExpressionStatementSyntax { Expression: AssignmentExpressionSyntax { Left: IdentifierNameSyntax identifierNameSyntax } assignmentExpressionSyntax } expressionStatementSyntax && parameterNames.Contains(identifierNameSyntax.Identifier.Text)
                        )
                        {
                            var newLeft = F.MemberAccessExpression(SyntaxKind.SimpleMemberAccessExpression, F.ThisExpression(), identifierNameSyntax);
                            return expressionStatementSyntax.WithExpression(assignmentExpressionSyntax.WithLeft(newLeft));
                        }

                        return constructorStatement;
                    });

                    var constructorDeclaration = F.ConstructorDeclaration(F.Identifier(_classSymbol.Name))
                        .WithModifiers(F.TokenList(F.Token(_classSymbol.IsAbstract ? SyntaxKind.ProtectedKeyword : SyntaxKind.PublicKeyword)))
                        .WithParameterList(
                            F.ParameterList(
                                F.SeparatedList(constructor.Parameters.Select(tp => _typesForSymbols.AsParameterSyntax(tp)))))
                        .WithBody(F.Block(constructorStatementsWithThisWhereRequired));

                    if (parameterNames.Any())
                    {
                        constructorDeclaration = constructorDeclaration.WithInitializer(F.ConstructorInitializer(
                            SyntaxKind.BaseConstructorInitializer,
                            F.ArgumentList(constructor.Parameters.AsArgumentList())));
                    }

                    declarationList.Add(constructorDeclaration);
                }
            }
        }
    }
}
