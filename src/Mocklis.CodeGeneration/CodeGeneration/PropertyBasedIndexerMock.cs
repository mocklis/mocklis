// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PropertyBasedIndexerMock.cs">
//   SPDX-License-Identifier: MIT
//   Copyright © 2019-2021 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.CodeGeneration
{
    #region Using Directives

    using System;
    using System.Collections.Generic;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp;
    using Microsoft.CodeAnalysis.CSharp.Syntax;
    using Mocklis.CodeGeneration.Compatibility;
    using F = Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

    #endregion

    public class PropertyBasedIndexerMock : IMemberMock
    {
        public string ClassName { get; }
        public INamedTypeSymbol InterfaceSymbol { get; }
        public string InterfaceName { get; }
        public IPropertySymbol Symbol { get; }
        public string MemberMockName { get; }

        public PropertyBasedIndexerMock(INamedTypeSymbol classSymbol, INamedTypeSymbol interfaceSymbol,
            IPropertySymbol symbol,
            string mockMemberName)
        {
            ClassName = classSymbol.Name;
            InterfaceSymbol = interfaceSymbol;
            InterfaceName = interfaceSymbol.Name;
            Symbol = symbol;
            MemberMockName = mockMemberName;
        }

        public ISyntaxAdder GetSyntaxAdder(MocklisTypesForSymbols typesForSymbols, bool strict, bool veryStrict)
        {
            return new SyntaxAdder(this, typesForSymbols, strict, veryStrict);
        }

        private class SyntaxAdder : ISyntaxAdder
        {
            private readonly PropertyBasedIndexerMock _mock;
            private readonly MocklisTypesForSymbols _typesForSymbols;
            private readonly bool _strict;
            private readonly bool _veryStrict;

            private SingleTypeOrValueTuple KeyType { get; }
            private TypeSyntax KeyTypeSyntax { get; }
            private TypeSyntax ValueTypeSyntax { get; }
            private TypeSyntax MockPropertyType { get; }

            public SyntaxAdder(PropertyBasedIndexerMock mock, MocklisTypesForSymbols typesForSymbols, bool strict, bool veryStrict)
            {
                _mock = mock;
                _typesForSymbols = typesForSymbols;
                _strict = strict;
                _veryStrict = veryStrict;

                var builder = new SingleTypeOrValueTupleBuilder(typesForSymbols);
                foreach (var p in _mock.Symbol.Parameters)
                {
                    builder.AddParameter(p);
                }

                KeyType = builder.Build(_mock.MemberMockName);

                var keyTypeSyntax = KeyType.BuildTypeSyntax();

                KeyTypeSyntax = keyTypeSyntax ?? throw new ArgumentException("Property symbol must have at least one parameter", nameof(mock));

                ValueTypeSyntax = typesForSymbols.ParseTypeName(_mock.Symbol.Type, _mock.Symbol.NullableOrOblivious());

                MockPropertyType = typesForSymbols.IndexerMock(KeyTypeSyntax, ValueTypeSyntax);

            }

            public ITypeSymbol InterfaceSymbol => _mock.InterfaceSymbol;

            public void AddMembersToClass(IList<MemberDeclarationSyntax> declarationList, NameSyntax interfaceNameSyntax)
            {
                declarationList.Add(MockPropertyType.MockProperty(_mock.MemberMockName));
                declarationList.Add(ExplicitInterfaceMember(_typesForSymbols, interfaceNameSyntax));
            }

            public void AddInitialisersToConstructor(List<StatementSyntax> constructorStatements)
            {
                constructorStatements.Add(_typesForSymbols.InitialisationStatement(MockPropertyType, _mock.MemberMockName, _mock.ClassName, _mock.InterfaceName, _mock.Symbol.Name, _strict, _veryStrict));
            }

            private MemberDeclarationSyntax ExplicitInterfaceMember(MocklisTypesForSymbols typesForSymbols, NameSyntax interfaceNameSyntax)
            {
                var decoratedValueTypeSyntax = ValueTypeSyntax;

                if (_mock.Symbol.ReturnsByRef)
                {
                    decoratedValueTypeSyntax = F.RefType(decoratedValueTypeSyntax);
                }
                else if (_mock.Symbol.ReturnsByRefReadonly)
                {
                    decoratedValueTypeSyntax = F.RefType(decoratedValueTypeSyntax).WithReadOnlyKeyword(F.Token(SyntaxKind.ReadOnlyKeyword));
                }

                var mockedIndexer = F.IndexerDeclaration(decoratedValueTypeSyntax)
                    .WithParameterList(KeyType.BuildParameterList())
                    .WithExplicitInterfaceSpecifier(F.ExplicitInterfaceSpecifier(interfaceNameSyntax));

                var arguments = KeyType.BuildArgumentList();

                if (_mock.Symbol.IsReadOnly)
                {
                    ExpressionSyntax elementAccess = F.ElementAccessExpression(F.IdentifierName(_mock.MemberMockName))
                        .WithExpressionsAsArgumentList(arguments);

                    if (_mock.Symbol.ReturnsByRef || _mock.Symbol.ReturnsByRefReadonly)
                    {
                        elementAccess = typesForSymbols.WrapByRef(elementAccess, ValueTypeSyntax);
                    }

                    mockedIndexer = mockedIndexer.WithExpressionBody(F.ArrowExpressionClause(elementAccess))
                        .WithSemicolonToken(F.Token(SyntaxKind.SemicolonToken));
                }
                else
                {
                    if (!_mock.Symbol.IsWriteOnly)
                    {
                        mockedIndexer = mockedIndexer.AddAccessorListAccessors(F.AccessorDeclaration(SyntaxKind.GetAccessorDeclaration)
                            .WithExpressionBody(F.ArrowExpressionClause(F.ElementAccessExpression(F.IdentifierName(_mock.MemberMockName))
                                .WithExpressionsAsArgumentList(arguments)))
                            .WithSemicolonToken(F.Token(SyntaxKind.SemicolonToken))
                        );
                    }

                    if (!_mock.Symbol.IsReadOnly)
                    {
                        mockedIndexer = mockedIndexer.AddAccessorListAccessors(F.AccessorDeclaration(SyntaxKind.SetAccessorDeclaration)
                            .WithExpressionBody(F.ArrowExpressionClause(
                                F.AssignmentExpression(SyntaxKind.SimpleAssignmentExpression,
                                    F.ElementAccessExpression(F.IdentifierName(_mock.MemberMockName)).WithExpressionsAsArgumentList(arguments),
                                    F.IdentifierName("value"))))
                            .WithSemicolonToken(F.Token(SyntaxKind.SemicolonToken)));
                    }
                }

                return mockedIndexer;
            }
        }
    }
}
