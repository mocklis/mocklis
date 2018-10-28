// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RoslynExtensions.cs">
//   Copyright © 2018 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.CodeGeneration
{
    #region Using Directives

    using System.Linq;
    using Microsoft.CodeAnalysis.CSharp;
    using Microsoft.CodeAnalysis.CSharp.Syntax;

    #endregion

    public static class RoslynExtensions
    {
        public static ObjectCreationExpressionSyntax WithExpressionsAsArgumentList(
            this ObjectCreationExpressionSyntax objectCreationExpression,
            params ExpressionSyntax[] expressions)
        {
            return objectCreationExpression.WithArgumentList(
                SyntaxFactory.ArgumentList(SyntaxFactory.SeparatedList(expressions.Select(SyntaxFactory.Argument))));
        }

        public static InvocationExpressionSyntax WithExpressionsAsArgumentList(
            this InvocationExpressionSyntax invocationExpression,
            params ExpressionSyntax[] expressions)
        {
            return invocationExpression.WithArgumentList(
                SyntaxFactory.ArgumentList(SyntaxFactory.SeparatedList(expressions.Select(SyntaxFactory.Argument))));
        }
    }
}
