// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IfIndexerStepBase.cs">
//   Copyright © 2018 Esbjörn Redmo and contributors. All rights reserved.
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

    public abstract class IfIndexerStepBase<TKey, TValue> : IndexerStepWithNext<TKey, TValue>
    {
        public sealed class IfBranchCaller : IIndexerStep<TKey, TValue>, ICanHaveNextIndexerStep<TKey, TValue>
        {
            private IIndexerStep<TKey, TValue> _nextStep = MissingIndexerStep<TKey, TValue>.Instance;

            TStep ICanHaveNextIndexerStep<TKey, TValue>.SetNextStep<TStep>(TStep step)
            {
                if (step == null)
                {
                    throw new ArgumentNullException(nameof(step));
                }

                _nextStep = step;
                return step;
            }

            TValue IIndexerStep<TKey, TValue>.Get(IMockInfo mockInfo, TKey key)
            {
                return _nextStep.Get(mockInfo, key);
            }

            void IIndexerStep<TKey, TValue>.Set(IMockInfo mockInfo, TKey key, TValue value)
            {
                _nextStep.Set(mockInfo, key, value);
            }

            public IIndexerStep<TKey, TValue> ElseBranch { get; }

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
                return _ifIndexerStep.NextStep.Get(mockInfo, key);
            }

            public void Set(IMockInfo mockInfo, TKey key, TValue value)
            {
                // Call directly to next step thus bypassing the condition check.
                _ifIndexerStep.NextStep.Set(mockInfo, key, value);
            }
        }

        protected IIndexerStep<TKey, TValue> IfBranch { get; }

        protected IfIndexerStepBase(Action<IfBranchCaller> branch)
        {
            var ifBranch = new IfBranchCaller(this);
            IfBranch = ifBranch;
            branch?.Invoke(ifBranch);
        }
    }
}
