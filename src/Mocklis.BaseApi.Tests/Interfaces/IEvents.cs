// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IEvents.cs">
//   SPDX-License-Identifier: MIT
//   Copyright © 2019-2024 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.Interfaces
{
    #region Using Directives

    using System;
    using Mocklis.Helpers;

    #endregion

    public interface IEvents
    {
        event EventHandler? MyEvent;
        event EventHandler<SpecialEventArgs>? SpecialEvent;
    }
}
