// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PropertyBasedPropertyMock.cs">
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

public sealed class PropertyBasedPropertyMock : IMemberMock
{
    public IPropertySymbol Symbol { get; }
    public string MemberMockName { get; }

    public PropertyBasedPropertyMock(IPropertySymbol symbol, string mockMemberName)
    {
        Symbol = symbol;
        MemberMockName = mockMemberName;
    }

    public void AddSyntax(MocklisTypesForSymbols typesForSymbols, IList<MemberDeclarationSyntax> declarationList,
        List<StatementSyntax> constructorStatements,
        NameSyntax interfaceNameSyntax, string className, string interfaceName)
    {
        var syntaxAdder = new SyntaxAdder(this, typesForSymbols);
        syntaxAdder.AddMembersToClass(declarationList, interfaceNameSyntax);
        syntaxAdder.AddInitialisersToConstructor(constructorStatements, className, interfaceName);
    }

    private class SyntaxAdder
    {
        private TypeSyntax ValueTypeSyntax { get; }
        private TypeSyntax MockPropertyType { get; }
        private MocklisTypesForSymbols TypesForSymbols { get; }
        private PropertyBasedPropertyMock Mock { get; }

        public SyntaxAdder(PropertyBasedPropertyMock mock, MocklisTypesForSymbols typesForSymbols)
        {
            TypesForSymbols = typesForSymbols;
            Mock = mock;
            ValueTypeSyntax = typesForSymbols.ParseTypeName(mock.Symbol.Type, mock.Symbol.NullableOrOblivious());
            MockPropertyType = typesForSymbols.PropertyMock(ValueTypeSyntax);
        }

        public void AddMembersToClass(IList<MemberDeclarationSyntax> declarationList, NameSyntax interfaceNameSyntax)
        {
            declarationList.Add(MockPropertyType.MockProperty(Mock.MemberMockName));
            declarationList.Add(ExplicitInterfaceMember(interfaceNameSyntax));
        }

        public void AddInitialisersToConstructor(List<StatementSyntax> constructorStatements, string className, string interfaceName)
        {
            constructorStatements.Add(TypesForSymbols.InitialisationStatement(MockPropertyType, Mock.MemberMockName, className, interfaceName,
                Mock.Symbol.Name));
        }

        private MemberDeclarationSyntax ExplicitInterfaceMember(NameSyntax interfaceNameSyntax)
        {
            var decoratedValueTypeSyntax = ValueTypeSyntax;

            if (Mock.Symbol.ReturnsByRef)
            {
                decoratedValueTypeSyntax = F.RefType(decoratedValueTypeSyntax);
            }
            else if (Mock.Symbol.ReturnsByRefReadonly)
            {
                decoratedValueTypeSyntax = F.RefType(decoratedValueTypeSyntax).WithReadOnlyKeyword(F.Token(SyntaxKind.ReadOnlyKeyword));
            }

            var mockedProperty = F.PropertyDeclaration(decoratedValueTypeSyntax, Mock.Symbol.Name)
                .WithExplicitInterfaceSpecifier(F.ExplicitInterfaceSpecifier(interfaceNameSyntax));

            if (Mock.Symbol.IsReadOnly)
            {
                ExpressionSyntax elementAccess = F.MemberAccessExpression(SyntaxKind.SimpleMemberAccessExpression,
                    F.IdentifierName(Mock.MemberMockName),
                    F.IdentifierName("Value"));

                if (Mock.Symbol.ReturnsByRef || Mock.Symbol.ReturnsByRefReadonly)
                {
                    elementAccess = TypesForSymbols.WrapByRef(elementAccess, ValueTypeSyntax);
                }

                mockedProperty = mockedProperty.WithExpressionBody(F.ArrowExpressionClause(elementAccess))
                    .WithSemicolonToken(F.Token(SyntaxKind.SemicolonToken));
            }
            else
            {
                if (!Mock.Symbol.IsWriteOnly)
                {
                    mockedProperty = mockedProperty.AddAccessorListAccessors(F.AccessorDeclaration(SyntaxKind.GetAccessorDeclaration)
                        .WithExpressionBody(F.ArrowExpressionClause(F.MemberAccessExpression(SyntaxKind.SimpleMemberAccessExpression,
                            F.IdentifierName(Mock.MemberMockName),
                            F.IdentifierName("Value"))))
                        .WithSemicolonToken(F.Token(SyntaxKind.SemicolonToken))
                    );
                }

                if (!Mock.Symbol.IsReadOnly)
                {
                    mockedProperty = mockedProperty.AddAccessorListAccessors(F.AccessorDeclaration(SyntaxKind.SetAccessorDeclaration)
                        .WithExpressionBody(
                            F.ArrowExpressionClause(
                                F.AssignmentExpression(SyntaxKind.SimpleAssignmentExpression,
                                    F.MemberAccessExpression(SyntaxKind.SimpleMemberAccessExpression, F.IdentifierName(Mock.MemberMockName),
                                        F.IdentifierName("Value")),
                                    F.IdentifierName("value"))))
                        .WithSemicolonToken(F.Token(SyntaxKind.SemicolonToken))
                    );
                }
            }

            return mockedProperty;
        }
    }
}
