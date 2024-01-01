// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PropertyBasedMock.cs">
//   SPDX-License-Identifier: MIT
//   Copyright © 2019-2023 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.MockGenerator.CodeGeneration
{
    #region Using Directives

    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp;
    using Microsoft.CodeAnalysis.CSharp.Syntax;
    using F = Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

    #endregion

    public abstract class PropertyBasedMock<TSymbol> where TSymbol : ISymbol
    {
        protected INamedTypeSymbol ClassSymbol { get; }
        protected INamedTypeSymbol InterfaceSymbol { get; }
        protected TSymbol Symbol { get; }
        protected string MemberMockName { get; }

        protected PropertyBasedMock(INamedTypeSymbol classSymbol, INamedTypeSymbol interfaceSymbol, TSymbol symbol, string memberMockName)
        {
            ClassSymbol = classSymbol;
            InterfaceSymbol = interfaceSymbol;
            Symbol = symbol;
            MemberMockName = memberMockName;
        }

        protected PropertyDeclarationSyntax MockProperty(TypeSyntax mockPropertyType)
        {
            return F.PropertyDeclaration(mockPropertyType, MemberMockName).AddModifiers(F.Token(SyntaxKind.PublicKeyword))
                .AddAccessorListAccessors(F.AccessorDeclaration(SyntaxKind.GetAccessorDeclaration)
                    .WithSemicolonToken(F.Token(SyntaxKind.SemicolonToken)));
        }

        protected ExpressionStatementSyntax InitialisationStatement(TypeSyntax mockPropertyType, MocklisTypesForSymbols typesForSymbols, bool strict, bool veryStrict)
        {
            return F.ExpressionStatement(F.AssignmentExpression(SyntaxKind.SimpleAssignmentExpression,
                F.IdentifierName(MemberMockName),
                F.ObjectCreationExpression(mockPropertyType)
                    .WithExpressionsAsArgumentList(
                        F.ThisExpression(),
                        F.LiteralExpression(SyntaxKind.StringLiteralExpression, F.Literal(ClassSymbol.Name)),
                        F.LiteralExpression(SyntaxKind.StringLiteralExpression, F.Literal(InterfaceSymbol.Name)),
                        F.LiteralExpression(SyntaxKind.StringLiteralExpression, F.Literal(Symbol.Name)),
                        F.LiteralExpression(SyntaxKind.StringLiteralExpression, F.Literal(MemberMockName)),
                        typesForSymbols.StrictnessExpression(strict, veryStrict)
                    )));
        }
    }
}
