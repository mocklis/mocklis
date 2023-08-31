// --------------------------------------------------------------------------------------------------------------------
// <copyright file="VirtualMethodBasedIndexerMock.cs">
//   SPDX-License-Identifier: MIT
//   Copyright © 2019-2021 Esbjörn Redmo and contributors. All rights reserved.
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
    using Mocklis.CodeGeneration.Compatibility;
    using Mocklis.CodeGeneration.UniqueNames;
    using F = Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

    #endregion

    public sealed class VirtualMethodBasedIndexerMock : IMemberMock
    {
        public IPropertySymbol Symbol { get; }
        public string MemberMockName { get; }

        public VirtualMethodBasedIndexerMock(IPropertySymbol symbol, string mockMemberName)
        {
            Symbol = symbol;
            MemberMockName = mockMemberName;
        }
        
        public ISyntaxAdder GetSyntaxAdder(MocklisTypesForSymbols typesForSymbols)
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

            public void AddMembersToClass(MocklisTypesForSymbols typesForSymbols, MockSettings mockSettingns,
                IList<MemberDeclarationSyntax> declarationList, NameSyntax interfaceNameSyntax, string className,
                string interfaceName)
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
                    declarationList.Add(MockGetVirtualMethod(_typesForSymbols, valueTypeSyntax, className, interfaceName));
                }

                if (!_mock.Symbol.IsReadOnly)
                {
                    declarationList.Add(MockSetVirtualMethod(_typesForSymbols, valueTypeSyntax, className, interfaceName));
                }

                declarationList.Add(ExplicitInterfaceMember(_typesForSymbols, valueWithReadonlyTypeSyntax, interfaceNameSyntax));
            }

            public void AddInitialisersToConstructor(MocklisTypesForSymbols typesForSymbols, MockSettings mockSettings,
                List<StatementSyntax> constructorStatements, string className, string interfaceName)
            {
            }

            private MemberDeclarationSyntax MockGetVirtualMethod(MocklisTypesForSymbols typesForSymbols, TypeSyntax valueTypeSyntax, string className, string interfaceName)
            {
                return F.MethodDeclaration(valueTypeSyntax, F.Identifier(_mock.MemberMockName))
                    .WithModifiers(F.TokenList(F.Token(SyntaxKind.ProtectedKeyword), F.Token(SyntaxKind.VirtualKeyword)))
                    .WithParameterList(F.ParameterList(F.SeparatedList(_mock.Symbol.Parameters.Select(a =>
                        F.Parameter(F.Identifier(a.Name)).WithType(typesForSymbols.ParseTypeName(a.Type, a.NullableOrOblivious()))))))
                    .WithBody(F.Block(typesForSymbols.ThrowMockMissingStatement("VirtualIndexerGet", _mock.MemberMockName, className, interfaceName, _mock.Symbol.Name)));
            }

            private MemberDeclarationSyntax MockSetVirtualMethod(MocklisTypesForSymbols typesForSymbols, TypeSyntax valueTypeSyntax, string className, string interfaceName)
            {
                var uniquifier = new Uniquifier(_mock.Symbol.Parameters.Select(p => p.Name));

                var parameterList = F.SeparatedList(_mock.Symbol.Parameters.Select(a =>
                        F.Parameter(F.Identifier(a.Name)).WithType(typesForSymbols.ParseTypeName(a.Type, a.NullableOrOblivious()))))
                    .Add(F.Parameter(F.Identifier(uniquifier.GetUniqueName("value"))).WithType(valueTypeSyntax));

                return F.MethodDeclaration(F.PredefinedType(F.Token(SyntaxKind.VoidKeyword)), F.Identifier(_mock.MemberMockName))
                    .WithModifiers(F.TokenList(F.Token(SyntaxKind.ProtectedKeyword), F.Token(SyntaxKind.VirtualKeyword)))
                    .WithParameterList(F.ParameterList(parameterList))
                    .WithBody(F.Block(typesForSymbols.ThrowMockMissingStatement("VirtualIndexerSet", _mock.MemberMockName, className, interfaceName, _mock.Symbol.Name)));
            }

            private MemberDeclarationSyntax ExplicitInterfaceMember(MocklisTypesForSymbols typesForSymbols, TypeSyntax valueWithReadonlyTypeSyntax, NameSyntax interfaceNameSyntax)
            {
                var mockedIndexer = F.IndexerDeclaration(valueWithReadonlyTypeSyntax)
                    .WithParameterList(F.BracketedParameterList(F.SeparatedList(_mock.Symbol.Parameters.Select(a =>
                        F.Parameter(F.Identifier(a.Name)).WithType(typesForSymbols.ParseTypeName(a.Type, a.NullableOrOblivious()))))))
                    .WithExplicitInterfaceSpecifier(F.ExplicitInterfaceSpecifier(interfaceNameSyntax));

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
