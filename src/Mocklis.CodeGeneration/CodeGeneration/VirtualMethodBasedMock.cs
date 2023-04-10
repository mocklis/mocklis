// --------------------------------------------------------------------------------------------------------------------
// <copyright file="VirtualMethodBasedMock.cs">
//   SPDX-License-Identifier: MIT
//   Copyright © 2019-2021 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.CodeGeneration
{
    #region Using Directives

    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp;
    using Microsoft.CodeAnalysis.CSharp.Syntax;
    using F = Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

    #endregion

    public abstract class VirtualMethodBasedMock<TSymbol> where TSymbol : ISymbol
    {
        protected INamedTypeSymbol ClassSymbol { get; }
        protected INamedTypeSymbol InterfaceSymbol { get; }
        protected TSymbol Symbol { get; }
        protected string MemberMockName { get; }

        protected VirtualMethodBasedMock(INamedTypeSymbol classSymbol, INamedTypeSymbol interfaceSymbol,
            TSymbol symbol, string memberMockName)
        {
            ClassSymbol = classSymbol;
            InterfaceSymbol = interfaceSymbol;
            Symbol = symbol;
            MemberMockName = memberMockName;
        }

        protected ThrowStatementSyntax ThrowMockMissingStatement(MocklisTypesForSymbols typesForSymbols, string mockType)
        {
            return F.ThrowStatement(F.ObjectCreationExpression(typesForSymbols.MockMissingException())
                .WithExpressionsAsArgumentList(
                    F.MemberAccessExpression(SyntaxKind.SimpleMemberAccessExpression, typesForSymbols.MockType(),
                        F.IdentifierName(mockType)),
                    F.LiteralExpression(SyntaxKind.StringLiteralExpression, F.Literal(ClassSymbol.Name)),
                    F.LiteralExpression(SyntaxKind.StringLiteralExpression, F.Literal(InterfaceSymbol.Name)),
                    F.LiteralExpression(SyntaxKind.StringLiteralExpression, F.Literal(Symbol.Name)),
                    F.LiteralExpression(SyntaxKind.StringLiteralExpression, F.Literal(MemberMockName))
                )
            );
        }
    }
}
