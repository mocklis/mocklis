// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Asserts.cs">
//   SPDX-License-Identifier: MIT
//   Copyright © 2019-2023 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.Helpers
{
    #region Using Directives

    using System;

    #endregion

    public static class Asserts
    {
        public static T AssertNotNull<T>(this T? item) where T : class
        {
            return item ?? throw new ArgumentNullException(nameof(item));
        }
    }
}
