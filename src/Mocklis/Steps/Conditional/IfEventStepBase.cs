// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IfEventStepBase.cs">
//   Copyright © 2018 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.Steps.Conditional
{
    #region Using Directives

    using System;
    using Mocklis.Core;

    #endregion

    public abstract class IfEventStepBase<THandler> : MedialEventStep<THandler> where THandler : Delegate
    {
        private class Rejoiner : IEventStep<THandler>
        {
            private readonly IfEventStepBase<THandler> _step;

            public Rejoiner(IfEventStepBase<THandler> step)
            {
                _step = step;
            }

            public void Add(object instance, MemberMock memberMock, THandler value)
            {
                // Call directly to next step thus bypassing the condition check.
                _step.NextStep.Add(instance, memberMock, value);
            }

            public void Remove(object instance, MemberMock memberMock, THandler value)
            {
                // Call directly to next step thus bypassing the condition check.
                _step.NextStep.Remove(instance, memberMock, value);
            }
        }

        protected MedialEventStep<THandler> IfBranch { get; }

        protected IfEventStepBase(Action<IEventStepCaller<THandler>, IEventStep<THandler>> ifBranchSetup)
        {
            IfBranch = new MedialEventStep<THandler>();
            ifBranchSetup?.Invoke(IfBranch, new Rejoiner(this));
        }
    }
}
