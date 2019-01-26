// --------------------------------------------------------------------------------------------------------------------
// <copyright file="VirtualMethodBasedIndexerMock.cs">
//   Copyright © 2019 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.CodeGeneration
{
    #region Using Directives

    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp;
    using Microsoft.CodeAnalysis.CSharp.Syntax;
    using F = Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

    #endregion

    public sealed class VirtualMethodBasedIndexerMock : VirtualMethodBasedMock<IPropertySymbol>, IMemberMock
    {
        private RefTypeSyntax ValueTypeSyntax { get; }

        public VirtualMethodBasedIndexerMock(MocklisTypesForSymbols typesForSymbols, INamedTypeSymbol classSymbol, INamedTypeSymbol interfaceSymbol,
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
                .WithParameterList(F.ParameterList(F.SeparatedList(Symbol.Parameters.Select(a =>
                    F.Parameter(F.Identifier(a.Name)).WithType(TypesForSymbols.ParseTypeName(a.Type))))))
                .WithBody(F.Block(ThrowMockMissingStatement("VirtualIndexerGet")));
        }

        private MemberDeclarationSyntax ExplicitInterfaceMember()
        {
            var type = Symbol.ReturnsByRefReadonly ? ValueTypeSyntax.WithReadOnlyKeyword(F.Token(SyntaxKind.ReadOnlyKeyword)) : ValueTypeSyntax;

            var mockedIndexer = F.IndexerDeclaration(type)
                .WithParameterList(F.BracketedParameterList(F.SeparatedList(Symbol.Parameters.Select(a =>
                    F.Parameter(F.Identifier(a.Name)).WithType(TypesForSymbols.ParseTypeName(a.Type))))))
                .WithExplicitInterfaceSpecifier(F.ExplicitInterfaceSpecifier(TypesForSymbols.ParseName(InterfaceSymbol)));

            mockedIndexer = mockedIndexer
                .WithExpressionBody(F.ArrowExpressionClause(F.RefExpression(F.InvocationExpression(F.IdentifierName(MemberMockName),
                    F.ArgumentList(F.SeparatedList(Symbol.Parameters.Select(a => F.Argument(F.IdentifierName(a.Name)))))))))
                .WithSemicolonToken(F.Token(SyntaxKind.SemicolonToken));

            return mockedIndexer;
        }
    }
}
