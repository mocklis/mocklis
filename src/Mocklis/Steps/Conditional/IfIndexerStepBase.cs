// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IfIndexerStepBase.cs">
//   Copyright © 2018 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.Steps.Conditional
{
    #region Using Directives

    using System;
    using Mocklis.Core;

    #endregion

    public abstract class IfIndexerStepBase<TKey, TValue> : MedialIndexerStep<TKey, TValue>
    {
        private class Rejoiner : IIndexerStep<TKey, TValue>
        {
            private readonly IfIndexerStepBase<TKey, TValue> _step;

            public Rejoiner(IfIndexerStepBase<TKey, TValue> step)
            {
                _step = step;
            }

            public TValue Get(object instance, MemberMock memberMock, TKey key)
            {
                // Call directly to next step thus bypassing the condition check.
                return _step.NextStep.Get(instance, memberMock, key);
            }

            public void Set(object instance, MemberMock memberMock, TKey key, TValue value)
            {
                // Call directly to next step thus bypassing the condition check.
                _step.NextStep.Set(instance, memberMock, key, value);
            }
        }

        protected MedialIndexerStep<TKey, TValue> IfBranch { get; }

        protected IfIndexerStepBase(Action<IIndexerStepCaller<TKey, TValue>, IIndexerStep<TKey, TValue>> ifBranchSetup)
        {
            IfBranch = new MedialIndexerStep<TKey, TValue>();
            ifBranchSetup(IfBranch, new Rejoiner(this));
        }
    }
}
