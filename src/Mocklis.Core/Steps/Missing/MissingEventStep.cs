// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MissingEventStep.cs">
//   Copyright © 2019 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.Steps.Missing
{
    #region Using Directives

    using System;
    using Mocklis.Core;

    #endregion

    /// <summary>
    ///     Class that represents a 'Missing' event step. This class cannot be inherited.
    ///     Implements the <see cref="IEventStep{THandler}" /> interface.
    /// </summary>
    /// <typeparam name="THandler">The event handler type for the event.</typeparam>
    /// <seealso cref="IEventStep{THandler}" />
    public sealed class MissingEventStep<THandler> : IEventStep<THandler> where THandler : Delegate
    {
        /// <summary>
        ///     The singleton <see cref="MissingEventStep{THandler}" /> instance for this type of mocked events.
        /// </summary>
        /// <remarks>
        ///     We can use a singleton for this step as it's both final and keeps no state.
        /// </remarks>
        public static readonly MissingEventStep<THandler> Instance = new MissingEventStep<THandler>();

        private MissingEventStep()
        {
        }

        /// <summary>
        ///     Called when an event handler is being added to the mocked event.
        ///     This implementation will throw a <see cref="MockMissingException" />.
        /// </summary>
        /// <param name="mockInfo">Information about the mock through which the event handler is being added.</param>
        /// <param name="value">The event handler that is being added.</param>
        public void Add(IMockInfo mockInfo, THandler value)
        {
            throw new MockMissingException(MockType.EventAdd, mockInfo);
        }

        /// <summary>
        ///     Called when an event handler is being removed to the mocked event.
        ///     This implementation will throw a <see cref="MockMissingException" />.
        /// </summary>
        /// <param name="mockInfo">Information about the mock through which the event handler is being removed.</param>
        /// <param name="value">The event handler that is being removed.</param>
        /// <exception cref="MockMissingException"></exception>
        public void Remove(IMockInfo mockInfo, THandler value)
        {
            throw new MockMissingException(MockType.EventRemove, mockInfo);
        }
    }
}
