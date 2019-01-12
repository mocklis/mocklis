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

                // make sure to reserve and use all names defined by the basetypes
                var uniquifier = new Uniquifier(classSymbol.BaseType.GetUsableNames());

                // Then reserve all names used by new members
                foreach (var member in members)
                {
                    uniquifier.ReserveName(member.memberSymbol.MetadataName);
                }

                return members.Select(m =>
                    CreateMock(mocklisSymbols, classSymbol, m.memberSymbol, m.interfaceSymbol, uniquifier.GetUniqueName(m.memberSymbol.MetadataName),
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
                string mockMemberName, MocklisTypesForSymbols typesForSymbols, bool mockReturnsByRef, bool mockReturnsByRefReadonly)
            {
                switch (memberSymbol)
                {
                    case IPropertySymbol memberPropertySymbol:
                    {
                        bool useVirtualMethod = memberPropertySymbol.ReturnsByRef && !mockReturnsByRef ||
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
                        // we need to use a virtual method if this is a vararg or if any of the parameters (or return type) are restricted types.
                        var hasRestrictedParameter = memberMethodSymbol.Parameters.Any(p => !mocklisSymbols.HasImplicitConversionToObject(p.Type));
                        var hasRestrictedReturnType = !memberMethodSymbol.ReturnsVoid &&
                                                      !mocklisSymbols.HasImplicitConversionToObject(memberMethodSymbol.ReturnType);

                        bool useVirtualMethod = memberMethodSymbol.Arity > 0 || hasRestrictedParameter || hasRestrictedReturnType ||
                                                memberMethodSymbol.IsVararg;

                        useVirtualMethod = useVirtualMethod || memberMethodSymbol.ReturnsByRef && !mockReturnsByRef;
                        useVirtualMethod = useVirtualMethod || memberMethodSymbol.ReturnsByRefReadonly && !mockReturnsByRefReadonly;

                        if (useVirtualMethod)
                        {
                            return new VirtualMethodBasedMethodMock(typesForSymbols, classSymbol, interfaceSymbol, memberMethodSymbol,
                                mockMemberName);
                        }

                        return new PropertyBasedMethodMock(typesForSymbols, classSymbol, interfaceSymbol, memberMethodSymbol, mockMemberName);
                    }
                    default:
                        return null;
                }
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
                var constructorStatements = new List<StatementSyntax>();

                foreach (var mock in _mocks)
                {
                    mock.AddInitialisersToConstructor(constructorStatements);
                }

                foreach (var constructor in _classSymbol.BaseType.Constructors)
                {
                    if (constructor.IsStatic || constructor.IsVararg)
                    {
                        continue;
                    }

                    switch (constructor.DeclaredAccessibility)
                    {
                        case Accessibility.Protected:
                        case Accessibility.ProtectedOrInternal:
                        case Accessibility.Public:
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

                            declarationList.Add(F.ConstructorDeclaration(F.Identifier(_classSymbol.Name))
                                .WithModifiers(F.TokenList(F.Token(_classSymbol.IsAbstract ? SyntaxKind.ProtectedKeyword : SyntaxKind.PublicKeyword)))
                                .WithParameterList(
                                    F.ParameterList(F.SeparatedList(constructor.Parameters.Select(_typesForSymbols.AsParameterSyntax))))
                                .WithInitializer(F.ConstructorInitializer(SyntaxKind.BaseConstructorInitializer,
                                    F.ArgumentList(F.SeparatedList(constructor.Parameters.Select(_typesForSymbols.AsArgumentSyntax)))))
                                .WithBody(F.Block(constructorStatementsWithThisWhereRequired)));
                            break;
                        }
                    }
                }
            }
        }
    }
}
