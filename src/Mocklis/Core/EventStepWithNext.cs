// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EventStepWithNext.cs">
//   Copyright © 2019 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.Core
{
    #region Using Directives

    using System;
    using Mocklis.Steps.Missing;

    #endregion

    /// <summary>
    ///     Class that models an event step that can forward calls on to a next step. It is a common base class for
    ///     implementing new steps.
    ///     Implements the <see cref="Mocklis.Core.IEventStep{THandler}" /> interface.
    ///     Implements the <see cref="Mocklis.Core.ICanHaveNextEventStep{THandler}" /> interface.
    /// </summary>
    /// <typeparam name="THandler">The event handler type for the event.</typeparam>
    /// <seealso cref="Mocklis.Core.IEventStep{THandler}" />
    /// <seealso cref="Mocklis.Core.ICanHaveNextEventStep{THandler}" />
    public class EventStepWithNext<THandler> : IEventStep<THandler>, ICanHaveNextEventStep<THandler> where THandler : Delegate
    {
        /// <summary>
        ///     Gets the current next step.
        /// </summary>
        /// <value>The current next step.</value>
        protected IEventStep<THandler> NextStep { get; private set; } = MissingEventStep<THandler>.Instance;

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
        ///     Can be overriden to provide a bespoke behaviour in a step. The default behaviour is to forward the adds on to the
        ///     next step.
        /// </summary>
        /// <param name="mockInfo">Information about the mock through which the event handler is being added.</param>
        /// <param name="value">The event handler that is being added.</param>
        public virtual void Add(IMockInfo mockInfo, THandler value)
        {
            NextStep.Add(mockInfo, value);
        }

        /// <summary>
        ///     Called when an event handler is being removed to the mocked event.
        ///     Can be overriden to provide a bespoke behaviour in a step. The default behaviour is to forward the removes on to
        ///     the next step.
        /// </summary>
        /// <param name="mockInfo">Information about the mock through which the event handler is being removed.</param>
        /// <param name="value">The event handler that is being removed.</param>
        public virtual void Remove(IMockInfo mockInfo, THandler value)
        {
            NextStep.Remove(mockInfo, value);
        }
    }
}
