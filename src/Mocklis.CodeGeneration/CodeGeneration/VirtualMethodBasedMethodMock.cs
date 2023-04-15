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

        public ISyntaxAdder GetSyntaxAdder(MocklisTypesForSymbols typesForSymbols, bool strict, bool veryStrict)
        {
            return new SyntaxAdder(this, typesForSymbols);
        }

        private class SyntaxAdder : ISyntaxAdder
        {
            private readonly VirtualMethodBasedMethodMock _mock;
            private readonly MocklisTypesForSymbols _typesForSymbols;

            public SyntaxAdder(VirtualMethodBasedMethodMock mock, MocklisTypesForSymbols typesForSymbols)
            {
                _mock = mock;
                _typesForSymbols = typesForSymbols.WithSubstitutions(_mock.ClassSymbol, _mock.Symbol);
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

            public void AddMembersToClass(IList<MemberDeclarationSyntax> declarationList)
            {
                TypeSyntax returnTypeWithoutReadonly;
                TypeSyntax returnType;

                if (_mock.Symbol.ReturnsByRef)
                {
                    RefTypeSyntax tmp = F.RefType(_typesForSymbols.ParseTypeName(_mock.Symbol.ReturnType, _mock.Symbol.ReturnTypeIsNullableOrOblivious()));
                    returnType = tmp;
                    returnTypeWithoutReadonly = tmp;
                }
                else if (_mock.Symbol.ReturnsByRefReadonly)
                {
                    RefTypeSyntax tmp = F.RefType(_typesForSymbols.ParseTypeName(_mock.Symbol.ReturnType, _mock.Symbol.ReturnTypeIsNullableOrOblivious()));
                    returnType = tmp.WithReadOnlyKeyword(F.Token(SyntaxKind.ReadOnlyKeyword));
                    returnTypeWithoutReadonly = tmp;
                }
                else if (_mock.Symbol.ReturnsVoid)
                {
                    returnType = F.PredefinedType(F.Token(SyntaxKind.VoidKeyword));
                    returnTypeWithoutReadonly = returnType;
                }
                else
                {
                    returnType = _typesForSymbols.ParseTypeName(_mock.Symbol.ReturnType, _mock.Symbol.ReturnTypeIsNullableOrOblivious());
                    returnTypeWithoutReadonly = returnType;
                }

                var arglistParameterName = FindArglistParameterName(_mock.Symbol);

                declarationList.Add(MockVirtualMethod(_typesForSymbols, returnTypeWithoutReadonly, arglistParameterName));
                declarationList.Add(ExplicitInterfaceMember(returnType, arglistParameterName));
            }

            public void AddInitialisersToConstructor(List<StatementSyntax> constructorStatements)
            {
            }

            private MemberDeclarationSyntax MockVirtualMethod(MocklisTypesForSymbols typesForSymbols, TypeSyntax returnTypeWithoutReadonly,
                string? arglistParameterName)
            {
                var parameters = F.SeparatedList(_mock.Symbol.Parameters.Select(typesForSymbols.AsParameterSyntax));
                if (arglistParameterName != null && typesForSymbols.RuntimeArgumentHandle() != null)
                {
                    parameters = parameters.Add(F.Parameter(F.Identifier(arglistParameterName)).WithType(typesForSymbols.RuntimeArgumentHandle()));
                }

                var method = F.MethodDeclaration(returnTypeWithoutReadonly, F.Identifier(_mock.MemberMockName))
                    .WithModifiers(F.TokenList(F.Token(SyntaxKind.ProtectedKeyword), F.Token(SyntaxKind.VirtualKeyword)))
                    .WithParameterList(F.ParameterList(parameters))
                    .WithBody(F.Block(_mock.ThrowMockMissingStatement(typesForSymbols, "VirtualMethod")));

                if (_mock.Symbol.TypeParameters.Any())
                {
                    method = method.WithTypeParameterList(TypeParameterList(typesForSymbols));

                    var constraints = typesForSymbols.AsConstraintClauses(_mock.Symbol.TypeParameters);

                    if (constraints.Any())
                    {
                        method = method.AddConstraintClauses(constraints);
                    }
                }

                return method;
            }

            private MemberDeclarationSyntax ExplicitInterfaceMember(TypeSyntax returnType, string? arglistParameterName)
            {
                var parameters = F.SeparatedList(_mock.Symbol.Parameters.Select(p => _typesForSymbols.AsParameterSyntax(p)));
                if (arglistParameterName != null)
                {
                    parameters = parameters.Add(F.Parameter(F.Token(SyntaxKind.ArgListKeyword)));
                }

                var arguments = _mock.Symbol.Parameters.AsArgumentList();

                if (arglistParameterName != null && _typesForSymbols.RuntimeArgumentHandle() != null)
                {
                    arguments = arguments.Add(F.Argument(F.LiteralExpression(SyntaxKind.ArgListExpression, F.Token(SyntaxKind.ArgListKeyword))));
                }

                var mockedMethod = F.MethodDeclaration(returnType, _mock.Symbol.Name)
                    .WithParameterList(F.ParameterList(parameters))
                    .WithExplicitInterfaceSpecifier(F.ExplicitInterfaceSpecifier(_typesForSymbols.ParseName(_mock.InterfaceSymbol)));

                if (_mock.Symbol.TypeParameters.Any())
                {
                    mockedMethod = mockedMethod.WithTypeParameterList(TypeParameterList(_typesForSymbols));
                }

                var invocation = _mock.Symbol.TypeParameters.Any()
                    ? (ExpressionSyntax)F.GenericName(_mock.MemberMockName)
                        .WithTypeArgumentList(F.TypeArgumentList(
                            F.SeparatedList(_mock.Symbol.TypeParameters.Select(typeParameter =>
                                _typesForSymbols.ParseTypeName(typeParameter, false)))))
                    : F.IdentifierName(_mock.MemberMockName);

                invocation = F.InvocationExpression(invocation, F.ArgumentList(arguments));

                if (_mock.Symbol.ReturnsByRef || _mock.Symbol.ReturnsByRefReadonly)
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
                return F.TypeParameterList(F.SeparatedList(_mock.Symbol.TypeParameters.Select(typeParameter =>
                    F.TypeParameter(typesForSymbols.FindTypeParameterName(typeParameter.Name)))));
            }
        }
    }
}
