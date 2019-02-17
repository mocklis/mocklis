// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IEvents.cs">
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
