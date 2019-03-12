// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IEvents.cs">
//   SPDX-License-Identifier: MIT
//   Copyright © 2019 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.Tests.Interfaces
{
    #region Using Directives

    using System;

    #endregion

    public interface IEvents
    {
        event EventHandler MyEvent;
    }
}
