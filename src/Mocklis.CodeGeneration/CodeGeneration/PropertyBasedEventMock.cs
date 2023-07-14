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

    public class PropertyBasedEventMock : IMemberMock
    {
        public string ClassName { get; }
        public INamedTypeSymbol InterfaceSymbol { get; }
        public string InterfaceName { get; }
        public IEventSymbol Symbol { get; }
        public string MemberMockName { get; }

        public PropertyBasedEventMock(INamedTypeSymbol classSymbol, INamedTypeSymbol interfaceSymbol, IEventSymbol symbol, string memberMockName)
        {
            ClassName = classSymbol.Name;
            InterfaceSymbol = interfaceSymbol;
            InterfaceName = interfaceSymbol.Name;
            Symbol = symbol;
            MemberMockName = memberMockName;
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


            public ITypeSymbol InterfaceSymbol => _mock.InterfaceSymbol;

            public void AddMembersToClass(IList<MemberDeclarationSyntax> declarationList, NameSyntax interfaceNameSyntax)
            {
                declarationList.Add(MockPropertyType.MockProperty(_mock.MemberMockName));
                declarationList.Add(ExplicitInterfaceMember(interfaceNameSyntax));
            }

            public void AddInitialisersToConstructor(List<StatementSyntax> constructorStatements)
            {
                constructorStatements.Add(_typesForSymbols.InitialisationStatement(MockPropertyType, _mock.MemberMockName, _mock.ClassName, _mock.InterfaceName, _mock.Symbol.Name, _strict, _veryStrict));
            }

            private MemberDeclarationSyntax ExplicitInterfaceMember(NameSyntax interfaceNameSyntax)
            {
                var mockedProperty = F.EventDeclaration(EventHandlerTypeSyntax, _mock.Symbol.Name)
                    .WithExplicitInterfaceSpecifier(F.ExplicitInterfaceSpecifier(interfaceNameSyntax));

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
