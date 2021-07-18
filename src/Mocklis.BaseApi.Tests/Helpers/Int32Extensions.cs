// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Int32Extensions.cs">
//   SPDX-License-Identifier: MIT
//   Copyright © 2019-2021 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.Helpers
{
    #region Using Directives

    using System;

    #endregion

    public static class Int32Extensions
    {
        public static DateTime AtUtc(this int date, int time)
        {
            var day = date % 100;
            var month = date / 100 % 100;
            var year = date / 10000;

            var second = time % 100;
            var minute = time / 100 % 100;
            var hour = time / 10000;

            return new DateTime(year, month, day, hour, minute, second, DateTimeKind.Utc);
        }
    }
}
