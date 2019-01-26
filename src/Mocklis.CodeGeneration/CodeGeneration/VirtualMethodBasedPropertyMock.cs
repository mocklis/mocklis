// --------------------------------------------------------------------------------------------------------------------
// <copyright file="VirtualMethodBasedPropertyMock.cs">
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
        private RefTypeSyntax ValueTypeSyntax { get; }

        public VirtualMethodBasedPropertyMock(MocklisTypesForSymbols typesForSymbols, INamedTypeSymbol classSymbol, INamedTypeSymbol interfaceSymbol,
            IPropertySymbol symbol,
            string mockMemberName) : base(typesForSymbols, classSymbol, interfaceSymbol, symbol, mockMemberName)
        {
            ValueTypeSyntax = F.RefType(typesForSymbols.ParseTypeName(symbol.Type));
        }

        public void AddMembersToClass(IList<MemberDeclarationSyntax> declarationList)
        {
            declarationList.Add(MockVirtualMethod());
            declarationList.Add(ExplicitInterfaceMember());
        }

        public void AddInitialisersToConstructor(List<StatementSyntax> constructorStatements)
        {
        }

        private MemberDeclarationSyntax MockVirtualMethod()
        {
            return F.MethodDeclaration(ValueTypeSyntax, F.Identifier(MemberMockName))
                .WithModifiers(F.TokenList(F.Token(SyntaxKind.ProtectedKeyword), F.Token(SyntaxKind.VirtualKeyword)))
                .WithBody(F.Block(ThrowMockMissingStatement("VirtualPropertyGet")));
        }

        private MemberDeclarationSyntax ExplicitInterfaceMember()
        {
            var type = Symbol.ReturnsByRefReadonly ? ValueTypeSyntax.WithReadOnlyKeyword(F.Token(SyntaxKind.ReadOnlyKeyword)) : ValueTypeSyntax;

            var mockedProperty = F.PropertyDeclaration(type, Symbol.Name)
                .WithExplicitInterfaceSpecifier(F.ExplicitInterfaceSpecifier(TypesForSymbols.ParseName(InterfaceSymbol)));

            mockedProperty = mockedProperty
                .WithExpressionBody(F.ArrowExpressionClause(F.RefExpression(F.InvocationExpression(F.IdentifierName(MemberMockName)))))
                .WithSemicolonToken(F.Token(SyntaxKind.SemicolonToken));

            return mockedProperty;
        }
    }
}
