// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EventSymbolExtensions.cs">
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

    public static class EventSymbolExtensions
    {
        private static readonly PropertyInfo? NullableAnnotationPropertyInfo = typeof(IEventSymbol).GetProperty("NullableAnnotation");

        public static bool NullableOrOblivious(this IEventSymbol eventSymbol)
        {
            if (NullableAnnotationPropertyInfo == null)
            {
                return true;
            }

            var result = (byte)NullableAnnotationPropertyInfo.GetValue(eventSymbol);
            return result != 1;
        }
    }
}
