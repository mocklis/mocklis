// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PropertyBasedPropertyMock.cs">
//   SPDX-License-Identifier: MIT
//   Copyright © 2019-2020 Esbjörn Redmo and contributors. All rights reserved.
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

    public class PropertyBasedPropertyMock : PropertyBasedMock<IPropertySymbol>, IMemberMock
    {
        private TypeSyntax ValueTypeSyntax { get; }
        private TypeSyntax MockPropertyType { get; }

        public PropertyBasedPropertyMock(MocklisTypesForSymbols typesForSymbols, INamedTypeSymbol classSymbol, INamedTypeSymbol interfaceSymbol,
            IPropertySymbol symbol,
            string mockMemberName, bool strict, bool veryStrict) : base(typesForSymbols,
            classSymbol, interfaceSymbol, symbol, mockMemberName, strict, veryStrict)
        {
            ValueTypeSyntax = typesForSymbols.ParseTypeName(symbol.Type, symbol.NullableOrOblivious());
            MockPropertyType = typesForSymbols.PropertyMock(ValueTypeSyntax);
        }

        public void AddMembersToClass(IList<MemberDeclarationSyntax> declarationList)
        {
            declarationList.Add(MockProperty(MockPropertyType));
            declarationList.Add(ExplicitInterfaceMember());
        }

        public void AddInitialisersToConstructor(List<StatementSyntax> constructorStatements)
        {
            constructorStatements.Add(InitialisationStatement(MockPropertyType));
        }

        private MemberDeclarationSyntax ExplicitInterfaceMember()
        {
            var decoratedValueTypeSyntax = ValueTypeSyntax;

            if (Symbol.ReturnsByRef)
            {
                decoratedValueTypeSyntax = F.RefType(decoratedValueTypeSyntax);
            }
            else if (Symbol.ReturnsByRefReadonly)
            {
                decoratedValueTypeSyntax = F.RefType(decoratedValueTypeSyntax).WithReadOnlyKeyword(F.Token(SyntaxKind.ReadOnlyKeyword));
            }

            var mockedProperty = F.PropertyDeclaration(decoratedValueTypeSyntax, Symbol.Name)
                .WithExplicitInterfaceSpecifier(F.ExplicitInterfaceSpecifier(TypesForSymbols.ParseName(InterfaceSymbol)));

            if (Symbol.IsReadOnly)
            {
                ExpressionSyntax elementAccess = F.MemberAccessExpression(SyntaxKind.SimpleMemberAccessExpression, F.IdentifierName(MemberMockName),
                    F.IdentifierName("Value"));

                if (Symbol.ReturnsByRef || Symbol.ReturnsByRefReadonly)
                {
                    elementAccess = TypesForSymbols.WrapByRef(elementAccess, ValueTypeSyntax);
                }

                mockedProperty = mockedProperty.WithExpressionBody(F.ArrowExpressionClause(elementAccess))
                    .WithSemicolonToken(F.Token(SyntaxKind.SemicolonToken));
            }
            else
            {
                if (!Symbol.IsWriteOnly)
                {
                    mockedProperty = mockedProperty.AddAccessorListAccessors(F.AccessorDeclaration(SyntaxKind.GetAccessorDeclaration)
                        .WithExpressionBody(F.ArrowExpressionClause(F.MemberAccessExpression(SyntaxKind.SimpleMemberAccessExpression,
                            F.IdentifierName(MemberMockName),
                            F.IdentifierName("Value"))))
                        .WithSemicolonToken(F.Token(SyntaxKind.SemicolonToken))
                    );
                }

                if (!Symbol.IsReadOnly)
                {
                    mockedProperty = mockedProperty.AddAccessorListAccessors(F.AccessorDeclaration(SyntaxKind.SetAccessorDeclaration)
                        .WithExpressionBody(
                            F.ArrowExpressionClause(
                                F.AssignmentExpression(SyntaxKind.SimpleAssignmentExpression,
                                    F.MemberAccessExpression(SyntaxKind.SimpleMemberAccessExpression, F.IdentifierName(MemberMockName),
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
