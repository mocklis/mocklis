// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RoslynExtensions.cs">
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
    using F = Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

    #endregion

    public static class RoslynExtensions
    {
        public static ObjectCreationExpressionSyntax WithExpressionsAsArgumentList(
            this ObjectCreationExpressionSyntax objectCreationExpression,
            params ExpressionSyntax[] expressions)
        {
            return objectCreationExpression.WithArgumentList(
                F.ArgumentList(F.SeparatedList(expressions.Where(e => e != null).Select(F.Argument))));
        }

        public static InvocationExpressionSyntax WithExpressionsAsArgumentList(
            this InvocationExpressionSyntax invocationExpression,
            params ExpressionSyntax?[] expressions)
        {
            return invocationExpression.WithArgumentList(
                F.ArgumentList(F.SeparatedList(expressions.Where(e => e != null).Select(e => F.Argument(e!)))));
        }

        public static ElementAccessExpressionSyntax WithExpressionsAsArgumentList(
            this ElementAccessExpressionSyntax elementAccessExpression,
            params ExpressionSyntax?[] expressions)
        {
            return elementAccessExpression.WithArgumentList(
                F.BracketedArgumentList(
                    F.SeparatedList(expressions.Where(e => e != null).Select(e => F.Argument(e!)))));
        }

        public static IEnumerable<string> GetUsableNames(this ITypeSymbol typeSymbol)
        {
            ITypeSymbol? tmp = typeSymbol;

            while (tmp != null)
            {
                foreach (var member in tmp.GetMembers())
                {
                    if (member.CanBeReferencedByName)
                    {
                        yield return member.Name;
                    }
                }

                tmp = tmp.BaseType;
            }
        }

        public static SeparatedSyntaxList<ArgumentSyntax> AsArgumentList(this IEnumerable<IParameterSymbol> parameters)
        {
            var args = parameters.Select(p =>
            {
                var syntax = F.Argument(F.IdentifierName(p.Name));

                switch (p.RefKind)
                {
                    case RefKind.Out:
                    {
                        syntax = syntax.WithRefOrOutKeyword(F.Token(SyntaxKind.OutKeyword));
                        break;
                    }

                    case RefKind.Ref:
                    {
                        syntax = syntax.WithRefOrOutKeyword(F.Token(SyntaxKind.RefKeyword));
                        break;
                    }
                }

                return syntax;
            });

            return F.SeparatedList(args);
        }

        public static ParameterListSyntax AsParameterList(this IEnumerable<IParameterSymbol> parameters, MocklisTypesForSymbols typesForSymbols)
        {
            var args = parameters.Select(p =>
            {
                var syntax = F.Parameter(F.Identifier(p.Name)).WithType(typesForSymbols.ParseTypeName(p.Type, p.NullableOrOblivious()));

                switch (p.RefKind)
                {
                    case RefKind.In:
                    {
                        syntax = syntax.WithModifiers(F.TokenList(F.Token(SyntaxKind.InKeyword)));
                        break;
                    }

                    case RefKind.Out:
                    {
                        syntax = syntax.WithModifiers(F.TokenList(F.Token(SyntaxKind.OutKeyword)));
                        break;
                    }

                    case RefKind.Ref:
                    {
                        syntax = syntax.WithModifiers(F.TokenList(F.Token(SyntaxKind.RefKeyword)));
                        break;
                    }
                }

                return syntax;
            });

            return F.ParameterList(F.SeparatedList(args));
        }

        public static PropertyDeclarationSyntax MockProperty(this TypeSyntax mockPropertyType, string memberMockName)
        {
            return F.PropertyDeclaration(mockPropertyType, memberMockName).AddModifiers(F.Token(SyntaxKind.PublicKeyword))
                .AddAccessorListAccessors(F.AccessorDeclaration(SyntaxKind.GetAccessorDeclaration)
                    .WithSemicolonToken(F.Token(SyntaxKind.SemicolonToken)));
        }

        public static string FullName(this ITypeSymbol typeSymbol)
        {
            if (typeSymbol.ContainingNamespace.IsGlobalNamespace)
            {
                return typeSymbol.Name;
            }

            return typeSymbol.ContainingNamespace.Name + "." + typeSymbol.Name;
        }
    }
}
