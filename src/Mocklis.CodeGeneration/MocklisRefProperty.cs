// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MocklisRefProperty.cs">
//   Copyright © 2018 Esbjörn Redmo and contributors. All rights reserved.
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

    public class MocklisRefProperty : MocklisMember<IPropertySymbol>
    {
        private RefTypeSyntax ValueTypeSyntax { get; }
        public override TypeSyntax MockPropertyType { get; }

        public MocklisRefProperty(MocklisClass mocklisClass, INamedTypeSymbol interfaceSymbol, IPropertySymbol symbol) : base(mocklisClass,
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
                .WithBody(
                    F.Block(F.ThrowStatement(F.ObjectCreationExpression(MocklisClass.MockMissingException)
                            .WithExpressionsAsArgumentList(
                                F.MemberAccessExpression(SyntaxKind.SimpleMemberAccessExpression, MocklisClass.MockType,
                                    F.IdentifierName("VirtualPropertyGet")),
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

            var mockedProperty = F.PropertyDeclaration(type, Symbol.Name)
                .WithExplicitInterfaceSpecifier(F.ExplicitInterfaceSpecifier(InterfaceName));

            mockedProperty = mockedProperty
                .WithExpressionBody(F.ArrowExpressionClause(F.RefExpression(F.InvocationExpression(F.IdentifierName(memberMockName)))))
                .WithSemicolonToken(F.Token(SyntaxKind.SemicolonToken));

            return mockedProperty;
        }
    }
}
