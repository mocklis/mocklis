// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AddActionEventStep.cs">
//   SPDX-License-Identifier: MIT
//   Copyright © 2019-2024 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.Steps.Lambda
{
    #region Using Directives

    using System;
    using Mocklis.Core;

    #endregion

    /// <summary>
    ///     Class that represents an 'AddAction' event step.
    ///     Inherits from the <see cref="EventStepWithNext{THandler}" /> class.
    /// </summary>
    /// <typeparam name="THandler">The event handler type for the event.</typeparam>
    /// <seealso cref="EventStepWithNext{THandler}" />
    public class AddActionEventStep<THandler> : EventStepWithNext<THandler> where THandler : Delegate
    {
        private readonly Action<THandler?> _action;

        /// <summary>
        ///     Initializes a new instance of the <see cref="AddActionEventStep{THandler}" /> class.
        /// </summary>
        /// <param name="action">An action to be invoked when an event handler is added.</param>
        public AddActionEventStep(Action<THandler?> action)
        {
            _action = action ?? throw new ArgumentNullException(nameof(action));
        }

        /// <summary>
        ///     Called when an event handler is being added to the mocked event.
        ///     This implementation invokes the action with the event handler.
        /// </summary>
        /// <param name="mockInfo">Information about the mock through which the event handler is being added.</param>
        /// <param name="value">The event handler that is being added.</param>
        public override void Add(IMockInfo mockInfo, THandler? value)
        {
            _action(value);
        }
    }
}
