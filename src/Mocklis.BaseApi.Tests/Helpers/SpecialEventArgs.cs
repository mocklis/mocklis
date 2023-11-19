// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SpecialEventArgs.cs">
//   SPDX-License-Identifier: MIT
//   Copyright © 2019-2023 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.Helpers
{
    #region Using Directives

    using System;

    #endregion

    public class SpecialEventArgs : EventArgs
    {
        public string Text { get; }
        public int Number { get; }

        public SpecialEventArgs(string text, int number)
        {
            Text = text;
            Number = number;
        }
    }
}
