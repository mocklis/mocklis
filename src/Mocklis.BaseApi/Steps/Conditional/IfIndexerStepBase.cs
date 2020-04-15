// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IfIndexerStepBase.cs">
//   SPDX-License-Identifier: MIT
//   Copyright © 2019-2020 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.Steps.Conditional
{
    /*
     * See comment on IfEventStepBase class.
     */

    #region Using Directives

    using System;
    using Mocklis.Core;
    using Mocklis.Steps.Missing;

    #endregion

    /// <summary>
    ///     Base class for conditional indexer steps with an alternative branch and the ability to rejoin the normal branch
    ///     from the alternative branch.
    ///     Inherits from the <see cref="IndexerStepWithNext{TKey,TValue}" /> class.
    /// </summary>
    /// <typeparam name="TKey">The type of the indexer key.</typeparam>
    /// <typeparam name="TValue">The type of the indexer value.</typeparam>
    /// <seealso cref="IndexerStepWithNext{TKey, TValue}" />
    public abstract class IfIndexerStepBase<TKey, TValue> : IndexerStepWithNext<TKey, TValue>
    {
        /// <summary>
        ///     Starting point for the alternative path of a conditional indexer step. This class cannot be inherited.
        ///     Implements the <see cref="IIndexerStep{TKey, TValue}" /> interface.
        ///     Implements the <see cref="ICanHaveNextIndexerStep{TKey, TValue}" /> interface.
        /// </summary>
        /// <seealso cref="IIndexerStep{TKey, TValue}" />
        /// <seealso cref="ICanHaveNextIndexerStep{TKey, TValue}" />
        public sealed class IfBranchCaller : IIndexerStep<TKey, TValue>, ICanHaveNextIndexerStep<TKey, TValue>
        {
            private IIndexerStep<TKey, TValue> _nextStep = MissingIndexerStep<TKey, TValue>.Instance;

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

                _nextStep = step;
                return step;
            }

            /// <summary>
            ///     Called when a value is read from the indexer.
            /// </summary>
            /// <param name="mockInfo">Information about the mock through which the value is read.</param>
            /// <param name="key">The indexer key used.</param>
            /// <returns>The value being read.</returns>
            TValue IIndexerStep<TKey, TValue>.Get(IMockInfo mockInfo, TKey key)
            {
                return _nextStep.GetWithStrictnessCheckIfNull(mockInfo, key);
            }

            /// <summary>
            ///     Called when a value is written to the indexer.
            /// </summary>
            /// <param name="mockInfo">Information about the mock through which the value is written.</param>
            /// <param name="key">The indexer key used.</param>
            /// <param name="value">The value being written.</param>
            void IIndexerStep<TKey, TValue>.Set(IMockInfo mockInfo, TKey key, TValue value)
            {
                _nextStep.SetWithStrictnessCheckIfNull(mockInfo, key, value);
            }

            /// <summary>
            ///     Gets a step that can be used to rejoin the normal ('else') branch.
            /// </summary>
            public IIndexerStep<TKey, TValue> ElseBranch { get; }

            /// <summary>
            ///     Initializes a new instance of the <see cref="IfBranchCaller" /> class.
            /// </summary>
            /// <param name="ifIndexerStep">The if step whose 'default' set of steps will constitute 'else' branch.</param>
            public IfBranchCaller(IfIndexerStepBase<TKey, TValue> ifIndexerStep)
            {
                ElseBranch = new ElseBranchRejoiner(ifIndexerStep);
            }
        }

        private sealed class ElseBranchRejoiner : IIndexerStep<TKey, TValue>
        {
            private readonly IfIndexerStepBase<TKey, TValue> _ifIndexerStep;

            public ElseBranchRejoiner(IfIndexerStepBase<TKey, TValue> ifIndexerStep)
            {
                _ifIndexerStep = ifIndexerStep;
            }

            public TValue Get(IMockInfo mockInfo, TKey key)
            {
                // Call directly to next step thus bypassing the condition check.
                return _ifIndexerStep.NextStep.GetWithStrictnessCheckIfNull(mockInfo, key);
            }

            public void Set(IMockInfo mockInfo, TKey key, TValue value)
            {
                // Call directly to next step thus bypassing the condition check.
                _ifIndexerStep.NextStep.SetWithStrictnessCheckIfNull(mockInfo, key, value);
            }
        }

        /// <summary>
        ///     Gets the alternative branch.
        /// </summary>
        protected IIndexerStep<TKey, TValue> IfBranch { get; }

        /// <summary>
        ///     Initializes a new instance of the <see cref="IfIndexerStepBase{TKey, TValue}" /> class.
        /// </summary>
        /// <param name="branch">
        ///     An action to set up the alternative branch; it also provides a means of re-joining the normal
        ///     branch.
        /// </param>
        protected IfIndexerStepBase(Action<IfBranchCaller> branch)
        {
            if (branch == null)
            {
                throw new ArgumentNullException(nameof(branch));
            }

            var ifBranch = new IfBranchCaller(this);
            IfBranch = ifBranch;
            branch.Invoke(ifBranch);
        }
    }
}
