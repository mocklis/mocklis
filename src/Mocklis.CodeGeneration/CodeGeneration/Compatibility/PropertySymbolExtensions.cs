// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PropertySymbolExtensions.cs">
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

    public static class PropertySymbolExtensions
    {
        private static readonly PropertyInfo NullableAnnotationPropertyInfo = typeof(IPropertySymbol).GetProperty("NullableAnnotation");

        public static bool NullableOrOblivious(this IPropertySymbol propertySymbol)
        {
            if (NullableAnnotationPropertyInfo == null)
            {
                return true;
            }

            var result = (byte)NullableAnnotationPropertyInfo.GetValue(propertySymbol);
            return result != 1;
        }
    }
}
