// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PropertyBasedEventMock.cs">
//   Copyright © 2018 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.CodeGeneration
{
    #region Using Directives

    using System.Collections.Generic;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp;
    using Microsoft.CodeAnalysis.CSharp.Syntax;
    using F = Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

    #endregion

    public class PropertyBasedEventMock : PropertyBasedMock<IEventSymbol>, IMemberMock
    {
        private TypeSyntax EventHandlerTypeSyntax { get; }
        private TypeSyntax MockPropertyType { get; }

        public PropertyBasedEventMock(MocklisTypesForSymbols typesForSymbols, INamedTypeSymbol classSymbol, INamedTypeSymbol interfaceSymbol,
            IEventSymbol symbol,
            string memberMockName) : base(typesForSymbols, classSymbol, interfaceSymbol, symbol, memberMockName)
        {
            EventHandlerTypeSyntax = typesForSymbols.ParseTypeName(symbol.Type);
            MockPropertyType = typesForSymbols.EventMock(EventHandlerTypeSyntax);
        }

        public void AddMembersToClass(IList<MemberDeclarationSyntax> declarationList)
        {
            declarationList.Add(MockProperty(MockPropertyType));
            declarationList.Add(ExplicitInterfaceMember());
        }

        public void AddInitialisersToConstructor(List<StatementSyntax> constructorStatements)
        {
            constructorStatements.Add(InitialisationStatement(MockPropertyType));
        }

        private MemberDeclarationSyntax ExplicitInterfaceMember()
        {
            var mockedProperty = F.EventDeclaration(EventHandlerTypeSyntax, Symbol.Name)
                .WithExplicitInterfaceSpecifier(F.ExplicitInterfaceSpecifier(TypesForSymbols.ParseName(InterfaceSymbol)));

            mockedProperty = mockedProperty.AddAccessorListAccessors(F.AccessorDeclaration(SyntaxKind.AddAccessorDeclaration)
                .WithExpressionBody(F.ArrowExpressionClause(F.InvocationExpression(
                        F.MemberAccessExpression(SyntaxKind.SimpleMemberAccessExpression, F.IdentifierName(MemberMockName),
                            F.IdentifierName("Add")))
                    .WithExpressionsAsArgumentList(F.IdentifierName("value"))))
                .WithSemicolonToken(F.Token(SyntaxKind.SemicolonToken))
            );

            mockedProperty = mockedProperty.AddAccessorListAccessors(F.AccessorDeclaration(SyntaxKind.RemoveAccessorDeclaration)
                .WithExpressionBody(F.ArrowExpressionClause(F.InvocationExpression(
                        F.MemberAccessExpression(SyntaxKind.SimpleMemberAccessExpression,
                            F.IdentifierName(MemberMockName), F.IdentifierName("Remove")))
                    .WithExpressionsAsArgumentList(F.IdentifierName("value"))))
                .WithSemicolonToken(F.Token(SyntaxKind.SemicolonToken)));

            return mockedProperty;
        }
    }
}
