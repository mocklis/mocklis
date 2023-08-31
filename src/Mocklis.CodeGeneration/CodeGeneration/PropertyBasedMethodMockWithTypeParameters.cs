// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PropertyBasedMethodMockWithTypeParameters.cs">
//   SPDX-License-Identifier: MIT
//   Copyright © 2019-2021 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.CodeGeneration
{
    #region Using Directives

    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp;
    using Microsoft.CodeAnalysis.CSharp.Syntax;
    using F = Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

    #endregion

    public class PropertyBasedMethodMockWithTypeParameters : PropertyBasedMethodMock
    {
        public string MockProviderName { get; }

        public PropertyBasedMethodMockWithTypeParameters(INamedTypeSymbol interfaceSymbol, IMethodSymbol symbol, string mockMemberName, Substitutions substitutions, string mockProviderName)
            : base(interfaceSymbol, symbol, mockMemberName, substitutions)
        {
            MockProviderName = mockProviderName;
        }

        protected override ISyntaxAdder CreateSyntaxAdder(MocklisTypesForSymbols typesForSymbols, bool strict, bool veryStrict)
        {
            return new SyntaxAdder(this, typesForSymbols, strict, veryStrict);
        }

        private class SyntaxAdder : SyntaxAdder<PropertyBasedMethodMockWithTypeParameters>
        {
            public SyntaxAdder(PropertyBasedMethodMockWithTypeParameters mock, MocklisTypesForSymbols typesForSymbols, bool strict, bool veryStrict) :
                base(mock, typesForSymbols, strict, veryStrict)
            {
            }

            public override void AddMembersToClass(IList<MemberDeclarationSyntax> declarationList, NameSyntax interfaceNameSyntax, string className, string interfaceName)
            {
                declarationList.Add(TypedMockProviderField());
                declarationList.Add(MockProviderMethod(className, interfaceName));
                declarationList.Add(ExplicitInterfaceMember(interfaceNameSyntax));
            }

            private MemberDeclarationSyntax TypedMockProviderField()
            {
                return F.FieldDeclaration(F.VariableDeclaration(TypesForSymbols.TypedMockProvider()).WithVariables(
                    F.SingletonSeparatedList(F.VariableDeclarator(F.Identifier(Mock.MockProviderName))
                        .WithInitializer(F.EqualsValueClause(F.ObjectCreationExpression(TypesForSymbols.TypedMockProvider())
                            .WithArgumentList(F.ArgumentList())))))
                ).WithModifiers(F.TokenList(F.Token(SyntaxKind.PrivateKeyword), F.Token(SyntaxKind.ReadOnlyKeyword)));
            }

            private MemberDeclarationSyntax MockProviderMethod(string className, string interfaceName)
            {
                var m = F.MethodDeclaration(MockMemberType, F.Identifier(Mock.MemberMockName)).WithTypeParameterList(TypeParameterList());

                m = m.WithModifiers(F.TokenList(F.Token(SyntaxKind.PublicKeyword)));

                var keyCreation = F.LocalDeclarationStatement(F.VariableDeclaration(F.IdentifierName("var")).WithVariables(F.SingletonSeparatedList(F
                    .VariableDeclarator(F.Identifier("key")).WithInitializer(F.EqualsValueClause(TypesOfTypeParameters())))));

                var mockCreation = F.SimpleLambdaExpression(F.Parameter(F.Identifier("keyString")), F.ObjectCreationExpression(MockMemberType)
                    .WithExpressionsAsArgumentList(
                        F.ThisExpression(),
                        F.LiteralExpression(SyntaxKind.StringLiteralExpression, F.Literal(className)),
                        F.LiteralExpression(SyntaxKind.StringLiteralExpression, F.Literal(interfaceName)),
                        F.BinaryExpression(SyntaxKind.AddExpression, F.LiteralExpression(SyntaxKind.StringLiteralExpression, F.Literal(Mock.Symbol.Name)),
                            F.IdentifierName("keyString")),
                        F.BinaryExpression(SyntaxKind.AddExpression,
                            F.BinaryExpression(SyntaxKind.AddExpression,
                                F.LiteralExpression(SyntaxKind.StringLiteralExpression, F.Literal(Mock.MemberMockName)), F.IdentifierName("keyString")),
                            F.LiteralExpression(
                                SyntaxKind.StringLiteralExpression,
                                F.Literal("()"))),
                        TypesForSymbols.StrictnessExpression(Strict, VeryStrict)
                    ));

                var returnStatement = F.ReturnStatement(F.CastExpression(MockMemberType, F.InvocationExpression(
                        F.MemberAccessExpression(SyntaxKind.SimpleMemberAccessExpression, F.IdentifierName(Mock.MockProviderName),
                            F.IdentifierName("GetOrAdd")))
                    .WithArgumentList(
                        F.ArgumentList(F.SeparatedList(new[] { F.Argument(F.IdentifierName("key")), F.Argument(mockCreation) })))));

                m = m.WithBody(F.Block(keyCreation, returnStatement));

                var constraints = TypesForSymbols.AsConstraintClauses(Mock.Symbol.TypeParameters, Mock.Substitutions.FindTypeParameterName);

                if (constraints.Any())
                {
                    m = m.AddConstraintClauses(constraints);
                }

                return m;
            }

            private ImplicitArrayCreationExpressionSyntax TypesOfTypeParameters()
            {
                return F.ImplicitArrayCreationExpression(F.InitializerExpression(SyntaxKind.ArrayInitializerExpression,
                    F.SeparatedList<ExpressionSyntax>(Mock.Symbol.TypeParameters.Select(typeParameter =>
                        F.TypeOfExpression(F.IdentifierName(Mock.Substitutions.FindTypeParameterName(typeParameter.Name)))))));
            }

            public override void AddInitialisersToConstructor(List<StatementSyntax> constructorStatements, string className, string interfaceName)
            {
            }

            protected override MethodDeclarationSyntax ExplicitInterfaceMemberMethodDeclaration(TypeSyntax returnType, NameSyntax interfaceNameSyntax)
            {
                var m = base.ExplicitInterfaceMemberMethodDeclaration(returnType, interfaceNameSyntax)
                    .WithTypeParameterList(TypeParameterList());

                var constraints = TypesForSymbols.AsClassConstraintClausesForReferenceTypes(Mock.Symbol.TypeParameters, Mock.Substitutions.FindTypeParameterName);
                if (constraints.Any())
                {
                    m = m.AddConstraintClauses(constraints);
                }

                return m;
            }

            protected override ExpressionSyntax ExplicitInterfaceMemberMemberMockInstance()
            {
                return F.InvocationExpression(F.GenericName(Mock.MemberMockName).WithTypeArgumentList(TypeArgumentList()))
                    .WithArgumentList(F.ArgumentList());
            }

            private TypeParameterListSyntax TypeParameterList()
            {
                return F.TypeParameterList(F.SeparatedList(Mock.Symbol.TypeParameters.Select(typeParameter =>
                    F.TypeParameter(Mock.Substitutions.FindTypeParameterName(typeParameter.Name)))));
            }

            private TypeArgumentListSyntax TypeArgumentList()
            {
                return F.TypeArgumentList(
                    F.SeparatedList<TypeSyntax>(Mock.Symbol.TypeParameters.Select(typeParameter =>
                        F.IdentifierName(Mock.Substitutions.FindTypeParameterName(typeParameter.Name)))));
            }
        }
    }
}
