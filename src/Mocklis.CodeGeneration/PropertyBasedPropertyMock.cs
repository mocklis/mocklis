// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PropertyBasedPropertyMock.cs">
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

    public class PropertyBasedPropertyMock : PropertyBasedMock<IPropertySymbol>, IMemberMock
    {
        private TypeSyntax ValueTypeSyntax { get; }
        private TypeSyntax MockPropertyType { get; }

        public PropertyBasedPropertyMock(MocklisTypesForSymbols typesForSymbols, INamedTypeSymbol classSymbol, INamedTypeSymbol interfaceSymbol,
            IPropertySymbol symbol,
            string mockMemberName) : base(typesForSymbols,
            classSymbol, interfaceSymbol, symbol, mockMemberName)
        {
            ValueTypeSyntax = typesForSymbols.ParseTypeName(symbol.Type);
            MockPropertyType = typesForSymbols.PropertyMock(ValueTypeSyntax);
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
            var mockedProperty = F.PropertyDeclaration(ValueTypeSyntax, Symbol.Name)
                .WithExplicitInterfaceSpecifier(F.ExplicitInterfaceSpecifier(TypesForSymbols.ParseName(InterfaceSymbol)));

            if (Symbol.IsReadOnly)
            {
                mockedProperty = mockedProperty
                    .WithExpressionBody(F.ArrowExpressionClause(F.MemberAccessExpression(SyntaxKind.SimpleMemberAccessExpression,
                        F.IdentifierName(MemberMockName), F.IdentifierName("Value")))).WithSemicolonToken(F.Token(SyntaxKind.SemicolonToken));
            }
            else
            {
                if (!Symbol.IsWriteOnly)
                {
                    mockedProperty = mockedProperty.AddAccessorListAccessors(F.AccessorDeclaration(SyntaxKind.GetAccessorDeclaration)
                        .WithExpressionBody(F.ArrowExpressionClause(F.MemberAccessExpression(SyntaxKind.SimpleMemberAccessExpression,
                            F.IdentifierName(MemberMockName),
                            F.IdentifierName("Value"))))
                        .WithSemicolonToken(F.Token(SyntaxKind.SemicolonToken))
                    );
                }

                if (!Symbol.IsReadOnly)
                {
                    mockedProperty = mockedProperty.AddAccessorListAccessors(F.AccessorDeclaration(SyntaxKind.SetAccessorDeclaration)
                        .WithExpressionBody(
                            F.ArrowExpressionClause(
                                F.AssignmentExpression(SyntaxKind.SimpleAssignmentExpression,
                                    F.MemberAccessExpression(SyntaxKind.SimpleMemberAccessExpression, F.IdentifierName(MemberMockName),
                                        F.IdentifierName("Value")),
                                    F.IdentifierName("value"))))
                        .WithSemicolonToken(F.Token(SyntaxKind.SemicolonToken))
                    );
                }
            }

            return mockedProperty;
        }
    }
}