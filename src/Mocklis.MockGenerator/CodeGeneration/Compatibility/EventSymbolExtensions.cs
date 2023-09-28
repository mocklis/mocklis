// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EventSymbolExtensions.cs">
//   SPDX-License-Identifier: MIT
//   Copyright © 2019-2021 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.CodeGeneration.Compatibility
{
    #region Using Directives

    using Microsoft.CodeAnalysis;

    #endregion

    public static class EventSymbolExtensions
    {
        public static bool NullableOrOblivious(this IEventSymbol eventSymbol)
        {
            return eventSymbol.NullableAnnotation != NullableAnnotation.NotAnnotated;
        }
    }
}
