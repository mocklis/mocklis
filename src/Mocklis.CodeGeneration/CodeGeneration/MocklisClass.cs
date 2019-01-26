// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MocklisClass.cs">
//   Copyright © 2019 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.CodeGeneration
{
    #region Using Directives

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
        public static ClassDeclarationSyntax EmptyMocklisClass(ClassDeclarationSyntax classDecl)
        {
            return classDecl.WithMembers(F.List<MemberDeclarationSyntax>())
                .WithOpenBraceToken(F.Token(SyntaxKind.OpenBraceToken))
                .WithCloseBraceToken(F.Token(SyntaxKind.CloseBraceToken));
        }

        public static ClassDeclarationSyntax UpdateMocklisClass(SemanticModel semanticModel, ClassDeclarationSyntax classDecl,
            MocklisSymbols mocklisSymbols)
        {
            var populator = new MocklisClassPopulator(semanticModel, classDecl, mocklisSymbols);
            return classDecl.WithMembers(F.List(populator.GenerateMembers()))
                .WithOpenBraceToken(F.Token(SyntaxKind.OpenBraceToken))
                .WithCloseBraceToken(F.Token(SyntaxKind.CloseBraceToken))
                .WithAdditionalAnnotations(Formatter.Annotation);
        }

        private class MocklisClassPopulator
        {
            private readonly INamedTypeSymbol _classSymbol;
            private readonly MocklisTypesForSymbols _typesForSymbols;
            private readonly IMemberMock[] _mocks;

            public MocklisClassPopulator(SemanticModel semanticModel, ClassDeclarationSyntax classDeclaration, MocklisSymbols mocklisSymbols)
            {
                _classSymbol = semanticModel.GetDeclaredSymbol(classDeclaration);
                _typesForSymbols = new MocklisTypesForSymbols(semanticModel, classDeclaration, mocklisSymbols);
                bool mockReturnsByRef = false;
                bool mockReturnsByRefReadonly = true;
                var attribute = _classSymbol.GetAttributes().Single(a => a.AttributeClass == mocklisSymbols.MocklisClassAttribute);
                foreach (var k in attribute.NamedArguments)
                {
                    switch (k.Key)
                    {
                        case "MockReturnsByRef":
                            if (k.Value.Value is bool newMockReturnsByRef)
                            {
                                mockReturnsByRef = newMockReturnsByRef;
                            }

                            break;
                        case "MockReturnsByRefReadonly":
                            if (k.Value.Value is bool newMockReturnsByRefReadonly)
                            {
                                mockReturnsByRefReadonly = newMockReturnsByRefReadonly;
                            }

                            break;
                    }
                }

                _mocks = CreateMocks(mocklisSymbols, _classSymbol, _typesForSymbols, mockReturnsByRef, mockReturnsByRefReadonly);
            }

            private static IMemberMock[] CreateMocks(MocklisSymbols mocklisSymbols, INamedTypeSymbol classSymbol,
                MocklisTypesForSymbols typesForSymbols, bool mockReturnsByRef,
                bool mockReturnsByRefReadonly)
            {
                var members = GetMembers(classSymbol).ToArray();

                // make sure to reserve and use all names defined by the basetypes, and the class itself
                var namesToReserveAndUse = new List<string>(classSymbol.BaseType.GetUsableNames()) { classSymbol.Name };
                var uniquifier = new Uniquifier(namesToReserveAndUse);

                // Then reserve all names used by new members
                foreach (var member in members)
                {
                    uniquifier.ReserveName(member.memberSymbol.MetadataName);
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
                                return new VirtualMethodBasedIndexerMock(typesForSymbols, classSymbol, interfaceSymbol, memberPropertySymbol,
                                    mockMemberName);
                            }

                            return new PropertyBasedIndexerMock(typesForSymbols, classSymbol, interfaceSymbol, memberPropertySymbol, mockMemberName);
                        }

                        if (useVirtualMethod)
                        {
                            return new VirtualMethodBasedPropertyMock(typesForSymbols, classSymbol, interfaceSymbol, memberPropertySymbol,
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
                            return new VirtualMethodBasedMethodMock(typesForSymbols, classSymbol, interfaceSymbol, memberMethodSymbol,
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
                        return null;
                }
            }

            private static string CreateMockProviderName(string memberName)
            {
                return "_" + char.ToLowerInvariant(memberName[0]) + memberName.Substring(1);
            }

            public SyntaxList<MemberDeclarationSyntax> GenerateMembers()
            {
                var declarationList = new List<MemberDeclarationSyntax>();

                GenerateConstructors(declarationList);

                foreach (var mock in _mocks)
                {
                    mock.AddMembersToClass(declarationList);
                }

                return new SyntaxList<MemberDeclarationSyntax>(declarationList);
            }

            private void GenerateConstructors(IList<MemberDeclarationSyntax> declarationList)
            {
                bool CanAccessConstructor(IMethodSymbol constructor)
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
                    mock.AddInitialisersToConstructor(constructorStatements);
                }

                var baseTypeConstructors = _classSymbol.BaseType.Constructors.Where(c => !c.IsStatic && !c.IsVararg && CanAccessConstructor(c))
                    .ToArray();

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
                        if (constructorStatement is ExpressionStatementSyntax expressionStatementSyntax
                            && expressionStatementSyntax.Expression is AssignmentExpressionSyntax assignmentExpressionSyntax
                            && assignmentExpressionSyntax.Left is IdentifierNameSyntax identifierNameSyntax
                            && parameterNames.Contains(identifierNameSyntax.Identifier.Text)
                        )
                        {
                            var newLeft = F.MemberAccessExpression(SyntaxKind.SimpleMemberAccessExpression, F.ThisExpression(),
                                identifierNameSyntax);
                            return expressionStatementSyntax.WithExpression(assignmentExpressionSyntax.WithLeft(newLeft));
                        }

                        return constructorStatement;
                    });

                    var constructorDeclaration = F.ConstructorDeclaration(F.Identifier(_classSymbol.Name))
                        .WithModifiers(F.TokenList(F.Token(_classSymbol.IsAbstract ? SyntaxKind.ProtectedKeyword : SyntaxKind.PublicKeyword)))
                        .WithParameterList(
                            F.ParameterList(
                                F.SeparatedList(constructor.Parameters.Select(tp => _typesForSymbols.AsParameterSyntax(tp, null)))))
                        .WithBody(F.Block(constructorStatementsWithThisWhereRequired));

                    if (parameterNames.Any())
                    {
                        constructorDeclaration = constructorDeclaration.WithInitializer(F.ConstructorInitializer(
                            SyntaxKind.BaseConstructorInitializer,
                            F.ArgumentList(F.SeparatedList(constructor.Parameters.Select(_typesForSymbols.AsArgumentSyntax)))));
                    }

                    declarationList.Add(constructorDeclaration);
                }
            }
        }
    }
}
