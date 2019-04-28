// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PropertyBasedIndexerMock.cs">
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

    public class PropertyBasedIndexerMock : PropertyBasedMock<IPropertySymbol>, IMemberMock
    {
        private SingleTypeOrValueTuple KeyType { get; }
        private TypeSyntax KeyTypeSyntax { get; }
        private TypeSyntax ValueTypeSyntax { get; }
        private TypeSyntax MockPropertyType { get; }

        public PropertyBasedIndexerMock(MocklisTypesForSymbols typesForSymbols, INamedTypeSymbol classSymbol, INamedTypeSymbol interfaceSymbol,
            IPropertySymbol symbol,
            string mockMemberName) : base(typesForSymbols,
            classSymbol, interfaceSymbol, symbol, mockMemberName)
        {
            var builder = new SingleTypeOrValueTupleBuilder(TypesForSymbols);
            foreach (var p in symbol.Parameters)
            {
                builder.AddParameter(p);
            }

            KeyType = builder.Build(mockMemberName);

            KeyTypeSyntax = KeyType.BuildTypeSyntax();

            ValueTypeSyntax = typesForSymbols.ParseTypeName(symbol.Type);

            MockPropertyType = typesForSymbols.IndexerMock(KeyTypeSyntax, ValueTypeSyntax);
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

            var mockedIndexer = F.IndexerDeclaration(decoratedValueTypeSyntax)
                .WithParameterList(KeyType.BuildParameterList())
                .WithExplicitInterfaceSpecifier(F.ExplicitInterfaceSpecifier(TypesForSymbols.ParseName(InterfaceSymbol)));

            var arguments = KeyType.BuildArgumentList();

            if (Symbol.IsReadOnly)
            {
                ExpressionSyntax elementAccess = F.ElementAccessExpression(F.IdentifierName(MemberMockName))
                    .WithExpressionsAsArgumentList(arguments);

                if (Symbol.ReturnsByRef || Symbol.ReturnsByRefReadonly)
                {
                    elementAccess = TypesForSymbols.WrapByRef(elementAccess, ValueTypeSyntax);
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
}
