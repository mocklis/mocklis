// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PropertyBasedIndexerMock.cs">
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

    public class PropertyBasedIndexerMock : PropertyBasedMock<IPropertySymbol>, IMemberMock
    {
        private bool IsMultiDimensional { get; }
        private TypeSyntax KeyTypeSyntax { get; }
        private TypeSyntax ValueTypeSyntax { get; }
        private TypeSyntax MockPropertyType { get; }

        public PropertyBasedIndexerMock(MocklisTypesForSymbols typesForSymbols, INamedTypeSymbol classSymbol, INamedTypeSymbol interfaceSymbol,
            IPropertySymbol symbol,
            string mockMemberName) : base(typesForSymbols,
            classSymbol, interfaceSymbol, symbol, mockMemberName)
        {
            IsMultiDimensional = symbol.Parameters.Length > 1;

            KeyTypeSyntax = IsMultiDimensional
                ? F.TupleType(F.SeparatedList(symbol.Parameters.Select(a =>
                    F.TupleElement(typesForSymbols.ParseTypeName(a.Type), F.Identifier(a.Name)))))
                : typesForSymbols.ParseTypeName(symbol.Parameters[0].Type);

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
                .WithParameterList(F.BracketedParameterList(F.SeparatedList(Symbol.Parameters.Select(a =>
                    F.Parameter(F.Identifier(a.Name)).WithType(TypesForSymbols.ParseTypeName(a.Type))))))
                .WithExplicitInterfaceSpecifier(F.ExplicitInterfaceSpecifier(TypesForSymbols.ParseName(InterfaceSymbol)));

            var keyParameter = IsMultiDimensional
                ? (ExpressionSyntax)F.TupleExpression(F.SeparatedList(Symbol.Parameters
                    .Select(a => F.Argument(F.IdentifierName(a.Name))).ToArray()))
                : F.IdentifierName(Symbol.Parameters[0].Name);

            if (Symbol.IsReadOnly)
            {
                ExpressionSyntax elementAccess = F.ElementAccessExpression(F.IdentifierName(MemberMockName))
                    .WithExpressionsAsArgumentList(keyParameter);

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
                            .WithExpressionsAsArgumentList(keyParameter)))
                        .WithSemicolonToken(F.Token(SyntaxKind.SemicolonToken))
                    );
                }

                if (!Symbol.IsReadOnly)
                {
                    mockedIndexer = mockedIndexer.AddAccessorListAccessors(F.AccessorDeclaration(SyntaxKind.SetAccessorDeclaration)
                        .WithExpressionBody(F.ArrowExpressionClause(
                            F.AssignmentExpression(SyntaxKind.SimpleAssignmentExpression,
                                F.ElementAccessExpression(F.IdentifierName(MemberMockName)).WithExpressionsAsArgumentList(keyParameter),
                                F.IdentifierName("value"))))
                        .WithSemicolonToken(F.Token(SyntaxKind.SemicolonToken)));
                }
            }

            return mockedIndexer;
        }
    }
}
