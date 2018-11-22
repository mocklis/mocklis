// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MocklisMember.cs">
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

    public abstract class MocklisMember : IHasPreferredName
    {
        public abstract MemberDeclarationSyntax MockProperty(string mockPropertyName);
        public abstract StatementSyntax InitialiseMockProperty(string mockPropertyName);

        public abstract MemberDeclarationSyntax ExplicitInterfaceMember(string mockPropertyName);

        public abstract TypeSyntax MockPropertyType { get; }

        protected MocklisMember(string mockPropertyName)
        {
            PreferredName = mockPropertyName;
        }

        public string PreferredName { get; }
    }

    public abstract class MocklisMember<TSymbol> : MocklisMember where TSymbol : ISymbol
    {
        public MocklisClass MocklisClass { get; }
        public INamedTypeSymbol InterfaceSymbol { get; }
        public NameSyntax InterfaceName { get; }
        public TSymbol Symbol { get; }

        public MocklisMember(MocklisClass mocklisClass, INamedTypeSymbol interfaceSymbol, TSymbol symbol) : base(symbol.MetadataName)
        {
            MocklisClass = mocklisClass;
            InterfaceSymbol = interfaceSymbol;
            Symbol = symbol;
            InterfaceName = MocklisClass.ParseName(InterfaceSymbol);
        }

        public override MemberDeclarationSyntax MockProperty(string mockPropertyName)
        {
            return F.PropertyDeclaration(MockPropertyType, mockPropertyName).AddModifiers(F.Token(SyntaxKind.PublicKeyword))
                .AddAccessorListAccessors(F.AccessorDeclaration(SyntaxKind.GetAccessorDeclaration)
                    .WithSemicolonToken(F.Token(SyntaxKind.SemicolonToken)));
        }

        public override StatementSyntax InitialiseMockProperty(string mockPropertyName)
        {
            return F.ExpressionStatement(F.AssignmentExpression(SyntaxKind.SimpleAssignmentExpression, F.IdentifierName(mockPropertyName),
                F.ObjectCreationExpression(MockPropertyType)
                    .WithExpressionsAsArgumentList(
                        F.ThisExpression(),
                        F.LiteralExpression(SyntaxKind.StringLiteralExpression, F.Literal(MocklisClass.Name)),
                        F.LiteralExpression(SyntaxKind.StringLiteralExpression, F.Literal(InterfaceSymbol.Name)),
                        F.LiteralExpression(SyntaxKind.StringLiteralExpression, F.Literal(Symbol.Name)),
                        F.LiteralExpression(SyntaxKind.StringLiteralExpression, F.Literal(mockPropertyName))
                    )));
        }
    }
}
