// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ModuloNComparer.cs">
//   SPDX-License-Identifier: MIT
//   Copyright © 2019 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.Tests.Helpers
{
    #region Using Directives

    using System;
    using System.Collections.Generic;

    #endregion

    public sealed class StringLengthComparer : IEqualityComparer<string>
    {
        public StringLengthComparer()
        {
        }

        public bool Equals(string x, string y) => ((x?.Length) ?? 0) == ((y?.Length) ?? 0);

        public int GetHashCode(string obj) => (obj?.Length) ?? 0;
    }
}
