// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TypeParameterSymbolExtensions.cs">
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

    public static class TypeParameterSymbolExtensions
    {
        private static readonly PropertyInfo HasNotNullConstraintPropertyInfo = typeof(ITypeParameterSymbol).GetProperty("HasNotNullConstraint");

        public static bool HasNotNullConstraint(this ITypeParameterSymbol typeParameterSymbol)
        {
            if (HasNotNullConstraintPropertyInfo == null)
            {
                return false;
            }

            return (bool)HasNotNullConstraintPropertyInfo.GetValue(typeParameterSymbol);
        }
    }
}
