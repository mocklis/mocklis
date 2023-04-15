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

    public class PropertyBasedEventMock : PropertyBasedMock<IEventSymbol>, IMemberMock, ISyntaxAdder
    {
        private TypeSyntax EventHandlerTypeSyntax { get; }
        private TypeSyntax MockPropertyType { get; }

        public PropertyBasedEventMock(MocklisTypesForSymbols typesForSymbols, INamedTypeSymbol classSymbol, INamedTypeSymbol interfaceSymbol,
            IEventSymbol symbol, string memberMockName) : base(classSymbol, interfaceSymbol, symbol, memberMockName)
        {
            EventHandlerTypeSyntax = typesForSymbols.ParseTypeName(symbol.Type, symbol.NullableOrOblivious());
            MockPropertyType = typesForSymbols.EventMock(typesForSymbols.ParseTypeName(symbol.Type, false));
        }

        public void AddMembersToClass(IList<MemberDeclarationSyntax> declarationList, MocklisTypesForSymbols typesForSymbols, bool strict, bool veryStrict)
        {
            declarationList.Add(MockProperty(MockPropertyType));
            declarationList.Add(ExplicitInterfaceMember(typesForSymbols));
        }

        public void AddInitialisersToConstructor(List<StatementSyntax> constructorStatements, MocklisTypesForSymbols typesForSymbols, bool strict, bool veryStrict)
        {
            constructorStatements.Add(InitialisationStatement(MockPropertyType, typesForSymbols, strict, veryStrict));
        }

        private MemberDeclarationSyntax ExplicitInterfaceMember(MocklisTypesForSymbols typesForSymbols)
        {
            var mockedProperty = F.EventDeclaration(EventHandlerTypeSyntax, Symbol.Name)
                .WithExplicitInterfaceSpecifier(F.ExplicitInterfaceSpecifier(typesForSymbols.ParseName(InterfaceSymbol)));

            mockedProperty = mockedProperty.AddAccessorListAccessors(F.AccessorDeclaration(SyntaxKind.AddAccessorDeclaration)
                .WithExpressionBody(F.ArrowExpressionClause(F.InvocationExpression(
                        F.MemberAccessExpression(SyntaxKind.SimpleMemberAccessExpression, F.IdentifierName(MemberMockName),
                            F.IdentifierName("Add")))
                    .WithExpressionsAsArgumentList(F.IdentifierName("value"))))
                .WithSemicolonToken(F.Token(SyntaxKind.SemicolonToken))
            );

            mockedProperty = mockedProperty.AddAccessorListAccessors(F.AccessorDeclaration(SyntaxKind.RemoveAccessorDeclaration)
                .WithExpressionBody(F.ArrowExpressionClause(F.InvocationExpression(
                        F.MemberAccessExpression(SyntaxKind.SimpleMemberAccessExpression,
                            F.IdentifierName(MemberMockName), F.IdentifierName("Remove")))
                    .WithExpressionsAsArgumentList(F.IdentifierName("value"))))
                .WithSemicolonToken(F.Token(SyntaxKind.SemicolonToken)));

            return mockedProperty;
        }

        public ISyntaxAdder GetSyntaxAdder() => this;
    }
}
