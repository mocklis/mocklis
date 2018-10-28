// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MocklisProperty.cs">
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

    public class MocklisProperty : MocklisMember<IPropertySymbol>
    {
        private TypeSyntax ValueTypeSyntax { get; }
        public override TypeSyntax MockPropertyType { get; }

        public MocklisProperty(MocklisClass mocklisClass, INamedTypeSymbol interfaceSymbol, IPropertySymbol symbol) : base(mocklisClass,
            interfaceSymbol, symbol)
        {
            ValueTypeSyntax = mocklisClass.ParseTypeName(symbol.Type);

            MockPropertyType = symbol.IsReadOnly ? mocklisClass.ReadOnlyPropertyMock(ValueTypeSyntax) :
                symbol.IsWriteOnly ? mocklisClass.WriteOnlyPropertyMock(ValueTypeSyntax) :
                mocklisClass.PropertyMock(ValueTypeSyntax);
        }

        public override MemberDeclarationSyntax ExplicitInterfaceMember()
        {
            var mockedProperty = F.PropertyDeclaration(ValueTypeSyntax, Symbol.Name)
                .WithExplicitInterfaceSpecifier(F.ExplicitInterfaceSpecifier(InterfaceName));

            if (!Symbol.IsWriteOnly)
            {
                mockedProperty = mockedProperty.AddAccessorListAccessors(F.AccessorDeclaration(SyntaxKind.GetAccessorDeclaration)
                    .WithExpressionBody(F.ArrowExpressionClause(F.InvocationExpression(
                        F.MemberAccessExpression(SyntaxKind.SimpleMemberAccessExpression, F.IdentifierName(MockPropertyName),
                            F.IdentifierName("Get")))))
                    .WithSemicolonToken(F.Token(SyntaxKind.SemicolonToken))
                );
            }

            if (!Symbol.IsReadOnly)
            {
                mockedProperty = mockedProperty.AddAccessorListAccessors(F.AccessorDeclaration(SyntaxKind.SetAccessorDeclaration)
                    .WithExpressionBody(F.ArrowExpressionClause(F.InvocationExpression(
                            F.MemberAccessExpression(SyntaxKind.SimpleMemberAccessExpression,
                                F.IdentifierName(MockPropertyName), F.IdentifierName("Set")))
                        .WithExpressionsAsArgumentList(F.IdentifierName("value"))))
                    .WithSemicolonToken(F.Token(SyntaxKind.SemicolonToken)));
            }

            return mockedProperty;
        }
    }
}
