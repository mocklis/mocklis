// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StringLengthComparer.cs">
//   SPDX-License-Identifier: MIT
//   Copyright © 2019-2021 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.Helpers
{
    #region Using Directives

    using System.Collections.Generic;

    #endregion

    public sealed class StringLengthComparer : IEqualityComparer<string>
    {
        private static int GetLength(string? s)
        {
            return s?.Length ?? 0;
        }

        public bool Equals(string? x, string? y) => GetLength(x) == GetLength(y);

        public int GetHashCode(string obj) => GetLength(obj);
    }
}
