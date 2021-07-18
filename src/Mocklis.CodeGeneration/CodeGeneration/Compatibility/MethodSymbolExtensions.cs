// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MethodSymbolExtensions.cs">
//   SPDX-License-Identifier: MIT
//   Copyright © 2019-2021 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.CodeGeneration.Compatibility
{
    #region Using Directives

    using System.Reflection;
    using Microsoft.CodeAnalysis;

    #endregion

    public static class MethodSymbolExtensions
    {
        private static readonly PropertyInfo ReturnNullableAnnotationPropertyInfo = typeof(IMethodSymbol).GetProperty("ReturnNullableAnnotation");

        public static bool ReturnTypeIsNullableOrOblivious(this IMethodSymbol methodSymbol)
        {
            if (ReturnNullableAnnotationPropertyInfo == null)
            {
                return true;
            }

            var result = (byte)ReturnNullableAnnotationPropertyInfo.GetValue(methodSymbol);
            return result != 1;
        }
    }
}
