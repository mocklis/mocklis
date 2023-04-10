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

        public PropertyBasedMethodMockWithTypeParameters(MocklisTypesForSymbols typesForSymbols, INamedTypeSymbol classSymbol,
            INamedTypeSymbol interfaceSymbol, IMethodSymbol symbol, string mockMemberName, string mockProviderName)
            : base(typesForSymbols.WithSubstitutions(classSymbol, symbol), classSymbol, interfaceSymbol, symbol, mockMemberName)
        {
            MockProviderName = mockProviderName;
        }

        public override void AddMembersToClass(IList<MemberDeclarationSyntax> declarationList, MocklisTypesForSymbols typesForSymbols, bool strict,
            bool veryStrict)
        {
            declarationList.Add(TypedMockProviderField(typesForSymbols));
            declarationList.Add(MockProviderMethod(typesForSymbols, strict, veryStrict));
            declarationList.Add(ExplicitInterfaceMember(typesForSymbols));
        }

        private MemberDeclarationSyntax TypedMockProviderField(MocklisTypesForSymbols typesForSymbols)
        {
            return F.FieldDeclaration(F.VariableDeclaration(typesForSymbols.TypedMockProvider()).WithVariables(
                F.SingletonSeparatedList(F.VariableDeclarator(F.Identifier(MockProviderName))
                    .WithInitializer(F.EqualsValueClause(F.ObjectCreationExpression(typesForSymbols.TypedMockProvider())
                        .WithArgumentList(F.ArgumentList())))))
            ).WithModifiers(F.TokenList(F.Token(SyntaxKind.PrivateKeyword), F.Token(SyntaxKind.ReadOnlyKeyword)));
        }

        private MemberDeclarationSyntax MockProviderMethod(MocklisTypesForSymbols typesForSymbols, bool strict, bool veryStrict)
        {
            var m = F.MethodDeclaration(MockMemberType, F.Identifier(MemberMockName)).WithTypeParameterList(TypeParameterList(typesForSymbols));

            m = m.WithModifiers(F.TokenList(F.Token(SyntaxKind.PublicKeyword)));

            var keyCreation = F.LocalDeclarationStatement(F.VariableDeclaration(F.IdentifierName("var")).WithVariables(F.SingletonSeparatedList(F
                .VariableDeclarator(F.Identifier("key")).WithInitializer(F.EqualsValueClause(TypesOfTypeParameters(typesForSymbols))))));

            var mockCreation = F.SimpleLambdaExpression(F.Parameter(F.Identifier("keyString")), F.ObjectCreationExpression(MockMemberType)
                .WithExpressionsAsArgumentList(
                    F.ThisExpression(),
                    F.LiteralExpression(SyntaxKind.StringLiteralExpression, F.Literal(ClassSymbol.Name)),
                    F.LiteralExpression(SyntaxKind.StringLiteralExpression, F.Literal(InterfaceSymbol.Name)),
                    F.BinaryExpression(SyntaxKind.AddExpression, F.LiteralExpression(SyntaxKind.StringLiteralExpression, F.Literal(Symbol.Name)),
                        F.IdentifierName("keyString")),
                    F.BinaryExpression(SyntaxKind.AddExpression,
                        F.BinaryExpression(SyntaxKind.AddExpression,
                            F.LiteralExpression(SyntaxKind.StringLiteralExpression, F.Literal(MemberMockName)), F.IdentifierName("keyString")),
                        F.LiteralExpression(
                            SyntaxKind.StringLiteralExpression,
                            F.Literal("()"))),
                    typesForSymbols.StrictnessExpression(strict, veryStrict)
                ));

            var returnStatement = F.ReturnStatement(F.CastExpression(MockMemberType, F.InvocationExpression(
                    F.MemberAccessExpression(SyntaxKind.SimpleMemberAccessExpression, F.IdentifierName(MockProviderName),
                        F.IdentifierName("GetOrAdd")))
                .WithArgumentList(
                    F.ArgumentList(F.SeparatedList(new[] { F.Argument(F.IdentifierName("key")), F.Argument(mockCreation) })))));

            m = m.WithBody(F.Block(keyCreation, returnStatement));

            var constraints = typesForSymbols.AsConstraintClauses(Symbol.TypeParameters);

            if (constraints.Any())
            {
                m = m.AddConstraintClauses(constraints);
            }

            return m;
        }

        private ImplicitArrayCreationExpressionSyntax TypesOfTypeParameters(MocklisTypesForSymbols typesForSymbols)
        {
            return F.ImplicitArrayCreationExpression(F.InitializerExpression(SyntaxKind.ArrayInitializerExpression,
                F.SeparatedList<ExpressionSyntax>(Symbol.TypeParameters.Select(typeParameter =>
                    F.TypeOfExpression(F.IdentifierName(typesForSymbols.FindTypeParameterName(typeParameter.Name)))))));
        }

        public override void AddInitialisersToConstructor(List<StatementSyntax> constructorStatements, MocklisTypesForSymbols typesForSymbols,
            bool strict, bool veryStrict)
        {
        }

        protected override MethodDeclarationSyntax ExplicitInterfaceMemberMethodDeclaration(TypeSyntax returnType,
            MocklisTypesForSymbols typesForSymbols)
        {
            var m = base.ExplicitInterfaceMemberMethodDeclaration(returnType, typesForSymbols).WithTypeParameterList(TypeParameterList(typesForSymbols));

            var constraints = typesForSymbols.AsClassConstraintClausesForReferenceTypes(Symbol.TypeParameters);
            if (constraints.Any())
            {
                m = m.AddConstraintClauses(constraints);
            }

            return m;
        }

        protected override ExpressionSyntax ExplicitInterfaceMemberMemberMockInstance(MocklisTypesForSymbols typesForSymbols)
        {
            return F.InvocationExpression(F.GenericName(MemberMockName).WithTypeArgumentList(TypeArgumentList(typesForSymbols))).WithArgumentList(F.ArgumentList());
        }

        private TypeParameterListSyntax TypeParameterList(MocklisTypesForSymbols typesForSymbols)
        {
            return F.TypeParameterList(F.SeparatedList(Symbol.TypeParameters.Select(typeParameter =>
                F.TypeParameter(typesForSymbols.FindTypeParameterName(typeParameter.Name)))));
        }

        private TypeArgumentListSyntax TypeArgumentList(MocklisTypesForSymbols typesForSymbols)
        {
            return F.TypeArgumentList(
                F.SeparatedList<TypeSyntax>(Symbol.TypeParameters.Select(typeParameter =>
                    F.IdentifierName(typesForSymbols.FindTypeParameterName(typeParameter.Name)))));
        }
    }
}
