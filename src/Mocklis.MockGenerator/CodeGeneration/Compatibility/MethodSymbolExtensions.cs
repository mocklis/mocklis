// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MethodSymbolExtensions.cs">
//   SPDX-License-Identifier: MIT
//   Copyright © 2019-2021 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.CodeGeneration.Compatibility
{
    #region Using Directives

    using Microsoft.CodeAnalysis;

    #endregion

    public static class MethodSymbolExtensions
    {
        public static bool ReturnTypeIsNullableOrOblivious(this IMethodSymbol methodSymbol)
        {
            return methodSymbol.ReturnNullableAnnotation != NullableAnnotation.NotAnnotated;
        }
    }
}
