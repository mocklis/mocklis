// --------------------------------------------------------------------------------------------------------------------
// <copyright file="VirtualMethodBasedPropertyMock.cs">
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

    public sealed class VirtualMethodBasedPropertyMock : VirtualMethodBasedMock<IPropertySymbol>, IMemberMock
    {
        public VirtualMethodBasedPropertyMock(INamedTypeSymbol classSymbol, INamedTypeSymbol interfaceSymbol, IPropertySymbol symbol, string mockMemberName) : base(classSymbol, interfaceSymbol, symbol, mockMemberName)
        {
        }

        public void AddMembersToClass(IList<MemberDeclarationSyntax> declarationList, MocklisTypesForSymbols typesForSymbols, bool strict,
            bool veryStrict)
        {
            var valueTypeSyntax = typesForSymbols.ParseTypeName(Symbol.Type, Symbol.NullableOrOblivious());
            var valueWithReadonlyTypeSyntax = valueTypeSyntax;

            if (Symbol.ReturnsByRef || Symbol.ReturnsByRefReadonly)
            {
                RefTypeSyntax tmp = F.RefType(valueTypeSyntax);
                valueTypeSyntax = tmp;
                valueWithReadonlyTypeSyntax = tmp;
                if (Symbol.ReturnsByRefReadonly)
                {
                    valueWithReadonlyTypeSyntax = tmp.WithReadOnlyKeyword(F.Token(SyntaxKind.ReadOnlyKeyword));
                }
            }

            if (!Symbol.IsWriteOnly)
            {
                declarationList.Add(MockGetVirtualMethod(typesForSymbols, valueTypeSyntax));
            }

            if (!Symbol.IsReadOnly)
            {
                declarationList.Add(MockSetVirtualMethod(typesForSymbols, valueTypeSyntax));
            }

            declarationList.Add(ExplicitInterfaceMember(typesForSymbols, valueWithReadonlyTypeSyntax));
        }

        public void AddInitialisersToConstructor(List<StatementSyntax> constructorStatements, MocklisTypesForSymbols typesForSymbols, bool strict,
            bool veryStrict)
        {
        }

        private MemberDeclarationSyntax MockGetVirtualMethod(MocklisTypesForSymbols typesForSymbols, TypeSyntax valueTypeSyntax)
        {
            return F.MethodDeclaration(valueTypeSyntax, F.Identifier(MemberMockName))
                .WithModifiers(F.TokenList(F.Token(SyntaxKind.ProtectedKeyword), F.Token(SyntaxKind.VirtualKeyword)))
                .WithBody(F.Block(ThrowMockMissingStatement(typesForSymbols, "VirtualPropertyGet")));
        }

        private MemberDeclarationSyntax MockSetVirtualMethod(MocklisTypesForSymbols typesForSymbols, TypeSyntax valueTypeSyntax)
        {
            return F.MethodDeclaration(F.PredefinedType(F.Token(SyntaxKind.VoidKeyword)), F.Identifier(MemberMockName))
                .WithParameterList(F.ParameterList(F.SeparatedList(new[]
                {
                    F.Parameter(F.Identifier("value")).WithType(valueTypeSyntax)
                })))
                .WithModifiers(F.TokenList(F.Token(SyntaxKind.ProtectedKeyword), F.Token(SyntaxKind.VirtualKeyword)))
                .WithBody(F.Block(ThrowMockMissingStatement(typesForSymbols, "VirtualPropertySet")));
        }

        private MemberDeclarationSyntax ExplicitInterfaceMember(MocklisTypesForSymbols typesForSymbols, TypeSyntax valueWithReadonlyTypeSyntax)
        {
            var mockedProperty = F.PropertyDeclaration(valueWithReadonlyTypeSyntax, Symbol.Name)
                .WithExplicitInterfaceSpecifier(F.ExplicitInterfaceSpecifier(typesForSymbols.ParseName(InterfaceSymbol)));

            if (Symbol.IsReadOnly)
            {
                ExpressionSyntax invocation = F.InvocationExpression(F.IdentifierName(MemberMockName));
                if (Symbol.ReturnsByRef || Symbol.ReturnsByRefReadonly)
                {
                    invocation = F.RefExpression(invocation);
                }

                mockedProperty = mockedProperty
                    .WithExpressionBody(F.ArrowExpressionClause(invocation))
                    .WithSemicolonToken(F.Token(SyntaxKind.SemicolonToken));
            }
            else
            {
                if (!Symbol.IsWriteOnly)
                {
                    mockedProperty = mockedProperty.AddAccessorListAccessors(F.AccessorDeclaration(SyntaxKind.GetAccessorDeclaration)
                        .WithExpressionBody(F.ArrowExpressionClause(F.InvocationExpression(F.IdentifierName(MemberMockName))))
                        .WithSemicolonToken(F.Token(SyntaxKind.SemicolonToken))
                    );
                }

                if (!Symbol.IsReadOnly)
                {
                    mockedProperty = mockedProperty.AddAccessorListAccessors(F.AccessorDeclaration(SyntaxKind.SetAccessorDeclaration)
                        .WithExpressionBody(F.ArrowExpressionClause(F.InvocationExpression(F.IdentifierName(MemberMockName),
                            F.ArgumentList(F.SeparatedList(new[] { F.Argument(F.IdentifierName("value")) })))))
                        .WithSemicolonToken(F.Token(SyntaxKind.SemicolonToken))
                    );
                }
            }

            return mockedProperty;
        }
    }
}
