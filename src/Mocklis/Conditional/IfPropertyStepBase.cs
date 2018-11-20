// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IfPropertyStepBase.cs">
//   Copyright © 2018 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.Conditional
{
    #region Using Directives

    using System;
    using Mocklis.Core;

    #endregion

    public abstract class IfPropertyStepBase<TValue> : MedialPropertyStep<TValue>
    {
        private class Rejoiner : IPropertyStep<TValue>
        {
            private readonly IfPropertyStepBase<TValue> _step;

            public Rejoiner(IfPropertyStepBase<TValue> step)
            {
                _step = step;
            }

            public TValue Get(object instance, MemberMock memberMock)
            {
                // Call directly to next step thus bypassing the condition check.
                return _step.NextStep.Get(instance, memberMock);
            }

            public void Set(object instance, MemberMock memberMock, TValue value)
            {
                // Call directly to next step thus bypassing the condition check.
                _step.NextStep.Set(instance, memberMock, value);
            }
        }

        protected MedialPropertyStep<TValue> IfBranch { get; }

        protected IfPropertyStepBase(Action<IPropertyStepCaller<TValue>, IPropertyStep<TValue>> ifBranchSetup)
        {
            IfBranch = new MedialPropertyStep<TValue>();
            ifBranchSetup(IfBranch, new Rejoiner(this));
        }
    }
}
