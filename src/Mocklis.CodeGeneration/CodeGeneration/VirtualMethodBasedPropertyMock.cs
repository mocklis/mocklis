// --------------------------------------------------------------------------------------------------------------------
// <copyright file="VirtualMethodBasedPropertyMock.cs">
//   SPDX-License-Identifier: MIT
//   Copyright © 2019 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.CodeGeneration
{
    #region Using Directives

    using System.Collections.Generic;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp;
    using Microsoft.CodeAnalysis.CSharp.Syntax;
    using F = Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

    #endregion

    public sealed class VirtualMethodBasedPropertyMock : VirtualMethodBasedMock<IPropertySymbol>, IMemberMock
    {
        private TypeSyntax ValueTypeSyntax { get; }
        private TypeSyntax ValueWithReadonlyTypeSyntax { get; }

        public VirtualMethodBasedPropertyMock(MocklisTypesForSymbols typesForSymbols, INamedTypeSymbol classSymbol, INamedTypeSymbol interfaceSymbol,
            IPropertySymbol symbol,
            string mockMemberName) : base(typesForSymbols, classSymbol, interfaceSymbol, symbol, mockMemberName)
        {
            ValueTypeSyntax = typesForSymbols.ParseTypeName(symbol.Type);
            ValueWithReadonlyTypeSyntax = ValueTypeSyntax;

            if (Symbol.ReturnsByRef || Symbol.ReturnsByRefReadonly)
            {
                RefTypeSyntax tmp = F.RefType(ValueTypeSyntax);
                ValueTypeSyntax = tmp;
                ValueWithReadonlyTypeSyntax = tmp;
                if (Symbol.ReturnsByRefReadonly)
                {
                    ValueWithReadonlyTypeSyntax = tmp.WithReadOnlyKeyword(F.Token(SyntaxKind.ReadOnlyKeyword));
                }
            }
        }

        public void AddMembersToClass(IList<MemberDeclarationSyntax> declarationList)
        {
            if (!Symbol.IsWriteOnly)
            {
                declarationList.Add(MockGetVirtualMethod());
            }

            if (!Symbol.IsReadOnly)
            {
                declarationList.Add(MockSetVirtualMethod());
            }

            declarationList.Add(ExplicitInterfaceMember());
        }

        public void AddInitialisersToConstructor(List<StatementSyntax> constructorStatements)
        {
        }

        private MemberDeclarationSyntax MockGetVirtualMethod()
        {
            return F.MethodDeclaration(ValueTypeSyntax, F.Identifier(MemberMockName))
                .WithModifiers(F.TokenList(F.Token(SyntaxKind.ProtectedKeyword), F.Token(SyntaxKind.VirtualKeyword)))
                .WithBody(F.Block(ThrowMockMissingStatement("VirtualPropertyGet")));
        }

        private MemberDeclarationSyntax MockSetVirtualMethod()
        {
            return F.MethodDeclaration(F.PredefinedType(F.Token(SyntaxKind.VoidKeyword)), F.Identifier(MemberMockName))
                .WithParameterList(F.ParameterList(F.SeparatedList(new[]
                {
                    F.Parameter(F.Identifier("Value")).WithType(ValueTypeSyntax)
                })))
                .WithModifiers(F.TokenList(F.Token(SyntaxKind.ProtectedKeyword), F.Token(SyntaxKind.VirtualKeyword)))
                .WithBody(F.Block(ThrowMockMissingStatement("VirtualPropertySet")));
        }

        private MemberDeclarationSyntax ExplicitInterfaceMember()
        {
            var mockedProperty = F.PropertyDeclaration(ValueWithReadonlyTypeSyntax, Symbol.Name)
                .WithExplicitInterfaceSpecifier(F.ExplicitInterfaceSpecifier(TypesForSymbols.ParseName(InterfaceSymbol)));

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
