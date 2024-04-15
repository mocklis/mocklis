// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PropertyStepWithNext.cs">
//   SPDX-License-Identifier: MIT
//   Copyright © 2019-2024 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.Core
{
    #region Using Directives

    using System;

    #endregion

    /// <summary>
    ///     Class that models a property step that can forward calls on to a next step. It is a common base class for
    ///     implementing new steps.
    ///     Implements the <see cref="IPropertyStep{TValue}" /> interface.
    ///     Implements the <see cref="ICanHaveNextPropertyStep{TValue}" /> interface.
    /// </summary>
    /// <typeparam name="TValue">The type of the property.</typeparam>
    /// <seealso cref="IPropertyStep{TValue}" />
    /// <seealso cref="ICanHaveNextPropertyStep{TValue}" />
    public class PropertyStepWithNext<TValue> : IPropertyStep<TValue>, ICanHaveNextPropertyStep<TValue>
    {
        /// <summary>
        ///     Gets the current next step.
        /// </summary>
        /// <value>The current next step.</value>
        protected IPropertyStep<TValue>? NextStep { get; private set; }

        /// <summary>
        ///     Replaces the current 'next' step with a new step.
        /// </summary>
        /// <typeparam name="TStep">The actual type of the new step.</typeparam>
        /// <param name="step">The new step.</param>
        /// <returns>The new step, so that we can add further steps in a fluent fashion.</returns>
        TStep ICanHaveNextPropertyStep<TValue>.SetNextStep<TStep>(TStep step)
        {
            if (step == null)
            {
                throw new ArgumentNullException(nameof(step));
            }

            NextStep = step;
            return step;
        }

        /// <summary>
        ///     Called when a value is read from the property.
        ///     Can be overridden to provide a bespoke behaviour in a step. The default behaviour is to forward the reads on to the
        ///     next step.
        /// </summary>
        /// <param name="mockInfo">Information about the mock through which the value is read.</param>
        /// <returns>The value being read.</returns>
        public virtual TValue Get(IMockInfo mockInfo)
        {
            return NextStep.GetWithStrictnessCheckIfNull(mockInfo);
        }

        /// <summary>
        ///     Called when a value is written to the property.
        ///     Can be overridden to provide a bespoke behaviour in a step. The default behaviour is to forward the reads on to the
        ///     next step.
        /// </summary>
        /// <param name="mockInfo">Information about the mock through which the value is written.</param>
        /// <param name="value">The value being written.</param>
        public virtual void Set(IMockInfo mockInfo, TValue value)
        {
            NextStep.SetWithStrictnessCheckIfNull(mockInfo, value);
        }

        /// <summary>
        ///     Removes the current 'next' step, restoring the mock to its non-programmed behaviour.
        /// </summary>
        public void Clear()
        {
            NextStep = null;
        }
    }
}
