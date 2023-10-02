// --------------------------------------------------------------------------------------------------------------------
// <copyright file="VirtualMethodBasedMethodMock.cs">
//   SPDX-License-Identifier: MIT
//   Copyright © 2019-2023 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.MockGenerator.CodeGeneration
{
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
        public ITypeParameterSubstitutions Substitutions { get; }

        public VirtualMethodBasedMethodMock(IMethodSymbol symbol, string mockMemberName, ITypeParameterSubstitutions substitutions)
        {
            Symbol = symbol;
            MemberMockName = mockMemberName;
            Substitutions = substitutions;
        }

        public ISyntaxAdder GetSyntaxAdder(MocklisTypesForSymbols typesForSymbols)
        {
            return new SyntaxAdder(this, typesForSymbols);
        }

        public void AddSource(SourceGenerationContext ctx, INamedTypeSymbol interfaceSymbol)
        {
            (string returnType, string returnTypeWithoutReadonly) = ctx.FindReturnTypes(Symbol, Substitutions);

            var arglistParameterName = Symbol.FindArglistParameterName();

            string typeParameterList = string.Empty;
            string constraints = string.Empty;

            if (Symbol.TypeParameters.Any())
            {
                typeParameterList = $"<{string.Join(", ", Symbol.TypeParameters.Select(t => Substitutions.FindSubstitution(t.Name)))}>";
                constraints = ctx.BuildConstraintClauses(Symbol.TypeParameters, Substitutions, false);
            }

            var parameters = ctx.BuildParameterList(Symbol.Parameters, Substitutions);
            var arguments = ctx.BuildArgumentList(Symbol.Parameters);
            if (arglistParameterName != null)
            {
                if (parameters != string.Empty)
                {
                    parameters += ", ";
                    arguments += ", ";
                }

                parameters += $"global::System.RuntimeArgumentHandle {arglistParameterName}";
                arguments += arglistParameterName;
            }

            ctx.AppendLine($"protected virtual {returnTypeWithoutReadonly} {MemberMockName}{typeParameterList}({parameters}){constraints}");
            ctx.AppendLine("{");
            ctx.IncreaseIndent();
            ctx.AppendThrow("VirtualMethod", MemberMockName, interfaceSymbol.Name, Symbol.Name);
            ctx.DecreaseIndent();
            ctx.AppendLine("}");
            ctx.AppendSeparator();

            var mockedMethod =
                $"{returnType} {ctx.ParseTypeName(interfaceSymbol, false, CodeGeneration.Substitutions.Empty)}.{Symbol.Name}{typeParameterList}({parameters})";

            ctx.Append($"{mockedMethod} => ");
            if (Symbol.ReturnsByRef || Symbol.ReturnsByRefReadonly)
            {
                ctx.Append("ref ");
            }

            ctx.AppendLine($"{MemberMockName}{typeParameterList}({arguments});");
        }

        private class SyntaxAdder : ISyntaxAdder
        {
            private readonly VirtualMethodBasedMethodMock _mock;
            private readonly MocklisTypesForSymbols _typesForSymbols;

            public SyntaxAdder(VirtualMethodBasedMethodMock mock, MocklisTypesForSymbols typesForSymbols)
            {
                _mock = mock;
                _typesForSymbols = typesForSymbols;
            }

            public void AddMembersToClass(MocklisTypesForSymbols typesForSymbols, MockSettings mockSettingns,
                IList<MemberDeclarationSyntax> declarationList, NameSyntax interfaceNameSyntax, string className,
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

            public void AddInitialisersToConstructor(MocklisTypesForSymbols typesForSymbols, MockSettings mockSettings,
                List<StatementSyntax> constructorStatements, string className, string interfaceName)
            {
            }

            // TODO: Consider whether a 'default' implementation in lenient mode is to do nothing and return default values.
            private MemberDeclarationSyntax MockVirtualMethod(MocklisTypesForSymbols typesForSymbols, TypeSyntax returnTypeWithoutReadonly,
                string? arglistParameterName, string className, string interfaceName)
            {
                var parameters =
                    F.SeparatedList(_mock.Symbol.Parameters.Select(a => typesForSymbols.AsParameterSyntax(a, _mock.Substitutions.FindSubstitution)));
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

                    var constraints = typesForSymbols.AsConstraintClauses(_mock.Symbol.TypeParameters, _mock.Substitutions.FindSubstitution);

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
                    F.SeparatedList(_mock.Symbol.Parameters.Select(p => _typesForSymbols.AsParameterSyntax(p, _mock.Substitutions.FindSubstitution)));
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
                                _typesForSymbols.ParseTypeName(typeParameter, false, _mock.Substitutions.FindSubstitution)))))
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
                    F.TypeParameter(_mock.Substitutions.FindSubstitution(typeParameter.Name)))));
            }
        }
    }
}
