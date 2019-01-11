// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PropertyBasedMock.cs">
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

    public abstract class PropertyBasedMock<TSymbol> where TSymbol : ISymbol
    {
        protected MocklisTypesForSymbols TypesForSymbols { get; }
        protected INamedTypeSymbol ClassSymbol { get; }
        protected INamedTypeSymbol InterfaceSymbol { get; }
        protected TSymbol Symbol { get; }
        protected string MemberMockName { get; }

        protected PropertyBasedMock(MocklisTypesForSymbols typesForSymbols, INamedTypeSymbol classSymbol, INamedTypeSymbol interfaceSymbol,
            TSymbol symbol, string memberMockName)
        {
            TypesForSymbols = typesForSymbols;
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

        protected ExpressionStatementSyntax InitialisationStatement(TypeSyntax mockPropertyType)
        {
            return F.ExpressionStatement(F.AssignmentExpression(SyntaxKind.SimpleAssignmentExpression,
                F.MemberAccessExpression(SyntaxKind.SimpleMemberAccessExpression, F.ThisExpression(), F.IdentifierName(MemberMockName)),
                F.ObjectCreationExpression(mockPropertyType)
                    .WithExpressionsAsArgumentList(
                        F.ThisExpression(),
                        F.LiteralExpression(SyntaxKind.StringLiteralExpression, F.Literal(ClassSymbol.Name)),
                        F.LiteralExpression(SyntaxKind.StringLiteralExpression, F.Literal(InterfaceSymbol.Name)),
                        F.LiteralExpression(SyntaxKind.StringLiteralExpression, F.Literal(Symbol.Name)),
                        F.LiteralExpression(SyntaxKind.StringLiteralExpression, F.Literal(MemberMockName))
                    )));
        }
    }
}
