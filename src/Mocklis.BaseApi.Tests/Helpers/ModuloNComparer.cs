// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ModuloNComparer.cs">
//   SPDX-License-Identifier: MIT
//   Copyright © 2019-2020 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.Helpers
{
    #region Using Directives

    using System;
    using System.Collections.Generic;

    #endregion

    public sealed class ModuloNComparer : IEqualityComparer<int>
    {
        private readonly int _n;

        public ModuloNComparer(int n)
        {
            _n = n >= 1 ? n : throw new ArgumentOutOfRangeException(nameof(n));
        }

        private int Mod(int value)
        {
            var result = value % _n;
            if (result >= 0)
            {
                return result;
            }

            return result + _n;
        }

        public bool Equals(int x, int y) => Mod(x) == Mod(y);

        public int GetHashCode(int obj) => Mod(obj);
    }
}
