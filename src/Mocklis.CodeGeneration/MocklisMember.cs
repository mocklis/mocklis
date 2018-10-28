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

    public abstract class MocklisMember
    {
        public string MockPropertyName { get; set; }
        public abstract MemberDeclarationSyntax MockProperty();
        public abstract MemberDeclarationSyntax ExplicitInterfaceMember();
        public abstract TypeSyntax MockPropertyType { get; }

        protected MocklisMember(string mockPropertyName)
        {
            MockPropertyName = mockPropertyName;
        }
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

        public override MemberDeclarationSyntax MockProperty()
        {
            return F.PropertyDeclaration(MockPropertyType, MockPropertyName).AddModifiers(F.Token(SyntaxKind.PublicKeyword))
                .AddAccessorListAccessors(F.AccessorDeclaration(SyntaxKind.GetAccessorDeclaration)
                    .WithSemicolonToken(F.Token(SyntaxKind.SemicolonToken)))
                .WithInitializer(
                    F.EqualsValueClause(
                        F.ObjectCreationExpression(MockPropertyType)
                            .WithExpressionsAsArgumentList(
                                F.LiteralExpression(SyntaxKind.StringLiteralExpression, F.Literal(InterfaceSymbol.Name)),
                                F.LiteralExpression(SyntaxKind.StringLiteralExpression, F.Literal(Symbol.Name)),
                                F.LiteralExpression(SyntaxKind.StringLiteralExpression, F.Literal(MockPropertyName))
                            ))).WithSemicolonToken(F.Token(SyntaxKind.SemicolonToken));
        }


    }
}
