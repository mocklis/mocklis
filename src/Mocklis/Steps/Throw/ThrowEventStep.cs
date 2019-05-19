// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ThrowEventStep.cs">
//   SPDX-License-Identifier: MIT
//   Copyright © 2019 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.Steps.Throw
{
    #region Using Directives

    using System;
    using Mocklis.Core;

    #endregion

    /// <summary>
    ///     Class that represents a 'Throw' event step, that will throw an exception whenever an event handler is added or
    ///     removed.
    ///     Implements the <see cref="IEventStep{THandler}" /> interface.
    /// </summary>
    /// <typeparam name="THandler">The event handler type for the event.</typeparam>
    /// <seealso cref="IEventStep{THandler}" />
    public class ThrowEventStep<THandler> : IEventStep<THandler> where THandler : Delegate
    {
        private readonly Func<object, THandler, Exception> _exceptionFactory;

        /// <summary>
        ///     Initializes a new instance of the <see cref="ThrowEventStep{THandler}" /> class.
        /// </summary>
        /// <param name="exceptionFactory">
        ///     A Func that creates the exception to be thrown. Takes the mocked instance and event
        ///     handler as parameters.
        /// </param>
        public ThrowEventStep(Func<object, THandler, Exception> exceptionFactory)
        {
            _exceptionFactory = exceptionFactory ?? throw new ArgumentNullException(nameof(exceptionFactory));
        }

        /// <summary>
        ///     Called when an event handler is being added to the mocked event. This implementation creates and throws an
        ///     exception.
        /// </summary>
        /// <param name="mockInfo">Information about the mock through which the event handler is being added.</param>
        /// <param name="value">The event handler that is being added.</param>
        public void Add(IMockInfo mockInfo, THandler value)
        {
            throw _exceptionFactory(mockInfo.MockInstance, value);
        }

        /// <summary>
        ///     Called when an event handler is being removed to the mocked event. This implementation creates and throws an
        ///     exception.
        /// </summary>
        /// <param name="mockInfo">Information about the mock through which the event handler is being removed.</param>
        /// <param name="value">The event handler that is being removed.</param>
        public void Remove(IMockInfo mockInfo, THandler value)
        {
            throw _exceptionFactory(mockInfo.MockInstance, value);
        }
    }
}
