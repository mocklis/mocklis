// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IfMethodStepBase.cs">
//   Copyright © 2018 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.Steps.Conditional
{
    #region Using Directives

    using System;
    using Mocklis.Core;

    #endregion

    public abstract class IfMethodStepBase<TParam, TResult> : MedialMethodStep<TParam, TResult>
    {
        private class Rejoiner : IMethodStep<TParam, TResult>
        {
            private readonly IfMethodStepBase<TParam, TResult> _step;

            public Rejoiner(IfMethodStepBase<TParam, TResult> step)
            {
                _step = step;
            }

            public TResult Call(object instance, MemberMock memberMock, TParam param)
            {
                // Call directly to next step thus bypassing the condition check.
                return _step.NextStep.Call(instance, memberMock, param);
            }
        }

        protected MedialMethodStep<TParam, TResult> IfBranch { get; }

        protected IfMethodStepBase(Action<IMethodStepCaller<TParam, TResult>, IMethodStep<TParam, TResult>> ifBranchSetup)
        {
            IfBranch = new MedialMethodStep<TParam, TResult>();
            ifBranchSetup(IfBranch, new Rejoiner(this));
        }
    }
}
