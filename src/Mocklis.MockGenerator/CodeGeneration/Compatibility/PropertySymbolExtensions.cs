// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PropertySymbolExtensions.cs">
//   SPDX-License-Identifier: MIT
//   Copyright © 2019-2021 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.CodeGeneration.Compatibility
{
    #region Using Directives

    using Microsoft.CodeAnalysis;

    #endregion

    public static class PropertySymbolExtensions
    {
        public static bool NullableOrOblivious(this IPropertySymbol propertySymbol)
        {
            return propertySymbol.NullableAnnotation != NullableAnnotation.NotAnnotated;
        }
    }
}
