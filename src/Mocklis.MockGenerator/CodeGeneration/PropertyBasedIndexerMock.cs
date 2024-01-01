// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PropertyBasedIndexerMock.cs">
//   SPDX-License-Identifier: MIT
//   Copyright © 2019-2023 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.MockGenerator.CodeGeneration;

#region Using Directives

using System;
using System.Collections.Generic;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using F = Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

#endregion

public sealed class PropertyBasedIndexerMock : IMemberMock
{
    public IPropertySymbol Symbol { get; }
    public string MemberMockName { get; }

    public PropertyBasedIndexerMock(IPropertySymbol symbol,
        string mockMemberName)
    {
        Symbol = symbol;
        MemberMockName = mockMemberName;
    }

    public void AddSyntax(MocklisTypesForSymbols typesForSymbols, IList<MemberDeclarationSyntax> declarationList,
        List<StatementSyntax> constructorStatements,
        NameSyntax interfaceNameSyntax, string className, string interfaceName)
    {
        var builder = new SingleTypeOrValueTupleBuilder();
        foreach (var p in Symbol.Parameters)
        {
            builder.AddParameter(p);
        }

        var keyType = builder.Build(MemberMockName);

        var keyTypeSyntax = keyType.BuildTypeSyntax(typesForSymbols, null) ??
                            throw new ArgumentException("Property symbol must have at least one parameter", nameof(Symbol));

        var valueTypeSyntax = typesForSymbols.ParseTypeName(Symbol.Type, Symbol.NullableOrOblivious());

        var mockPropertyType = typesForSymbols.IndexerMock(keyTypeSyntax, valueTypeSyntax);

        declarationList.Add(mockPropertyType.MockProperty(MemberMockName));
        declarationList.Add(ExplicitInterfaceMember(typesForSymbols, interfaceNameSyntax, valueTypeSyntax, keyType));

        constructorStatements.Add(
            typesForSymbols.InitialisationStatement(mockPropertyType, MemberMockName, className, interfaceName, Symbol.Name));
    }

    private MemberDeclarationSyntax ExplicitInterfaceMember(MocklisTypesForSymbols typesForSymbols, NameSyntax interfaceNameSyntax,
        TypeSyntax valueTypeSyntax, SingleTypeOrValueTuple keyType)
    {
        var decoratedValueTypeSyntax = valueTypeSyntax;

        if (Symbol.ReturnsByRef)
        {
            decoratedValueTypeSyntax = F.RefType(decoratedValueTypeSyntax);
        }
        else if (Symbol.ReturnsByRefReadonly)
        {
            decoratedValueTypeSyntax = F.RefType(decoratedValueTypeSyntax).WithReadOnlyKeyword(F.Token(SyntaxKind.ReadOnlyKeyword));
        }

        var mockedIndexer = F.IndexerDeclaration(decoratedValueTypeSyntax)
            .WithParameterList(keyType.BuildParameterList(typesForSymbols, null))
            .WithExplicitInterfaceSpecifier(F.ExplicitInterfaceSpecifier(interfaceNameSyntax));

        var arguments = keyType.BuildArgumentList();

        if (Symbol.IsReadOnly)
        {
            ExpressionSyntax elementAccess = F.ElementAccessExpression(F.IdentifierName(MemberMockName))
                .WithExpressionsAsArgumentList(arguments);

            if (Symbol.ReturnsByRef || Symbol.ReturnsByRefReadonly)
            {
                elementAccess = typesForSymbols.WrapByRef(elementAccess, valueTypeSyntax);
            }

            mockedIndexer = mockedIndexer.WithExpressionBody(F.ArrowExpressionClause(elementAccess))
                .WithSemicolonToken(F.Token(SyntaxKind.SemicolonToken));
        }
        else
        {
            if (!Symbol.IsWriteOnly)
            {
                mockedIndexer = mockedIndexer.AddAccessorListAccessors(F.AccessorDeclaration(SyntaxKind.GetAccessorDeclaration)
                    .WithExpressionBody(F.ArrowExpressionClause(F.ElementAccessExpression(F.IdentifierName(MemberMockName))
                        .WithExpressionsAsArgumentList(arguments)))
                    .WithSemicolonToken(F.Token(SyntaxKind.SemicolonToken))
                );
            }

            if (!Symbol.IsReadOnly)
            {
                mockedIndexer = mockedIndexer.AddAccessorListAccessors(F.AccessorDeclaration(SyntaxKind.SetAccessorDeclaration)
                    .WithExpressionBody(F.ArrowExpressionClause(
                        F.AssignmentExpression(SyntaxKind.SimpleAssignmentExpression,
                            F.ElementAccessExpression(F.IdentifierName(MemberMockName)).WithExpressionsAsArgumentList(arguments),
                            F.IdentifierName("value"))))
                    .WithSemicolonToken(F.Token(SyntaxKind.SemicolonToken)));
            }
        }

        return mockedIndexer;
    }
}
