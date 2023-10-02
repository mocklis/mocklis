// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EventStepWithNext.cs">
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
    ///     Class that models an event step that can forward calls on to a next step. It is a common base class for
    ///     implementing new steps.
    ///     Implements the <see cref="IEventStep{THandler}" /> interface.
    ///     Implements the <see cref="ICanHaveNextEventStep{THandler}" /> interface.
    /// </summary>
    /// <typeparam name="THandler">The event handler type for the event.</typeparam>
    /// <seealso cref="IEventStep{THandler}" />
    /// <seealso cref="ICanHaveNextEventStep{THandler}" />
    /// <seealso cref="IEventStep{THandler}" />
    public class EventStepWithNext<THandler> : IEventStep<THandler>, ICanHaveNextEventStep<THandler> where THandler : Delegate
    {
        /// <summary>
        ///     Gets the current next step.
        /// </summary>
        /// <value>The current next step.</value>
        protected IEventStep<THandler>? NextStep { get; private set; }

        /// <summary>
        ///     Replaces the current 'next' step with a new step.
        /// </summary>
        /// <typeparam name="TStep">The actual type of the new step.</typeparam>
        /// <param name="step">The new step.</param>
        /// <returns>The new step, so that we can add further steps in a fluent fashion.</returns>
        TStep ICanHaveNextEventStep<THandler>.SetNextStep<TStep>(TStep step)
        {
            if (step == null)
            {
                throw new ArgumentNullException(nameof(step));
            }

            NextStep = step;
            return step;
        }

        /// <summary>
        ///     Called when an event handler is being added to the mocked event.
        ///     Can be overridden to provide a bespoke behaviour in a step. The default behaviour is to forward the adds on to the
        ///     next step.
        /// </summary>
        /// <param name="mockInfo">Information about the mock through which the event handler is being added.</param>
        /// <param name="value">The event handler that is being added.</param>
        public virtual void Add(IMockInfo mockInfo, THandler? value)
        {
            NextStep.AddWithStrictnessCheckIfNull(mockInfo, value);
        }

        /// <summary>
        ///     Called when an event handler is being removed from the mocked event.
        ///     Can be overridden to provide a bespoke behaviour in a step. The default behaviour is to forward the removes on to
        ///     the next step.
        /// </summary>
        /// <param name="mockInfo">Information about the mock through which the event handler is being removed.</param>
        /// <param name="value">The event handler that is being removed.</param>
        public virtual void Remove(IMockInfo mockInfo, THandler? value)
        {
            NextStep.RemoveWithStrictnessCheckIfNull(mockInfo, value);
        }
    }
}
