// --------------------------------------------------------------------------------------------------------------------
// <copyright file="VirtualMethodBasedMethodMock.cs">
//   Copyright © 2018 Esbjörn Redmo and contributors. All rights reserved.
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
    using Mocklis.CodeGeneration.UniqueNames;
    using F = Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

    #endregion

    public class VirtualMethodBasedMethodMock : VirtualMethodBasedMock<IMethodSymbol>, IMemberMock
    {
        private TypeSyntax ReturnTypeWithoutReadonly { get; }
        private TypeSyntax ReturnType { get; }
        private string ArglistParameterName { get; }

        public VirtualMethodBasedMethodMock(MocklisTypesForSymbols typesForSymbols, INamedTypeSymbol classSymbol, INamedTypeSymbol interfaceSymbol,
            IMethodSymbol symbol,
            string mockMemberName) : base(typesForSymbols, classSymbol, interfaceSymbol, symbol, mockMemberName)
        {
            if (symbol.ReturnsByRef)
            {
                RefTypeSyntax tmp = F.RefType(typesForSymbols.ParseTypeName(symbol.ReturnType));
                ReturnType = tmp;
                ReturnTypeWithoutReadonly = tmp;
            }
            else if (symbol.ReturnsByRefReadonly)
            {
                RefTypeSyntax tmp = F.RefType(typesForSymbols.ParseTypeName(symbol.ReturnType));
                ReturnType = tmp.WithReadOnlyKeyword(F.Token(SyntaxKind.ReadOnlyKeyword));
                ReturnTypeWithoutReadonly = tmp;
            }
            else if (symbol.ReturnsVoid)
            {
                ReturnType = F.PredefinedType(F.Token(SyntaxKind.VoidKeyword));
                ReturnTypeWithoutReadonly = ReturnType;
            }
            else
            {
                ReturnType = typesForSymbols.ParseTypeName(symbol.ReturnType);
                ReturnTypeWithoutReadonly = ReturnType;
            }

            ArglistParameterName = FindArglistParameterName(symbol);
        }

        private static string FindArglistParameterName(IMethodSymbol symbol)
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
            declarationList.Add(MockVirtualMethod());
            declarationList.Add(ExplicitInterfaceMember());
        }

        public void AddInitialisersToConstructor(List<StatementSyntax> constructorStatements)
        {
        }

        private MemberDeclarationSyntax MockVirtualMethod()
        {
            var parameters = F.SeparatedList(Symbol.Parameters.Select(TypesForSymbols.AsParameterSyntax));
            if (ArglistParameterName != null && TypesForSymbols.RuntimeArgumentHandle != null)
            {
                parameters = parameters.Add(F.Parameter(F.Identifier(ArglistParameterName)).WithType(TypesForSymbols.RuntimeArgumentHandle));
            }

            var method = F.MethodDeclaration(ReturnTypeWithoutReadonly, F.Identifier(MemberMockName))
                .WithModifiers(F.TokenList(F.Token(SyntaxKind.ProtectedKeyword), F.Token(SyntaxKind.VirtualKeyword)))
                .WithParameterList(F.ParameterList(parameters))
                .WithBody(F.Block(ThrowMockMissingStatement("VirtualMethod")));

            if (Symbol.TypeParameters.Any())
            {
                method = method.WithTypeParameterList(TypeParameterList());

                var constraints = TypesForSymbols.AsConstraintClauses(Symbol.TypeParameters);

                if (constraints.Any())
                {
                    method = method.AddConstraintClauses(constraints);
                }
            }

            return method;
        }


        private MemberDeclarationSyntax ExplicitInterfaceMember()
        {
            var parameters = F.SeparatedList(Symbol.Parameters.Select(TypesForSymbols.AsParameterSyntax));
            if (ArglistParameterName != null)
            {
                parameters = parameters.Add(F.Parameter(F.Token(SyntaxKind.ArgListKeyword)));
            }

            var arguments = F.SeparatedList(Symbol.Parameters.Select(TypesForSymbols.AsArgumentSyntax));

            if (ArglistParameterName != null && TypesForSymbols.RuntimeArgumentHandle != null)
            {
                arguments = arguments.Add(F.Argument(F.LiteralExpression(SyntaxKind.ArgListExpression, F.Token(SyntaxKind.ArgListKeyword))));
            }

            var mockedMethod = F.MethodDeclaration(ReturnType, Symbol.Name)
                .WithParameterList(F.ParameterList(parameters))
                .WithExplicitInterfaceSpecifier(F.ExplicitInterfaceSpecifier(TypesForSymbols.ParseName(InterfaceSymbol)));

            if (Symbol.TypeParameters.Any())
            {
                mockedMethod = mockedMethod.WithTypeParameterList(TypeParameterList());
            }

            var invocation = Symbol.TypeParameters.Any()
                ? (ExpressionSyntax)F.GenericName(MemberMockName)
                    .WithTypeArgumentList(F.TypeArgumentList(
                        F.SeparatedList(Symbol.TypeParameters.Select(typeParameter => (TypeSyntax)F.IdentifierName(typeParameter.Name)))))
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

        private TypeParameterListSyntax TypeParameterList()
        {
            return F.TypeParameterList(F.SeparatedList(Symbol.TypeParameters.Select(typeParameter => F.TypeParameter(typeParameter.Name))));
        }
    }
}
