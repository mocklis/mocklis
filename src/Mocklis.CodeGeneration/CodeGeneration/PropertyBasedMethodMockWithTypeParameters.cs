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

        public PropertyBasedMethodMockWithTypeParameters(INamedTypeSymbol classSymbol, INamedTypeSymbol interfaceSymbol, IMethodSymbol symbol, string mockMemberName, string mockProviderName)
            : base(classSymbol, interfaceSymbol, symbol, mockMemberName)
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

            public override void AddMembersToClass(IList<MemberDeclarationSyntax> declarationList)
            {
                declarationList.Add(TypedMockProviderField());
                declarationList.Add(MockProviderMethod());
                declarationList.Add(ExplicitInterfaceMember());
            }

            private MemberDeclarationSyntax TypedMockProviderField()
            {
                return F.FieldDeclaration(F.VariableDeclaration(_typesForSymbols.TypedMockProvider()).WithVariables(
                    F.SingletonSeparatedList(F.VariableDeclarator(F.Identifier(_mock.MockProviderName))
                        .WithInitializer(F.EqualsValueClause(F.ObjectCreationExpression(_typesForSymbols.TypedMockProvider())
                            .WithArgumentList(F.ArgumentList())))))
                ).WithModifiers(F.TokenList(F.Token(SyntaxKind.PrivateKeyword), F.Token(SyntaxKind.ReadOnlyKeyword)));
            }

            private MemberDeclarationSyntax MockProviderMethod()
            {
                var m = F.MethodDeclaration(MockMemberType, F.Identifier(_mock.MemberMockName)).WithTypeParameterList(TypeParameterList(_typesForSymbols));

                m = m.WithModifiers(F.TokenList(F.Token(SyntaxKind.PublicKeyword)));

                var keyCreation = F.LocalDeclarationStatement(F.VariableDeclaration(F.IdentifierName("var")).WithVariables(F.SingletonSeparatedList(F
                    .VariableDeclarator(F.Identifier("key")).WithInitializer(F.EqualsValueClause(TypesOfTypeParameters(_typesForSymbols))))));

                var mockCreation = F.SimpleLambdaExpression(F.Parameter(F.Identifier("keyString")), F.ObjectCreationExpression(MockMemberType)
                    .WithExpressionsAsArgumentList(
                        F.ThisExpression(),
                        F.LiteralExpression(SyntaxKind.StringLiteralExpression, F.Literal(_mock.ClassSymbol.Name)),
                        F.LiteralExpression(SyntaxKind.StringLiteralExpression, F.Literal(_mock.InterfaceSymbol.Name)),
                        F.BinaryExpression(SyntaxKind.AddExpression, F.LiteralExpression(SyntaxKind.StringLiteralExpression, F.Literal(_mock.Symbol.Name)),
                            F.IdentifierName("keyString")),
                        F.BinaryExpression(SyntaxKind.AddExpression,
                            F.BinaryExpression(SyntaxKind.AddExpression,
                                F.LiteralExpression(SyntaxKind.StringLiteralExpression, F.Literal(_mock.MemberMockName)), F.IdentifierName("keyString")),
                            F.LiteralExpression(
                                SyntaxKind.StringLiteralExpression,
                                F.Literal("()"))),
                        _typesForSymbols.StrictnessExpression(_strict, _veryStrict)
                    ));

                var returnStatement = F.ReturnStatement(F.CastExpression(MockMemberType, F.InvocationExpression(
                        F.MemberAccessExpression(SyntaxKind.SimpleMemberAccessExpression, F.IdentifierName(_mock.MockProviderName),
                            F.IdentifierName("GetOrAdd")))
                    .WithArgumentList(
                        F.ArgumentList(F.SeparatedList(new[] { F.Argument(F.IdentifierName("key")), F.Argument(mockCreation) })))));

                m = m.WithBody(F.Block(keyCreation, returnStatement));

                var constraints = _typesForSymbols.AsConstraintClauses(_mock.Symbol.TypeParameters, Substitutions.FindTypeParameterName);

                if (constraints.Any())
                {
                    m = m.AddConstraintClauses(constraints);
                }

                return m;
            }

            private ImplicitArrayCreationExpressionSyntax TypesOfTypeParameters(MocklisTypesForSymbols typesForSymbols)
            {
                return F.ImplicitArrayCreationExpression(F.InitializerExpression(SyntaxKind.ArrayInitializerExpression,
                    F.SeparatedList<ExpressionSyntax>(_mock.Symbol.TypeParameters.Select(typeParameter =>
                        F.TypeOfExpression(F.IdentifierName(Substitutions.FindTypeParameterName(typeParameter.Name)))))));
            }

            public override void AddInitialisersToConstructor(List<StatementSyntax> constructorStatements)
            {
            }

            protected override MethodDeclarationSyntax ExplicitInterfaceMemberMethodDeclaration(TypeSyntax returnType)
            {
                var m = base.ExplicitInterfaceMemberMethodDeclaration(returnType)
                    .WithTypeParameterList(TypeParameterList(_typesForSymbols));

                var constraints = _typesForSymbols.AsClassConstraintClausesForReferenceTypes(_mock.Symbol.TypeParameters, Substitutions.FindTypeParameterName);
                if (constraints.Any())
                {
                    m = m.AddConstraintClauses(constraints);
                }

                return m;
            }

            protected override ExpressionSyntax ExplicitInterfaceMemberMemberMockInstance()
            {
                return F.InvocationExpression(F.GenericName(_mock.MemberMockName).WithTypeArgumentList(TypeArgumentList(_typesForSymbols)))
                    .WithArgumentList(F.ArgumentList());
            }

            private TypeParameterListSyntax TypeParameterList(MocklisTypesForSymbols typesForSymbols)
            {
                return F.TypeParameterList(F.SeparatedList(_mock.Symbol.TypeParameters.Select(typeParameter =>
                    F.TypeParameter(Substitutions.FindTypeParameterName(typeParameter.Name)))));
            }

            private TypeArgumentListSyntax TypeArgumentList(MocklisTypesForSymbols typesForSymbols)
            {
                return F.TypeArgumentList(
                    F.SeparatedList<TypeSyntax>(_mock.Symbol.TypeParameters.Select(typeParameter =>
                        F.IdentifierName(Substitutions.FindTypeParameterName(typeParameter.Name)))));
            }
        }
    }
}
