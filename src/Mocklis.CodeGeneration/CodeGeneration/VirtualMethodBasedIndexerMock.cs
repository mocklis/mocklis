// --------------------------------------------------------------------------------------------------------------------
// <copyright file="VirtualMethodBasedIndexerMock.cs">
//   SPDX-License-Identifier: MIT
//   Copyright © 2019-2020 Esbjörn Redmo and contributors. All rights reserved.
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

    public sealed class VirtualMethodBasedIndexerMock : VirtualMethodBasedMock<IPropertySymbol>, IMemberMock
    {
        private TypeSyntax ValueTypeSyntax { get; }
        private TypeSyntax ValueWithReadonlyTypeSyntax { get; }

        public VirtualMethodBasedIndexerMock(MocklisTypesForSymbols typesForSymbols, INamedTypeSymbol classSymbol, INamedTypeSymbol interfaceSymbol,
            IPropertySymbol symbol,
            string mockMemberName) : base(typesForSymbols, classSymbol, interfaceSymbol, symbol, mockMemberName)
        {
            ValueTypeSyntax = typesForSymbols.ParseTypeName(symbol.Type, symbol.NullableOrOblivious());
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
                .WithParameterList(F.ParameterList(F.SeparatedList(Symbol.Parameters.Select(a =>
                    F.Parameter(F.Identifier(a.Name)).WithType(TypesForSymbols.ParseTypeName(a.Type, a.NullableOrOblivious()))))))
                .WithBody(F.Block(ThrowMockMissingStatement("VirtualIndexerGet")));
        }

        private MemberDeclarationSyntax MockSetVirtualMethod()
        {
            var uniquifier = new Uniquifier(Symbol.Parameters.Select(p => p.Name));

            var parameterList = F.SeparatedList(Symbol.Parameters.Select(a =>
                    F.Parameter(F.Identifier(a.Name)).WithType(TypesForSymbols.ParseTypeName(a.Type, a.NullableOrOblivious()))))
                .Add(F.Parameter(F.Identifier(uniquifier.GetUniqueName("value"))).WithType(ValueTypeSyntax));

            return F.MethodDeclaration(F.PredefinedType(F.Token(SyntaxKind.VoidKeyword)), F.Identifier(MemberMockName))
                .WithModifiers(F.TokenList(F.Token(SyntaxKind.ProtectedKeyword), F.Token(SyntaxKind.VirtualKeyword)))
                .WithParameterList(F.ParameterList(parameterList))
                .WithBody(F.Block(ThrowMockMissingStatement("VirtualIndexerSet")));
        }

        private MemberDeclarationSyntax ExplicitInterfaceMember()
        {
            var mockedIndexer = F.IndexerDeclaration(ValueWithReadonlyTypeSyntax)
                .WithParameterList(F.BracketedParameterList(F.SeparatedList(Symbol.Parameters.Select(a =>
                    F.Parameter(F.Identifier(a.Name)).WithType(TypesForSymbols.ParseTypeName(a.Type, a.NullableOrOblivious()))))))
                .WithExplicitInterfaceSpecifier(F.ExplicitInterfaceSpecifier(TypesForSymbols.ParseName(InterfaceSymbol)));

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
    }
}
