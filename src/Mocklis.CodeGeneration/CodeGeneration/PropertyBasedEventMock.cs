// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PropertyBasedEventMock.cs">
//   SPDX-License-Identifier: MIT
//   Copyright © 2019-2021 Esbjörn Redmo and contributors. All rights reserved.
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

    public class PropertyBasedEventMock : PropertyBasedMock<IEventSymbol>, IMemberMock
    {
        public PropertyBasedEventMock(INamedTypeSymbol classSymbol, INamedTypeSymbol interfaceSymbol, IEventSymbol symbol, string memberMockName) : base(classSymbol, interfaceSymbol, symbol, memberMockName)
        {
        }

        public ISyntaxAdder GetSyntaxAdder(MocklisTypesForSymbols typesForSymbols, bool strict, bool veryStrict)
        {
            return new SyntaxAdder(this, typesForSymbols, strict, veryStrict);
        }

        private class SyntaxAdder : ISyntaxAdder
        {
            private readonly PropertyBasedEventMock _mock;
            private readonly MocklisTypesForSymbols _typesForSymbols;
            private readonly bool _strict;
            private readonly bool _veryStrict;
            private TypeSyntax EventHandlerTypeSyntax { get; }
            private TypeSyntax MockPropertyType { get; }

            public SyntaxAdder(PropertyBasedEventMock mock, MocklisTypesForSymbols typesForSymbols, bool strict, bool veryStrict)
            {
                _mock = mock;
                _typesForSymbols = typesForSymbols;
                _strict = strict;
                _veryStrict = veryStrict;
                EventHandlerTypeSyntax = typesForSymbols.ParseTypeName(_mock.Symbol.Type, _mock.Symbol.NullableOrOblivious());
                MockPropertyType = typesForSymbols.EventMock(typesForSymbols.ParseTypeName(_mock.Symbol.Type, false));
            }

            public void AddMembersToClass(IList<MemberDeclarationSyntax> declarationList)
            {
                declarationList.Add(_mock.MockProperty(MockPropertyType));
                declarationList.Add(ExplicitInterfaceMember());
            }

            public void AddInitialisersToConstructor(List<StatementSyntax> constructorStatements)
            {
                constructorStatements.Add(_mock.InitialisationStatement(MockPropertyType, _typesForSymbols, _strict, _veryStrict));
            }

            private MemberDeclarationSyntax ExplicitInterfaceMember()
            {
                var mockedProperty = F.EventDeclaration(EventHandlerTypeSyntax, _mock.Symbol.Name)
                    .WithExplicitInterfaceSpecifier(F.ExplicitInterfaceSpecifier(_typesForSymbols.ParseName(_mock.InterfaceSymbol)));

                mockedProperty = mockedProperty.AddAccessorListAccessors(F.AccessorDeclaration(SyntaxKind.AddAccessorDeclaration)
                    .WithExpressionBody(F.ArrowExpressionClause(F.InvocationExpression(
                            F.MemberAccessExpression(SyntaxKind.SimpleMemberAccessExpression, F.IdentifierName(_mock.MemberMockName),
                                F.IdentifierName("Add")))
                        .WithExpressionsAsArgumentList(F.IdentifierName("value"))))
                    .WithSemicolonToken(F.Token(SyntaxKind.SemicolonToken))
                );

                mockedProperty = mockedProperty.AddAccessorListAccessors(F.AccessorDeclaration(SyntaxKind.RemoveAccessorDeclaration)
                    .WithExpressionBody(F.ArrowExpressionClause(F.InvocationExpression(
                            F.MemberAccessExpression(SyntaxKind.SimpleMemberAccessExpression,
                                F.IdentifierName(_mock.MemberMockName), F.IdentifierName("Remove")))
                        .WithExpressionsAsArgumentList(F.IdentifierName("value"))))
                    .WithSemicolonToken(F.Token(SyntaxKind.SemicolonToken)));

                return mockedProperty;
            }
        }
    }
}
