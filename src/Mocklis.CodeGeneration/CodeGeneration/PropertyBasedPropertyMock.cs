// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PropertyBasedPropertyMock.cs">
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

    public class PropertyBasedPropertyMock : IMemberMock
    {
        protected string ClassName { get; }
        protected INamedTypeSymbol InterfaceSymbol { get; }
        protected string InterfaceName { get; }
        protected IPropertySymbol Symbol { get; }
        protected string MemberMockName { get; }

        public PropertyBasedPropertyMock(INamedTypeSymbol classSymbol, INamedTypeSymbol interfaceSymbol, IPropertySymbol symbol, string mockMemberName)
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
            private TypeSyntax ValueTypeSyntax { get; }
            private TypeSyntax MockPropertyType { get; }
            private MocklisTypesForSymbols TypesForSymbols { get; }
            private bool Strict { get; }
            private bool VeryStrict { get; }
            private PropertyBasedPropertyMock Mock { get; }
        

            public SyntaxAdder(PropertyBasedPropertyMock mock, MocklisTypesForSymbols typesForSymbols, bool strict, bool veryStrict)
            {
                TypesForSymbols = typesForSymbols;
                Strict = strict;
                VeryStrict = veryStrict;
                Mock = mock;
                ValueTypeSyntax = typesForSymbols.ParseTypeName(mock.Symbol.Type, mock.Symbol.NullableOrOblivious());
                MockPropertyType = typesForSymbols.PropertyMock(ValueTypeSyntax);
            }

            public void AddMembersToClass(IList<MemberDeclarationSyntax> declarationList, NameSyntax interfaceNameSyntax)
            {
                declarationList.Add(MockPropertyType.MockProperty(Mock.MemberMockName));
                declarationList.Add(ExplicitInterfaceMember(interfaceNameSyntax));
            }

            public ITypeSymbol InterfaceSymbol => Mock.InterfaceSymbol;

            public void AddInitialisersToConstructor(List<StatementSyntax> constructorStatements)
            {
                constructorStatements.Add(TypesForSymbols.InitialisationStatement(MockPropertyType, Mock.MemberMockName, Mock.ClassName, Mock.InterfaceName, Mock.Symbol.Name, Strict, VeryStrict));
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
                    ExpressionSyntax elementAccess = F.MemberAccessExpression(SyntaxKind.SimpleMemberAccessExpression, F.IdentifierName(Mock.MemberMockName),
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
}
