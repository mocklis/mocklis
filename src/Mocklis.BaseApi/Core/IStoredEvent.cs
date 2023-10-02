// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IStoredEvent.cs">
//   SPDX-License-Identifier: MIT
//   Copyright © 2019-2023 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.Core
{
    #region Using Directives

    using System;

    #endregion

    /// <summary>
    ///     Interface that provides access to stored event handler for a 'stored' event step.
    /// </summary>
    /// <typeparam name="THandler">The event handler type for the event.</typeparam>
    public interface IStoredEvent<out THandler> where THandler : Delegate
    {
        /// <summary>
        ///     Gets the currently stored event handler.
        /// </summary>
        THandler? EventHandler { get; }
    }
}
