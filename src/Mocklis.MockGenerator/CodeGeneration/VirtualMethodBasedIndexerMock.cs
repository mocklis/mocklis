// --------------------------------------------------------------------------------------------------------------------
// <copyright file="VirtualMethodBasedIndexerMock.cs">
//   SPDX-License-Identifier: MIT
//   Copyright © 2019-2023 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.MockGenerator.CodeGeneration
{
    #region Using Directives

    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp;
    using Microsoft.CodeAnalysis.CSharp.Syntax;
    using Mocklis.MockGenerator.CodeGeneration.Compatibility;
    using F = Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

    #endregion

    public sealed class VirtualMethodBasedIndexerMock : VirtualMethodBasedMock<IPropertySymbol>, IMemberMock
    {
        public VirtualMethodBasedIndexerMock(INamedTypeSymbol classSymbol, INamedTypeSymbol interfaceSymbol, IPropertySymbol symbol, string mockMemberName) : base(classSymbol, interfaceSymbol, symbol, mockMemberName)
        {
        }
        
        public ISyntaxAdder GetSyntaxAdder(MocklisTypesForSymbols typesForSymbols, bool strict, bool veryStrict)
        {
            return new SyntaxAdder(this, typesForSymbols);
        }

        private class SyntaxAdder : ISyntaxAdder
        {
            private readonly VirtualMethodBasedIndexerMock _mock;
            private readonly MocklisTypesForSymbols _typesForSymbols;

            public SyntaxAdder(VirtualMethodBasedIndexerMock mock, MocklisTypesForSymbols typesForSymbols)
            {
                _mock = mock;
                _typesForSymbols = typesForSymbols;
            }


            public void AddMembersToClass(IList<MemberDeclarationSyntax> declarationList)
            {
                var valueTypeSyntax = _typesForSymbols.ParseTypeName(_mock.Symbol.Type, _mock.Symbol.NullableOrOblivious());
                var valueWithReadonlyTypeSyntax = valueTypeSyntax;

                if (_mock.Symbol.ReturnsByRef || _mock.Symbol.ReturnsByRefReadonly)
                {
                    RefTypeSyntax tmp = F.RefType(valueTypeSyntax);
                    valueTypeSyntax = tmp;
                    valueWithReadonlyTypeSyntax = tmp;
                    if (_mock.Symbol.ReturnsByRefReadonly)
                    {
                        valueWithReadonlyTypeSyntax = tmp.WithReadOnlyKeyword(F.Token(SyntaxKind.ReadOnlyKeyword));
                    }
                }

                if (!_mock.Symbol.IsWriteOnly)
                {
                    declarationList.Add(MockGetVirtualMethod(_typesForSymbols, valueTypeSyntax));
                }

                if (!_mock.Symbol.IsReadOnly)
                {
                    declarationList.Add(MockSetVirtualMethod(_typesForSymbols, valueTypeSyntax));
                }

                declarationList.Add(ExplicitInterfaceMember(_typesForSymbols, valueWithReadonlyTypeSyntax));
            }

            public void AddInitialisersToConstructor(List<StatementSyntax> constructorStatements)
            {
            }

            private MemberDeclarationSyntax MockGetVirtualMethod(MocklisTypesForSymbols typesForSymbols, TypeSyntax valueTypeSyntax)
            {
                return F.MethodDeclaration(valueTypeSyntax, F.Identifier(_mock.MemberMockName))
                    .WithModifiers(F.TokenList(F.Token(SyntaxKind.ProtectedKeyword), F.Token(SyntaxKind.VirtualKeyword)))
                    .WithParameterList(F.ParameterList(F.SeparatedList(_mock.Symbol.Parameters.Select(a =>
                        F.Parameter(F.Identifier(a.Name)).WithType(typesForSymbols.ParseTypeName(a.Type, a.NullableOrOblivious()))))))
                    .WithBody(F.Block(_mock.ThrowMockMissingStatement(typesForSymbols, "VirtualIndexerGet")));
            }

            private MemberDeclarationSyntax MockSetVirtualMethod(MocklisTypesForSymbols typesForSymbols, TypeSyntax valueTypeSyntax)
            {
                var uniquifier = new Uniquifier(_mock.Symbol.Parameters.Select(p => p.Name));

                var parameterList = F.SeparatedList(_mock.Symbol.Parameters.Select(a =>
                        F.Parameter(F.Identifier(a.Name)).WithType(typesForSymbols.ParseTypeName(a.Type, a.NullableOrOblivious()))))
                    .Add(F.Parameter(F.Identifier(uniquifier.GetUniqueName("value"))).WithType(valueTypeSyntax));

                return F.MethodDeclaration(F.PredefinedType(F.Token(SyntaxKind.VoidKeyword)), F.Identifier(_mock.MemberMockName))
                    .WithModifiers(F.TokenList(F.Token(SyntaxKind.ProtectedKeyword), F.Token(SyntaxKind.VirtualKeyword)))
                    .WithParameterList(F.ParameterList(parameterList))
                    .WithBody(F.Block(_mock.ThrowMockMissingStatement(typesForSymbols, "VirtualIndexerSet")));
            }

            private MemberDeclarationSyntax ExplicitInterfaceMember(MocklisTypesForSymbols typesForSymbols, TypeSyntax valueWithReadonlyTypeSyntax)
            {
                var mockedIndexer = F.IndexerDeclaration(valueWithReadonlyTypeSyntax)
                    .WithParameterList(F.BracketedParameterList(F.SeparatedList(_mock.Symbol.Parameters.Select(a =>
                        F.Parameter(F.Identifier(a.Name)).WithType(typesForSymbols.ParseTypeName(a.Type, a.NullableOrOblivious()))))))
                    .WithExplicitInterfaceSpecifier(F.ExplicitInterfaceSpecifier(typesForSymbols.ParseName(_mock.InterfaceSymbol)));

                if (_mock.Symbol.IsReadOnly)
                {
                    ExpressionSyntax invocation = F.InvocationExpression(F.IdentifierName(_mock.MemberMockName),
                        F.ArgumentList(F.SeparatedList(_mock.Symbol.Parameters.Select(a => F.Argument(F.IdentifierName(a.Name))))));
                    if (_mock.Symbol.ReturnsByRef || _mock.Symbol.ReturnsByRefReadonly)
                    {
                        invocation = F.RefExpression(invocation);
                    }

                    mockedIndexer = mockedIndexer
                        .WithExpressionBody(F.ArrowExpressionClause(invocation))
                        .WithSemicolonToken(F.Token(SyntaxKind.SemicolonToken));
                }
                else
                {
                    if (!_mock.Symbol.IsWriteOnly)
                    {
                        var argumentList = F.SeparatedList(_mock.Symbol.Parameters.Select(a => F.Argument(F.IdentifierName(a.Name))));

                        mockedIndexer = mockedIndexer.AddAccessorListAccessors(F.AccessorDeclaration(SyntaxKind.GetAccessorDeclaration)
                            .WithExpressionBody(F.ArrowExpressionClause(F.InvocationExpression(F.IdentifierName(_mock.MemberMockName))
                                .WithArgumentList(F.ArgumentList(argumentList))))
                            .WithSemicolonToken(F.Token(SyntaxKind.SemicolonToken))
                        );
                    }

                    if (!_mock.Symbol.IsReadOnly)
                    {
                        var argumentList = F.SeparatedList(_mock.Symbol.Parameters.Select(a => F.Argument(F.IdentifierName(a.Name))))
                            .Add(F.Argument(F.IdentifierName("value")));

                        mockedIndexer = mockedIndexer.AddAccessorListAccessors(F.AccessorDeclaration(SyntaxKind.SetAccessorDeclaration)
                            .WithExpressionBody(F.ArrowExpressionClause(F.InvocationExpression(F.IdentifierName(_mock.MemberMockName),
                                F.ArgumentList(argumentList))))
                            .WithSemicolonToken(F.Token(SyntaxKind.SemicolonToken))
                        );
                    }
                }

                return mockedIndexer;
            }
        }
    }
}
