// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PropertyBasedEventMock.cs">
//   SPDX-License-Identifier: MIT
//   Copyright © 2019-2023 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.MockGenerator.CodeGeneration;

#region Using Directives

using System.Collections.Generic;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using F = Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

#endregion

public sealed class PropertyBasedEventMock : IMemberMock
{
    public IEventSymbol Symbol { get; }
    public string MemberMockName { get; }

    public PropertyBasedEventMock(IEventSymbol symbol, string memberMockName)
    {
        Symbol = symbol;
        MemberMockName = memberMockName;
    }

    public void AddSyntax(MocklisTypesForSymbols typesForSymbols, IList<MemberDeclarationSyntax> declarationList,
        List<StatementSyntax> constructorStatements,
        NameSyntax interfaceNameSyntax, string className, string interfaceName)
    {
        var mockPropertyType = typesForSymbols.EventMock(typesForSymbols.ParseTypeName(Symbol.Type, false));
        declarationList.Add(mockPropertyType.MockProperty(MemberMockName));
        declarationList.Add(ExplicitInterfaceMember(typesForSymbols, interfaceNameSyntax));
        constructorStatements.Add(typesForSymbols.InitialisationStatement(mockPropertyType, MemberMockName, className, interfaceName, Symbol.Name));
    }

    private MemberDeclarationSyntax ExplicitInterfaceMember(MocklisTypesForSymbols typesForSymbols, NameSyntax interfaceNameSyntax)
    {
        var eventHandlerTypeSyntax = typesForSymbols.ParseTypeName(Symbol.Type, Symbol.NullableOrOblivious());
        var mockedProperty = F.EventDeclaration(eventHandlerTypeSyntax, Symbol.Name)
            .WithExplicitInterfaceSpecifier(F.ExplicitInterfaceSpecifier(interfaceNameSyntax));

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
}
