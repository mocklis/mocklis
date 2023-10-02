// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ParameterSymbolExtensions.cs">
//   SPDX-License-Identifier: MIT
//   Copyright © 2019-2023 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.CodeGeneration.Compatibility
{
    #region Using Directives

    using Microsoft.CodeAnalysis;

    #endregion

    public static class ParameterSymbolExtensions
    {
        public static bool NullableOrOblivious(this IParameterSymbol parameterSymbol)
        {
            return parameterSymbol.NullableAnnotation != NullableAnnotation.NotAnnotated;
        }
    }
}
