// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ParameterSymbolExtensions.cs">
//   SPDX-License-Identifier: MIT
//   Copyright © 2019-2023 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.MockGenerator.CodeGeneration.Compatibility
{
    #region Using Directives

    using System.Reflection;
    using Microsoft.CodeAnalysis;

    #endregion

    public static class ParameterSymbolExtensions
    {
        private static readonly PropertyInfo? NullableAnnotationPropertyInfo = typeof(IParameterSymbol).GetProperty("NullableAnnotation");

        public static bool NullableOrOblivious(this IParameterSymbol parameterSymbol)
        {
            if (NullableAnnotationPropertyInfo == null)
            {
                return true;
            }

            var result = (byte)NullableAnnotationPropertyInfo.GetValue(parameterSymbol);
            return result != 1;
        }
    }
}
