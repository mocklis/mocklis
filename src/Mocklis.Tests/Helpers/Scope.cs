// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Scope.cs">
//   SPDX-License-Identifier: MIT
//   Copyright © 2019-2020 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.Tests.Helpers
{
    #region Using Directives

    using System;
    using System.Globalization;
#if !NETCOREAPP1_1
    using System.Threading;
#endif

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
#if NETCOREAPP1_1
                _savedCulture = CultureInfo.CurrentCulture;
                CultureInfo.CurrentCulture = cultureInfo;
#else
                _savedCulture = Thread.CurrentThread.CurrentCulture;
                Thread.CurrentThread.CurrentCulture = cultureInfo;
#endif
            }

            public void Dispose()
            {
#if NETCOREAPP1_1
                CultureInfo.CurrentCulture = _savedCulture;
#else
                Thread.CurrentThread.CurrentCulture = _savedCulture;
#endif
            }
        }
    }
}
