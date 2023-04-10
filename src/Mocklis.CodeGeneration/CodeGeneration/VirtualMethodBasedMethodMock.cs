// --------------------------------------------------------------------------------------------------------------------
// <copyright file="VirtualMethodBasedMethodMock.cs">
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
    using Mocklis.CodeGeneration.Compatibility;
    using Mocklis.CodeGeneration.UniqueNames;
    using F = Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

    #endregion

    public class VirtualMethodBasedMethodMock : VirtualMethodBasedMock<IMethodSymbol>, IMemberMock
    {
        public VirtualMethodBasedMethodMock(INamedTypeSymbol classSymbol, INamedTypeSymbol interfaceSymbol, IMethodSymbol symbol, string mockMemberName) : base(
            classSymbol, interfaceSymbol, symbol, mockMemberName)
        {
        }

        private static string? FindArglistParameterName(IMethodSymbol symbol)
        {
            if (symbol.IsVararg)
            {
                var uniquifier = new Uniquifier(symbol.Parameters.Select(a => a.Name));
                return uniquifier.GetUniqueName("arglist");
            }

            return null;
        }

        public void AddMembersToClass(IList<MemberDeclarationSyntax> declarationList, MocklisTypesForSymbols typesForSymbols, bool strict,
            bool veryStrict)
        {
            TypeSyntax returnTypeWithoutReadonly;
            TypeSyntax returnType;

            typesForSymbols = typesForSymbols.WithSubstitutions(ClassSymbol, Symbol);

            if (Symbol.ReturnsByRef)
            {
                RefTypeSyntax tmp = F.RefType(typesForSymbols.ParseTypeName(Symbol.ReturnType, Symbol.ReturnTypeIsNullableOrOblivious()));
                returnType = tmp;
                returnTypeWithoutReadonly = tmp;
            }
            else if (Symbol.ReturnsByRefReadonly)
            {
                RefTypeSyntax tmp = F.RefType(typesForSymbols.ParseTypeName(Symbol.ReturnType, Symbol.ReturnTypeIsNullableOrOblivious()));
                returnType = tmp.WithReadOnlyKeyword(F.Token(SyntaxKind.ReadOnlyKeyword));
                returnTypeWithoutReadonly = tmp;
            }
            else if (Symbol.ReturnsVoid)
            {
                returnType = F.PredefinedType(F.Token(SyntaxKind.VoidKeyword));
                returnTypeWithoutReadonly = returnType;
            }
            else
            {
                returnType = typesForSymbols.ParseTypeName(Symbol.ReturnType, Symbol.ReturnTypeIsNullableOrOblivious());
                returnTypeWithoutReadonly = returnType;
            }

            var arglistParameterName = FindArglistParameterName(Symbol);

            declarationList.Add(MockVirtualMethod(typesForSymbols, returnTypeWithoutReadonly, arglistParameterName));
            declarationList.Add(ExplicitInterfaceMember(typesForSymbols, returnType, arglistParameterName));
        }

        public void AddInitialisersToConstructor(List<StatementSyntax> constructorStatements, MocklisTypesForSymbols typesForSymbols, bool strict,
            bool veryStrict)
        {
        }

        private MemberDeclarationSyntax MockVirtualMethod(MocklisTypesForSymbols typesForSymbols, TypeSyntax returnTypeWithoutReadonly, string? arglistParameterName)
        {
            var parameters = F.SeparatedList(Symbol.Parameters.Select(ps => typesForSymbols.AsParameterSyntax(ps)));
            if (arglistParameterName != null && typesForSymbols.RuntimeArgumentHandle() != null)
            {
                parameters = parameters.Add(F.Parameter(F.Identifier(arglistParameterName)).WithType(typesForSymbols.RuntimeArgumentHandle()));
            }

            var method = F.MethodDeclaration(returnTypeWithoutReadonly, F.Identifier(MemberMockName))
                .WithModifiers(F.TokenList(F.Token(SyntaxKind.ProtectedKeyword), F.Token(SyntaxKind.VirtualKeyword)))
                .WithParameterList(F.ParameterList(parameters))
                .WithBody(F.Block(ThrowMockMissingStatement(typesForSymbols, "VirtualMethod")));

            if (Symbol.TypeParameters.Any())
            {
                method = method.WithTypeParameterList(TypeParameterList(typesForSymbols));

                var constraints = typesForSymbols.AsConstraintClauses(Symbol.TypeParameters);

                if (constraints.Any())
                {
                    method = method.AddConstraintClauses(constraints);
                }
            }

            return method;
        }

        private MemberDeclarationSyntax ExplicitInterfaceMember(MocklisTypesForSymbols typesForSymbols, TypeSyntax returnType, string? arglistParameterName)
        {
            var parameters = F.SeparatedList(Symbol.Parameters.Select(p => typesForSymbols.AsParameterSyntax(p)));
            if (arglistParameterName != null)
            {
                parameters = parameters.Add(F.Parameter(F.Token(SyntaxKind.ArgListKeyword)));
            }

            var arguments = Symbol.Parameters.AsArgumentList();

            if (arglistParameterName != null && typesForSymbols.RuntimeArgumentHandle() != null)
            {
                arguments = arguments.Add(F.Argument(F.LiteralExpression(SyntaxKind.ArgListExpression, F.Token(SyntaxKind.ArgListKeyword))));
            }

            var mockedMethod = F.MethodDeclaration(returnType, Symbol.Name)
                .WithParameterList(F.ParameterList(parameters))
                .WithExplicitInterfaceSpecifier(F.ExplicitInterfaceSpecifier(typesForSymbols.ParseName(InterfaceSymbol)));

            if (Symbol.TypeParameters.Any())
            {
                mockedMethod = mockedMethod.WithTypeParameterList(TypeParameterList(typesForSymbols));
            }

            var invocation = Symbol.TypeParameters.Any()
                ? (ExpressionSyntax)F.GenericName(MemberMockName)
                    .WithTypeArgumentList(F.TypeArgumentList(
                        F.SeparatedList(Symbol.TypeParameters.Select(typeParameter =>
                            typesForSymbols.ParseTypeName(typeParameter, false)))))
                : F.IdentifierName(MemberMockName);

            invocation = F.InvocationExpression(invocation, F.ArgumentList(arguments));

            if (Symbol.ReturnsByRef || Symbol.ReturnsByRefReadonly)
            {
                invocation = F.RefExpression(invocation);
            }

            mockedMethod = mockedMethod
                .WithExpressionBody(F.ArrowExpressionClause(invocation))
                .WithSemicolonToken(F.Token(SyntaxKind.SemicolonToken));

            return mockedMethod;
        }

        private TypeParameterListSyntax TypeParameterList(MocklisTypesForSymbols typesForSymbols)
        {
            return F.TypeParameterList(F.SeparatedList(Symbol.TypeParameters.Select(typeParameter =>
                F.TypeParameter(typesForSymbols.FindTypeParameterName(typeParameter.Name)))));
        }
    }
}
