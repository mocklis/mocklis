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
    using Mocklis.MockGenerator.CodeGeneration;
    using F = Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

    #endregion

    public class PropertyBasedMethodMockWithTypeParameters : PropertyBasedMethodMock
    {
        public string MockProviderName { get; }

        public PropertyBasedMethodMockWithTypeParameters(IMethodSymbol symbol, string mockMemberName, ITypeParameterSubstitutions substitutions, string mockProviderName)
            : base(symbol, mockMemberName, substitutions)
        {
            MockProviderName = mockProviderName;
        }

        protected override ISyntaxAdder CreateSyntaxAdder(MocklisTypesForSymbols typesForSymbols)
        {
            return new SyntaxAdder(this, typesForSymbols);
        }

        private class SyntaxAdder : SyntaxAdder<PropertyBasedMethodMockWithTypeParameters>
        {
            public SyntaxAdder(PropertyBasedMethodMockWithTypeParameters mock, MocklisTypesForSymbols typesForSymbols) :
                base(mock, typesForSymbols)
            {
            }

            public override void AddMembersToClass(MocklisTypesForSymbols typesForSymbols, MockSettings mockSettings,
                IList<MemberDeclarationSyntax> declarationList, NameSyntax interfaceNameSyntax, string className, string interfaceName)
            {
                declarationList.Add(TypedMockProviderField(typesForSymbols));
                declarationList.Add(MockProviderMethod(typesForSymbols, className, interfaceName, mockSettings));
                declarationList.Add(ExplicitInterfaceMember(typesForSymbols, interfaceNameSyntax));
            }

            private MemberDeclarationSyntax TypedMockProviderField(MocklisTypesForSymbols typesForSymbols)
            {
                return F.FieldDeclaration(F.VariableDeclaration(typesForSymbols.TypedMockProvider()).WithVariables(
                    F.SingletonSeparatedList(F.VariableDeclarator(F.Identifier(Mock.MockProviderName))
                        .WithInitializer(F.EqualsValueClause(F.ObjectCreationExpression(typesForSymbols.TypedMockProvider())
                            .WithArgumentList(F.ArgumentList())))))
                ).WithModifiers(F.TokenList(F.Token(SyntaxKind.PrivateKeyword), F.Token(SyntaxKind.ReadOnlyKeyword)));
            }

            private MemberDeclarationSyntax MockProviderMethod(MocklisTypesForSymbols typesForSymbols, string className, string interfaceName, MockSettings mockSettings)
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
                        typesForSymbols.StrictnessExpression(mockSettings.Strict, mockSettings.VeryStrict)
                    ));

                var returnStatement = F.ReturnStatement(F.CastExpression(MockMemberType, F.InvocationExpression(
                        F.MemberAccessExpression(SyntaxKind.SimpleMemberAccessExpression, F.IdentifierName(Mock.MockProviderName),
                            F.IdentifierName("GetOrAdd")))
                    .WithArgumentList(
                        F.ArgumentList(F.SeparatedList(new[] { F.Argument(F.IdentifierName("key")), F.Argument(mockCreation) })))));

                m = m.WithBody(F.Block(keyCreation, returnStatement));

                var constraints = typesForSymbols.AsConstraintClauses(Mock.Symbol.TypeParameters, Mock.Substitutions.FindSubstitution);

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
                        F.TypeOfExpression(F.IdentifierName(Mock.Substitutions.FindSubstitution(typeParameter.Name)))))));
            }

            public override void AddInitialisersToConstructor(MocklisTypesForSymbols typesForSymbols, MockSettings mockSettings,
                List<StatementSyntax> constructorStatements, string className, string interfaceName)
            {
            }

            protected override MethodDeclarationSyntax ExplicitInterfaceMemberMethodDeclaration(MocklisTypesForSymbols typesForSymbols, TypeSyntax returnType, NameSyntax interfaceNameSyntax)
            {
                var m = base.ExplicitInterfaceMemberMethodDeclaration(typesForSymbols, returnType, interfaceNameSyntax)
                    .WithTypeParameterList(TypeParameterList());

                var constraints = typesForSymbols.AsClassConstraintClausesForReferenceTypes(Mock.Symbol.TypeParameters, Mock.Substitutions.FindSubstitution);
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
                    F.TypeParameter(Mock.Substitutions.FindSubstitution(typeParameter.Name)))));
            }

            private TypeArgumentListSyntax TypeArgumentList()
            {
                return F.TypeArgumentList(
                    F.SeparatedList<TypeSyntax>(Mock.Symbol.TypeParameters.Select(typeParameter =>
                        F.IdentifierName(Mock.Substitutions.FindSubstitution(typeParameter.Name)))));
            }
        }
    }
}
