// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MocklisIndexer.cs">
//   Copyright © 2018 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.CodeGeneration
{
    #region Using Directives

    using System.Linq;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp;
    using Microsoft.CodeAnalysis.CSharp.Syntax;
    using F = Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

    #endregion

    public class MocklisIndexer : MocklisMember<IPropertySymbol>
    {
        private bool IsMultiDimensional { get; }
        private TypeSyntax KeyTypeSyntax { get; }
        private TypeSyntax ValueTypeSyntax { get; }

        public override TypeSyntax MockPropertyType { get; }

        public MocklisIndexer(MocklisClass mocklisClass, INamedTypeSymbol interfaceSymbol, IPropertySymbol symbol) : base(mocklisClass,
            interfaceSymbol, symbol)
        {
            IsMultiDimensional = symbol.Parameters.Length > 1;

            KeyTypeSyntax = IsMultiDimensional
                ? F.TupleType(F.SeparatedList(symbol.Parameters.Select(a =>
                    F.TupleElement(mocklisClass.ParseTypeName(a.Type), F.Identifier(a.Name)))))
                : mocklisClass.ParseTypeName(symbol.Parameters[0].Type);

            ValueTypeSyntax = mocklisClass.ParseTypeName(symbol.Type);

            MockPropertyType = mocklisClass.IndexerMock(KeyTypeSyntax, ValueTypeSyntax);
        }

        public override MemberDeclarationSyntax ExplicitInterfaceMember(string memberMockName)
        {
            var mockedIndexer = F.IndexerDeclaration(ValueTypeSyntax)
                .WithParameterList(F.BracketedParameterList(F.SeparatedList(Symbol.Parameters.Select(a =>
                    F.Parameter(F.Identifier(a.Name)).WithType(MocklisClass.ParseTypeName(a.Type))))))
                .WithExplicitInterfaceSpecifier(F.ExplicitInterfaceSpecifier(InterfaceName));

            var keyParameter = IsMultiDimensional
                ? (ExpressionSyntax)F.TupleExpression(F.SeparatedList(Symbol.Parameters
                    .Select(a => F.Argument(F.IdentifierName(a.Name))).ToArray()))
                : F.IdentifierName(Symbol.Parameters[0].Name);

            if (Symbol.IsReadOnly)
            {
                mockedIndexer = mockedIndexer.WithExpressionBody(F.ArrowExpressionClause(
                        F.ElementAccessExpression(F.IdentifierName(memberMockName))
                            .WithExpressionsAsArgumentList(keyParameter)))
                    .WithSemicolonToken(F.Token(SyntaxKind.SemicolonToken));
            }
            else
            {
                if (!Symbol.IsWriteOnly)
                {
                    mockedIndexer = mockedIndexer.AddAccessorListAccessors(F.AccessorDeclaration(SyntaxKind.GetAccessorDeclaration)
                        .WithExpressionBody(F.ArrowExpressionClause(F.ElementAccessExpression(F.IdentifierName(memberMockName))
                            .WithExpressionsAsArgumentList(keyParameter)))
                        .WithSemicolonToken(F.Token(SyntaxKind.SemicolonToken))
                    );
                }

                if (!Symbol.IsReadOnly)
                {
                    mockedIndexer = mockedIndexer.AddAccessorListAccessors(F.AccessorDeclaration(SyntaxKind.SetAccessorDeclaration)
                        .WithExpressionBody(F.ArrowExpressionClause(
                            F.AssignmentExpression(SyntaxKind.SimpleAssignmentExpression,
                                F.ElementAccessExpression(F.IdentifierName(memberMockName)).WithExpressionsAsArgumentList(keyParameter),
                                F.IdentifierName("value"))))
                        .WithSemicolonToken(F.Token(SyntaxKind.SemicolonToken)));
                }
            }

            return mockedIndexer;
        }
    }
}
