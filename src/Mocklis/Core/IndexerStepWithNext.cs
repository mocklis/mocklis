// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IndexerStepWithNext.cs">
//   SPDX-License-Identifier: MIT
//   Copyright © 2019-2020 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.Core
{
    #region Using Directives

    using System;

    #endregion

    /// <summary>
    ///     Class that models an indexer step that can forward calls on to a next step. It is a common base class for
    ///     implementing new steps.
    ///     Implements the <see cref="IIndexerStep{TKey, TValue}" /> interface.
    ///     Implements the <see cref="ICanHaveNextIndexerStep{TKey, TValue}" /> interface.
    /// </summary>
    /// <typeparam name="TKey">The type of the indexer key.</typeparam>
    /// <typeparam name="TValue">The type of the indexer value.</typeparam>
    /// <seealso cref="IIndexerStep{TKey, TValue}" />
    /// <seealso cref="ICanHaveNextIndexerStep{TKey, TValue}" />
    public class IndexerStepWithNext<TKey, TValue> : IIndexerStep<TKey, TValue>, ICanHaveNextIndexerStep<TKey, TValue>
    {
        /// <summary>
        ///     Gets the current next step.
        /// </summary>
        /// <value>The current next step.</value>
        protected IIndexerStep<TKey, TValue>? NextStep { get; private set; }

        /// <summary>
        ///     Replaces the current 'next' step with a new step.
        /// </summary>
        /// <typeparam name="TStep">The actual type of the new step.</typeparam>
        /// <param name="step">The new step.</param>
        /// <returns>The new step, so that we can add further steps in a fluent fashion.</returns>
        TStep ICanHaveNextIndexerStep<TKey, TValue>.SetNextStep<TStep>(TStep step)
        {
            if (step == null)
            {
                throw new ArgumentNullException(nameof(step));
            }

            NextStep = step;
            return step;
        }

        /// <summary>
        ///     Called when a value is read from the indexer.
        ///     Can be overridden to provide a bespoke behaviour in a step. The default behaviour is to forward the reads on to the
        ///     next step.
        /// </summary>
        /// <param name="mockInfo">Information about the mock through which the value is read.</param>
        /// <param name="key">The indexer key used.</param>
        /// <returns>The value being read.</returns>
        public virtual TValue Get(IMockInfo mockInfo, TKey key)
        {
            return NextStep.GetWithStrictnessCheckIfNull(mockInfo, key);
        }

        /// <summary>
        ///     Called when a value is written to the indexer.
        ///     Can be overridden to provide a bespoke behaviour in a step. The default behaviour is to forward the writes on to
        ///     the
        ///     next step.
        /// </summary>
        /// <param name="mockInfo">Information about the mock through which the value is written.</param>
        /// <param name="key">The indexer key used.</param>
        /// <param name="value">The value being written.</param>
        public virtual void Set(IMockInfo mockInfo, TKey key, TValue value)
        {
            NextStep.SetWithStrictnessCheckIfNull(mockInfo, key, value);
        }
    }
}
