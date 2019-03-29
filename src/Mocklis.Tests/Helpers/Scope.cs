// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Scope.cs">
//   SPDX-License-Identifier: MIT
//   Copyright © 2019 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.Tests.Helpers
{
    #region Using Directives

    using System;
    using System.Globalization;
    using System.Threading;

    #endregion

    public static class Scope
    {
        public static IDisposable CurrentCulture(CultureInfo cultureInfo)
        {
            return new CultureScope(cultureInfo);
        }

        private sealed class CultureScope : IDisposable
        {
            private readonly CultureInfo _savedCulture;

            public CultureScope(CultureInfo cultureInfo)
            {
                _savedCulture = Thread.CurrentThread.CurrentCulture;
                Thread.CurrentThread.CurrentCulture = cultureInfo;
            }

            public void Dispose()
            {
                Thread.CurrentThread.CurrentCulture = _savedCulture;
            }
        }
    }
}
