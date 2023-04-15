// --------------------------------------------------------------------------------------------------------------------
// <copyright file="VirtualMethodBasedIndexerMock.cs">
//   SPDX-License-Identifier: MIT
//   Copyright © 2019-2021 Esbjörn Redmo and contributors. All rights reserved.
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
    using Mocklis.CodeGeneration.Compatibility;
    using Mocklis.CodeGeneration.UniqueNames;
    using F = Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

    #endregion

    public sealed class VirtualMethodBasedIndexerMock : VirtualMethodBasedMock<IPropertySymbol>, IMemberMock, ISyntaxAdder
    {
        public VirtualMethodBasedIndexerMock(INamedTypeSymbol classSymbol, INamedTypeSymbol interfaceSymbol, IPropertySymbol symbol, string mockMemberName) : base(classSymbol, interfaceSymbol, symbol, mockMemberName)
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
                .WithParameterList(F.ParameterList(F.SeparatedList(Symbol.Parameters.Select(a =>
                    F.Parameter(F.Identifier(a.Name)).WithType(typesForSymbols.ParseTypeName(a.Type, a.NullableOrOblivious()))))))
                .WithBody(F.Block(ThrowMockMissingStatement(typesForSymbols, "VirtualIndexerGet")));
        }

        private MemberDeclarationSyntax MockSetVirtualMethod(MocklisTypesForSymbols typesForSymbols, TypeSyntax valueTypeSyntax)
        {
            var uniquifier = new Uniquifier(Symbol.Parameters.Select(p => p.Name));

            var parameterList = F.SeparatedList(Symbol.Parameters.Select(a =>
                    F.Parameter(F.Identifier(a.Name)).WithType(typesForSymbols.ParseTypeName(a.Type, a.NullableOrOblivious()))))
                .Add(F.Parameter(F.Identifier(uniquifier.GetUniqueName("value"))).WithType(valueTypeSyntax));

            return F.MethodDeclaration(F.PredefinedType(F.Token(SyntaxKind.VoidKeyword)), F.Identifier(MemberMockName))
                .WithModifiers(F.TokenList(F.Token(SyntaxKind.ProtectedKeyword), F.Token(SyntaxKind.VirtualKeyword)))
                .WithParameterList(F.ParameterList(parameterList))
                .WithBody(F.Block(ThrowMockMissingStatement(typesForSymbols, "VirtualIndexerSet")));
        }

        private MemberDeclarationSyntax ExplicitInterfaceMember(MocklisTypesForSymbols typesForSymbols, TypeSyntax valueWithReadonlyTypeSyntax)
        {
            var mockedIndexer = F.IndexerDeclaration(valueWithReadonlyTypeSyntax)
                .WithParameterList(F.BracketedParameterList(F.SeparatedList(Symbol.Parameters.Select(a =>
                    F.Parameter(F.Identifier(a.Name)).WithType(typesForSymbols.ParseTypeName(a.Type, a.NullableOrOblivious()))))))
                .WithExplicitInterfaceSpecifier(F.ExplicitInterfaceSpecifier(typesForSymbols.ParseName(InterfaceSymbol)));

            if (Symbol.IsReadOnly)
            {
                ExpressionSyntax invocation = F.InvocationExpression(F.IdentifierName(MemberMockName),
                    F.ArgumentList(F.SeparatedList(Symbol.Parameters.Select(a => F.Argument(F.IdentifierName(a.Name))))));
                if (Symbol.ReturnsByRef || Symbol.ReturnsByRefReadonly)
                {
                    invocation = F.RefExpression(invocation);
                }

                mockedIndexer = mockedIndexer
                    .WithExpressionBody(F.ArrowExpressionClause(invocation))
                    .WithSemicolonToken(F.Token(SyntaxKind.SemicolonToken));
            }
            else
            {
                if (!Symbol.IsWriteOnly)
                {
                    var argumentList = F.SeparatedList(Symbol.Parameters.Select(a => F.Argument(F.IdentifierName(a.Name))));

                    mockedIndexer = mockedIndexer.AddAccessorListAccessors(F.AccessorDeclaration(SyntaxKind.GetAccessorDeclaration)
                        .WithExpressionBody(F.ArrowExpressionClause(F.InvocationExpression(F.IdentifierName(MemberMockName))
                            .WithArgumentList(F.ArgumentList(argumentList))))
                        .WithSemicolonToken(F.Token(SyntaxKind.SemicolonToken))
                    );
                }

                if (!Symbol.IsReadOnly)
                {
                    var argumentList = F.SeparatedList(Symbol.Parameters.Select(a => F.Argument(F.IdentifierName(a.Name))))
                        .Add(F.Argument(F.IdentifierName("value")));

                    mockedIndexer = mockedIndexer.AddAccessorListAccessors(F.AccessorDeclaration(SyntaxKind.SetAccessorDeclaration)
                        .WithExpressionBody(F.ArrowExpressionClause(F.InvocationExpression(F.IdentifierName(MemberMockName),
                            F.ArgumentList(argumentList))))
                        .WithSemicolonToken(F.Token(SyntaxKind.SemicolonToken))
                    );
                }
            }

            return mockedIndexer;
        }

        public ISyntaxAdder GetSyntaxAdder() => this;
    }
}
