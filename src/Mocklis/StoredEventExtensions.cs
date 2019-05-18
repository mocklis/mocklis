// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StoredEventExtensions.cs">
//   SPDX-License-Identifier: MIT
//   Copyright © 2019 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis
{
    #region Using Directives

    using System;
    using System.ComponentModel;
    using Mocklis.Core;

    #endregion

    /// <summary>
    ///     Convenience extension methods that makes raising events on handlers in an IStoredEvent step a little easier.
    /// </summary>
    public static class StoredEventExtensions
    {
        /// <summary>
        ///     Raises an <see cref="EventHandler" /> event using a sender and <see cref="EventArgs" /> pair.
        /// </summary>
        /// <param name="storedEvent">The stored event step that holds the event handler.</param>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        public static void Raise(this IStoredEvent<EventHandler> storedEvent, object sender, EventArgs e)
        {
            storedEvent.EventHandler?.Invoke(sender, e);
        }

        /// <summary>
        ///     Raises an <see cref="EventHandler{TEventArgs}" /> generic event using a sender and
        ///     <typeparamref name="TEventArg" /> pair.
        /// </summary>
        /// <typeparam name="TEventArg">The type argument used for this event handler type.</typeparam>
        /// <param name="storedEvent">The stored event step that holds the event handler.</param>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The <typeparamref name="TEventArg" /> instance containing the event data.</param>
        public static void Raise<TEventArg>(this IStoredEvent<EventHandler<TEventArg>> storedEvent, object sender,
            TEventArg e) where TEventArg : EventArgs
        {
            storedEvent.EventHandler?.Invoke(sender, e);
        }

        /// <summary>
        ///     Raises an <see cref="PropertyChangedEventHandler" /> event using a sender and
        ///     <see cref="PropertyChangedEventArgs" /> pair.
        /// </summary>
        /// <param name="storedEvent">The stored event step that holds the event handler.</param>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The <see cref="PropertyChangedEventArgs" /> instance containing the event data.</param>
        public static void Raise(this IStoredEvent<PropertyChangedEventHandler> storedEvent, object sender,
            PropertyChangedEventArgs e)
        {
            storedEvent.EventHandler?.Invoke(sender, e);
        }
    }
}
