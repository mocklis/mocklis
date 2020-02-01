// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IEvents.cs">
//   SPDX-License-Identifier: MIT
//   Copyright © 2019-2020 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.Tests.Interfaces
{
    #region Using Directives

    using System;
    using Mocklis.Tests.Helpers;

    #endregion

    public interface IEvents
    {
        event EventHandler? MyEvent;
        event EventHandler<SpecialEventArgs>? SpecialEvent;
    }
}
