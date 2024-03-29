// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IMembers.cs">
//   SPDX-License-Identifier: MIT
//   Copyright © 2019-2024 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.Interfaces
{
    #region Using Directives

    using System;

    #endregion

    public interface IMembers
    {
        event EventHandler? MyEvent;
        string this[int index] { get; set; }
        void DoStuff();
        int Calculate(int value1, int value2);
        string StringProperty { get; set; }
    }
}
