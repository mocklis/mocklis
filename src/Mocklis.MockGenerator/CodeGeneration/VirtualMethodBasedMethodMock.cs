// --------------------------------------------------------------------------------------------------------------------
// <copyright file="VirtualMethodBasedMethodMock.cs">
//   SPDX-License-Identifier: MIT
//   Copyright © 2019-2023 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.MockGenerator.CodeGeneration;

#region Using Directives

using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using F = Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

#endregion

public sealed class VirtualMethodBasedMethodMock : IMemberMock
{
    public IMethodSymbol Symbol { get; }
    public string MemberMockName { get; }

    public VirtualMethodBasedMethodMock(IMethodSymbol symbol, string mockMemberName)
    {
        Symbol = symbol;
        MemberMockName = mockMemberName;
    }

    public void AddSyntax(MocklisTypesForSymbols typesForSymbols, IList<MemberDeclarationSyntax> declarationList,
        List<StatementSyntax> constructorStatements,
        NameSyntax interfaceNameSyntax, string className, string interfaceName)
    {
        var syntaxAdder = new SyntaxAdder(this, typesForSymbols);
        syntaxAdder.AddMembersToClass(declarationList, interfaceNameSyntax, className, interfaceName);
    }

    private class SyntaxAdder
    {
        private readonly VirtualMethodBasedMethodMock _mock;
        private readonly MocklisTypesForSymbols _typesForSymbols;
        private readonly ITypeParameterSubstitutions _substitutions;

        public SyntaxAdder(VirtualMethodBasedMethodMock mock, MocklisTypesForSymbols typesForSymbols)
        {
            _mock = mock;
            _typesForSymbols = typesForSymbols;
            _substitutions = typesForSymbols.BuildSubstitutions(mock.Symbol);
        }

        public void AddMembersToClass(IList<MemberDeclarationSyntax> declarationList, NameSyntax interfaceNameSyntax, string className,
            string interfaceName)
        {
            TypeSyntax returnTypeWithoutReadonly;
            TypeSyntax returnType;

            if (_mock.Symbol.ReturnsByRef)
            {
                RefTypeSyntax tmp =
                    F.RefType(_typesForSymbols.ParseTypeName(_mock.Symbol.ReturnType, _mock.Symbol.ReturnTypeIsNullableOrOblivious()));
                returnType = tmp;
                returnTypeWithoutReadonly = tmp;
            }
            else if (_mock.Symbol.ReturnsByRefReadonly)
            {
                RefTypeSyntax tmp =
                    F.RefType(_typesForSymbols.ParseTypeName(_mock.Symbol.ReturnType, _mock.Symbol.ReturnTypeIsNullableOrOblivious()));
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

            var arglistParameterName = _mock.Symbol.FindArglistParameterName();

            declarationList.Add(MockVirtualMethod(_typesForSymbols, returnTypeWithoutReadonly, arglistParameterName, className, interfaceName));
            declarationList.Add(ExplicitInterfaceMember(returnType, arglistParameterName, interfaceNameSyntax));
        }

        // TODO: Consider whether a 'default' implementation in lenient mode is to do nothing and return default values.
        private MemberDeclarationSyntax MockVirtualMethod(MocklisTypesForSymbols typesForSymbols, TypeSyntax returnTypeWithoutReadonly,
            string? arglistParameterName, string className, string interfaceName)
        {
            var parameters =
                F.SeparatedList(_mock.Symbol.Parameters.Select(a => typesForSymbols.AsParameterSyntax(a, _substitutions.FindSubstitution)));
            if (arglistParameterName != null)
            {
                parameters = parameters.Add(F.Parameter(F.Identifier(arglistParameterName)).WithType(typesForSymbols.RuntimeArgumentHandle()));
            }

            var method = F.MethodDeclaration(returnTypeWithoutReadonly, F.Identifier(_mock.MemberMockName))
                .WithModifiers(F.TokenList(F.Token(SyntaxKind.ProtectedKeyword), F.Token(SyntaxKind.VirtualKeyword)))
                .WithParameterList(F.ParameterList(parameters))
                .WithBody(F.Block(typesForSymbols.ThrowMockMissingStatement("VirtualMethod", _mock.MemberMockName, className, interfaceName,
                    _mock.Symbol.Name)));

            if (_mock.Symbol.TypeParameters.Any())
            {
                method = method.WithTypeParameterList(TypeParameterList());

                var constraints = typesForSymbols.AsConstraintClauses(_mock.Symbol.TypeParameters, _substitutions.FindSubstitution);

                if (constraints.Any())
                {
                    method = method.AddConstraintClauses(constraints);
                }
            }

            return method;
        }

        private MemberDeclarationSyntax ExplicitInterfaceMember(TypeSyntax returnType, string? arglistParameterName,
            NameSyntax interfaceNameSyntax)
        {
            var parameters =
                F.SeparatedList(_mock.Symbol.Parameters.Select(p => _typesForSymbols.AsParameterSyntax(p, _substitutions.FindSubstitution)));
            if (arglistParameterName != null)
            {
                parameters = parameters.Add(F.Parameter(F.Token(SyntaxKind.ArgListKeyword)));
            }

            var arguments = _mock.Symbol.Parameters.AsArgumentList();

            if (arglistParameterName != null)
            {
                arguments = arguments.Add(F.Argument(F.LiteralExpression(SyntaxKind.ArgListExpression, F.Token(SyntaxKind.ArgListKeyword))));
            }

            var mockedMethod = F.MethodDeclaration(returnType, _mock.Symbol.Name)
                .WithParameterList(F.ParameterList(parameters))
                .WithExplicitInterfaceSpecifier(F.ExplicitInterfaceSpecifier(interfaceNameSyntax));

            if (_mock.Symbol.TypeParameters.Any())
            {
                mockedMethod = mockedMethod.WithTypeParameterList(TypeParameterList());
            }

            var invocation = _mock.Symbol.TypeParameters.Any()
                ? (ExpressionSyntax)F.GenericName(_mock.MemberMockName)
                    .WithTypeArgumentList(F.TypeArgumentList(
                        F.SeparatedList(_mock.Symbol.TypeParameters.Select(typeParameter =>
                            _typesForSymbols.ParseTypeName(typeParameter, false, _substitutions.FindSubstitution)))))
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

        private TypeParameterListSyntax TypeParameterList()
        {
            return F.TypeParameterList(F.SeparatedList(_mock.Symbol.TypeParameters.Select(typeParameter =>
                F.TypeParameter(_substitutions.FindSubstitution(typeParameter.Name)))));
        }
    }
}
