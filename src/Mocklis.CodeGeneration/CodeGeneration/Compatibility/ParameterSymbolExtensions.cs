// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ParameterSymbolExtensions.cs">
//   SPDX-License-Identifier: MIT
//   Copyright © 2019 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.CodeGeneration.Compatibility
{
    #region Using Directives

    using System.Reflection;
    using Microsoft.CodeAnalysis;

    #endregion

    public static class ParameterSymbolExtensions
    {
        private static readonly PropertyInfo NullableAnnotationPropertyInfo = typeof(IParameterSymbol).GetProperty("NullableAnnotation");

        public static bool NullableOrOblivious(this IParameterSymbol parameterSymbol)
        {
            var result = (byte)NullableAnnotationPropertyInfo.GetValue(parameterSymbol);
            return result != 1;
        }
    }
}
