// --------------------------------------------------------------------------------------------------------------------
// <copyright file="VirtualMethodBasedMock.cs">
//   Copyright © 2019 Esbjörn Redmo and contributors. All rights reserved.
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
        protected MocklisTypesForSymbols TypesForSymbols { get; }
        protected INamedTypeSymbol ClassSymbol { get; }
        protected INamedTypeSymbol InterfaceSymbol { get; }
        protected TSymbol Symbol { get; }
        protected string MemberMockName { get; }

        protected VirtualMethodBasedMock(MocklisTypesForSymbols typesForSymbols, INamedTypeSymbol classSymbol, INamedTypeSymbol interfaceSymbol,
            TSymbol symbol, string memberMockName)
        {
            TypesForSymbols = typesForSymbols;
            ClassSymbol = classSymbol;
            InterfaceSymbol = interfaceSymbol;
            Symbol = symbol;
            MemberMockName = memberMockName;
        }

        protected ThrowStatementSyntax ThrowMockMissingStatement(string mockType)
        {
            return F.ThrowStatement(F.ObjectCreationExpression(TypesForSymbols.MockMissingException)
                .WithExpressionsAsArgumentList(
                    F.MemberAccessExpression(SyntaxKind.SimpleMemberAccessExpression, TypesForSymbols.MockType,
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
