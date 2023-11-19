// --------------------------------------------------------------------------------------------------------------------
// <copyright file="VirtualMethodBasedPropertyMock.cs">
//   SPDX-License-Identifier: MIT
//   Copyright © 2019-2023 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.CodeGeneration
{
    #region Using Directives

    using System.Collections.Generic;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp;
    using Microsoft.CodeAnalysis.CSharp.Syntax;
    using Mocklis.CodeGeneration.Compatibility;
    using F = Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

    #endregion

    public sealed class VirtualMethodBasedPropertyMock : VirtualMethodBasedMock<IPropertySymbol>, IMemberMock
    {
        public VirtualMethodBasedPropertyMock(INamedTypeSymbol classSymbol, INamedTypeSymbol interfaceSymbol, IPropertySymbol symbol, string mockMemberName) : base(classSymbol, interfaceSymbol, symbol, mockMemberName)
        {
        }

        public ISyntaxAdder GetSyntaxAdder(MocklisTypesForSymbols typesForSymbols, bool strict, bool veryStrict)
        {
            return new SyntaxAdder(this, typesForSymbols);
        }

        public class SyntaxAdder : ISyntaxAdder
        {
            private readonly VirtualMethodBasedPropertyMock _mock;
            private readonly MocklisTypesForSymbols _typesForSymbols;

            public SyntaxAdder(VirtualMethodBasedPropertyMock mock, MocklisTypesForSymbols typesForSymbols)
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
                    declarationList.Add(MockGetVirtualMethod(valueTypeSyntax));
                }

                if (!_mock.Symbol.IsReadOnly)
                {
                    declarationList.Add(MockSetVirtualMethod(valueTypeSyntax));
                }

                declarationList.Add(ExplicitInterfaceMember(valueWithReadonlyTypeSyntax));
            }

            public void AddInitialisersToConstructor(List<StatementSyntax> constructorStatements)
            {
            }

            private MemberDeclarationSyntax MockGetVirtualMethod(TypeSyntax valueTypeSyntax)
            {
                return F.MethodDeclaration(valueTypeSyntax, F.Identifier(_mock.MemberMockName))
                    .WithModifiers(F.TokenList(F.Token(SyntaxKind.ProtectedKeyword), F.Token(SyntaxKind.VirtualKeyword)))
                    .WithBody(F.Block(_mock.ThrowMockMissingStatement(_typesForSymbols, "VirtualPropertyGet")));
            }

            private MemberDeclarationSyntax MockSetVirtualMethod(TypeSyntax valueTypeSyntax)
            {
                return F.MethodDeclaration(F.PredefinedType(F.Token(SyntaxKind.VoidKeyword)), F.Identifier(_mock.MemberMockName))
                    .WithParameterList(F.ParameterList(F.SeparatedList(new[]
                    {
                        F.Parameter(F.Identifier("value")).WithType(valueTypeSyntax)
                    })))
                    .WithModifiers(F.TokenList(F.Token(SyntaxKind.ProtectedKeyword), F.Token(SyntaxKind.VirtualKeyword)))
                    .WithBody(F.Block(_mock.ThrowMockMissingStatement(_typesForSymbols, "VirtualPropertySet")));
            }

            private MemberDeclarationSyntax ExplicitInterfaceMember(TypeSyntax valueWithReadonlyTypeSyntax)
            {
                var mockedProperty = F.PropertyDeclaration(valueWithReadonlyTypeSyntax, _mock.Symbol.Name)
                    .WithExplicitInterfaceSpecifier(F.ExplicitInterfaceSpecifier(_typesForSymbols.ParseName(_mock.InterfaceSymbol)));

                if (_mock.Symbol.IsReadOnly)
                {
                    ExpressionSyntax invocation = F.InvocationExpression(F.IdentifierName(_mock.MemberMockName));
                    if (_mock.Symbol.ReturnsByRef || _mock.Symbol.ReturnsByRefReadonly)
                    {
                        invocation = F.RefExpression(invocation);
                    }

                    mockedProperty = mockedProperty
                        .WithExpressionBody(F.ArrowExpressionClause(invocation))
                        .WithSemicolonToken(F.Token(SyntaxKind.SemicolonToken));
                }
                else
                {
                    if (!_mock.Symbol.IsWriteOnly)
                    {
                        mockedProperty = mockedProperty.AddAccessorListAccessors(F.AccessorDeclaration(SyntaxKind.GetAccessorDeclaration)
                            .WithExpressionBody(F.ArrowExpressionClause(F.InvocationExpression(F.IdentifierName(_mock.MemberMockName))))
                            .WithSemicolonToken(F.Token(SyntaxKind.SemicolonToken))
                        );
                    }

                    if (!_mock.Symbol.IsReadOnly)
                    {
                        mockedProperty = mockedProperty.AddAccessorListAccessors(F.AccessorDeclaration(SyntaxKind.SetAccessorDeclaration)
                            .WithExpressionBody(F.ArrowExpressionClause(F.InvocationExpression(F.IdentifierName(_mock.MemberMockName),
                                F.ArgumentList(F.SeparatedList(new[] { F.Argument(F.IdentifierName("value")) })))))
                            .WithSemicolonToken(F.Token(SyntaxKind.SemicolonToken))
                        );
                    }
                }

                return mockedProperty;
            }
        }
    }
}
