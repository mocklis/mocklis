// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IStoredEvent.cs">
//   Copyright © 2018 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.Verification
{
    #region Using Directives

    using System;

    #endregion

    public interface IStoredEvent<out THandler> where THandler : Delegate
    {
        THandler EventHandler { get; }
    }
}
