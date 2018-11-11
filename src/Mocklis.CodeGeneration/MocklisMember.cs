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
        public abstract MemberDeclarationSyntax ExplicitInterfaceMember(string mockPropertyName);
        public abstract TypeSyntax MockPropertyType { get; }
        public abstract TypeSyntax MockPropertyInterfaceType { get; }

        protected MocklisMember(string mockPropertyName)
        {
            PreferredName = mockPropertyName;
        }

        public string PreferredName { get; }

        public virtual ExpressionSyntax ConstructorArgument(string uniqueName)
        {
            return F.IdentifierName(uniqueName);
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

        public override MemberDeclarationSyntax MockProperty(string mockPropertyName)
        {
            return F.PropertyDeclaration(MockPropertyType, mockPropertyName).AddModifiers(F.Token(SyntaxKind.PublicKeyword))
                .AddAccessorListAccessors(F.AccessorDeclaration(SyntaxKind.GetAccessorDeclaration)
                    .WithSemicolonToken(F.Token(SyntaxKind.SemicolonToken)))
                .WithInitializer(
                    F.EqualsValueClause(
                        F.ObjectCreationExpression(MockPropertyType)
                            .WithExpressionsAsArgumentList(
                                F.LiteralExpression(SyntaxKind.StringLiteralExpression, F.Literal(InterfaceSymbol.Name)),
                                F.LiteralExpression(SyntaxKind.StringLiteralExpression, F.Literal(Symbol.Name)),
                                F.LiteralExpression(SyntaxKind.StringLiteralExpression, F.Literal(mockPropertyName))
                            ))).WithSemicolonToken(F.Token(SyntaxKind.SemicolonToken));
        }
    }
}
