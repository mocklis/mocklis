// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StoredEventStep.cs">
//   SPDX-License-Identifier: MIT
//   Copyright © 2019-2020 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.Steps.Stored
{
    #region Using Directives

    using System;
    using System.Threading;
    using Mocklis.Core;

    #endregion

    /// <summary>
    ///     Class that represents a 'Stored' event step. It tracks event handlers exactly as a normal event would, combining
    ///     and removing delegates as needed.
    ///     Implements the <see cref="IEventStep{THandler}" /> interface.
    ///     Implements the <see cref="IStoredEvent{THandler}" /> interface.
    /// </summary>
    /// <typeparam name="THandler">The event handler type for the event.</typeparam>
    /// <seealso cref="IEventStep{THandler}" />
    /// <seealso cref="IStoredEvent{THandler}" />
    public class StoredEventStep<THandler> : IEventStep<THandler>, IStoredEvent<THandler> where THandler : Delegate
    {
        private THandler? _eventHandler;

        /// <summary>
        ///     Gets the currently stored event handler.
        /// </summary>
        public THandler? EventHandler => _eventHandler;

        /// <summary>
        ///     Called when an event handler is being added to the mocked event.
        /// </summary>
        /// <param name="mockInfo">Information about the mock through which the event handler is being added.</param>
        /// <param name="value">The event handler that is being added.</param>
        void IEventStep<THandler>.Add(IMockInfo mockInfo, THandler? value) => Add(value);

        /// <summary>
        ///     Adds an event handler to the store.
        /// </summary>
        /// <param name="value">The event handler to add.</param>
        public void Add(THandler? value)
        {
            THandler? previousHandler;
            THandler? eventHandler = _eventHandler;
            do
            {
                previousHandler = eventHandler;
                THandler newHandler = (THandler)Delegate.Combine(previousHandler, value);
                eventHandler = Interlocked.CompareExchange(ref _eventHandler, newHandler, previousHandler);
            }
            while (eventHandler != previousHandler);
        }

        /// <summary>
        ///     Called when an event handler is being removed to the mocked event.
        /// </summary>
        /// <param name="mockInfo">Information about the mock through which the event handler is being removed.</param>
        /// <param name="value">The event handler that is being removed.</param>
        void IEventStep<THandler>.Remove(IMockInfo mockInfo, THandler? value) => Remove(value);

        /// <summary>
        ///     Removes an event handler from the store.
        /// </summary>
        /// <param name="value">The event handler to remove.</param>
        public void Remove(THandler? value)
        {
            THandler? previousHandler;
            THandler? eventHandler = _eventHandler;
            do
            {
                previousHandler = eventHandler;
                THandler? newHandler = (THandler)Delegate.Remove(previousHandler, value);
                eventHandler = Interlocked.CompareExchange(ref _eventHandler, newHandler, previousHandler);
            }
            while (eventHandler != previousHandler);
        }

        /// <summary>
        ///     Removes all event handlers from the store.
        /// </summary>
        public void Clear()
        {
            _eventHandler = null;
        }
    }
}
