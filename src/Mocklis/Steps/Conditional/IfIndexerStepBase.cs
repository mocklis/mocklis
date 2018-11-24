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

    public abstract class IfIndexerStepBase<TKey, TValue> : MedialIndexerStep<TKey, TValue>
    {
        public sealed class IfBranchCaller : IIndexerStep<TKey, TValue>, IIndexerStepCaller<TKey, TValue>
        {
            private IIndexerStep<TKey, TValue> _nextStep = MissingIndexerStep<TKey, TValue>.Instance;

            public TStep SetNextStep<TStep>(TStep step) where TStep : IIndexerStep<TKey, TValue>
            {
                if (step == null)
                {
                    throw new ArgumentNullException(nameof(step));
                }

                _nextStep = step;
                return step;
            }

            TValue IIndexerStep<TKey, TValue>.Get(MemberMock memberMock, TKey key)
            {
                return _nextStep.Get(memberMock, key);
            }

            void IIndexerStep<TKey, TValue>.Set(MemberMock memberMock, TKey key, TValue value)
            {
                _nextStep.Set(memberMock, key, value);
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

            public TValue Get(MemberMock memberMock, TKey key)
            {
                // Call directly to next step thus bypassing the condition check.
                return _ifIndexerStep.NextStep.Get(memberMock, key);
            }

            public void Set(MemberMock memberMock, TKey key, TValue value)
            {
                // Call directly to next step thus bypassing the condition check.
                _ifIndexerStep.NextStep.Set(memberMock, key, value);
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
