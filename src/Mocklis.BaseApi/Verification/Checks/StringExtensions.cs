// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StringExtensions.cs">
//   SPDX-License-Identifier: MIT
//   Copyright © 2019-2024 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.Verification.Checks
{
    internal static class StringExtensions
    {
        /// <summary>
        ///     Wraps the string value in single quotes, unless it's null in which case the string &lt;null&gt; is returned.
        ///     The goal is to provide the ability to distinguish between empty string and null values in messages.
        /// </summary>
        /// <param name="value">A nullable string.</param>
        /// <returns>A string with the original value wrapped in single quotes, or the string &lt;null&gt;.</returns>
        public static string QuotedOrNull(this string? value)
        {
            return value switch
            {
                null => "<null>",
                _ => $"'{value}'"
            };
        }
    }
}
