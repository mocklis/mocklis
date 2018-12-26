// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MocklisRefIndexer.cs">
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

    public class MocklisRefIndexer : MocklisMember<IPropertySymbol>
    {
        private RefTypeSyntax ValueTypeSyntax { get; }
        public override TypeSyntax MockPropertyType { get; }

        public MocklisRefIndexer(MocklisClass mocklisClass, INamedTypeSymbol interfaceSymbol, IPropertySymbol symbol) : base(mocklisClass,
            interfaceSymbol, symbol)
        {
            ValueTypeSyntax = F.RefType(mocklisClass.ParseTypeName(symbol.Type));
        }

        public override StatementSyntax InitialiseMockProperty(string memberMockName)
        {
            return null;
        }

        public override MemberDeclarationSyntax MockProperty(string memberMockName)
        {
            return F.MethodDeclaration(ValueTypeSyntax, F.Identifier(memberMockName))
                .WithModifiers(F.TokenList(F.Token(SyntaxKind.ProtectedKeyword), F.Token(SyntaxKind.VirtualKeyword)))
                .WithParameterList(F.ParameterList(F.SeparatedList(Symbol.Parameters.Select(a =>
                    F.Parameter(F.Identifier(a.Name)).WithType(MocklisClass.ParseTypeName(a.Type))))))
                .WithBody(
                    F.Block(F.ThrowStatement(F.ObjectCreationExpression(MocklisClass.MockMissingException)
                            .WithExpressionsAsArgumentList(
                                F.MemberAccessExpression(SyntaxKind.SimpleMemberAccessExpression, MocklisClass.MockType,
                                    F.IdentifierName("VirtualIndexerGet")),
                                F.LiteralExpression(SyntaxKind.StringLiteralExpression, F.Literal(MocklisClass.Name)),
                                F.LiteralExpression(SyntaxKind.StringLiteralExpression, F.Literal(InterfaceSymbol.Name)),
                                F.LiteralExpression(SyntaxKind.StringLiteralExpression, F.Literal(Symbol.Name)),
                                F.LiteralExpression(SyntaxKind.StringLiteralExpression, F.Literal(memberMockName))
                            )
                        )
                    ));
        }

        public override MemberDeclarationSyntax ExplicitInterfaceMember(string memberMockName)
        {
            var type = Symbol.ReturnsByRefReadonly ? ValueTypeSyntax.WithReadOnlyKeyword(F.Token(SyntaxKind.ReadOnlyKeyword)) : ValueTypeSyntax;

            var mockedIndexer = F.IndexerDeclaration(type)
                .WithParameterList(F.BracketedParameterList(F.SeparatedList(Symbol.Parameters.Select(a =>
                    F.Parameter(F.Identifier(a.Name)).WithType(MocklisClass.ParseTypeName(a.Type))))))
                .WithExplicitInterfaceSpecifier(F.ExplicitInterfaceSpecifier(InterfaceName));

            mockedIndexer = mockedIndexer
                .WithExpressionBody(F.ArrowExpressionClause(F.RefExpression(F.InvocationExpression(F.IdentifierName(memberMockName),
                    F.ArgumentList(F.SeparatedList(Symbol.Parameters.Select(a => F.Argument(F.IdentifierName(a.Name)))))))))
                .WithSemicolonToken(F.Token(SyntaxKind.SemicolonToken));

            return mockedIndexer;
        }
    }
}
