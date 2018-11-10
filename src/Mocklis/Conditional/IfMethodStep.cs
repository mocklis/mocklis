// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IfMethodStep.cs">
//   Copyright © 2018 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.Conditional
{
    #region Using Directives

    using System;
    using Mocklis.Core;

    #endregion

    public class IfMethodStep<TParam, TResult> : MedialMethodStep<TParam, TResult>
    {
        private class Rejoiner : IMethodStep<TParam, TResult>
        {
            private readonly IfMethodStep<TParam, TResult> _step;

            public Rejoiner(IfMethodStep<TParam, TResult> step)
            {
                _step = step;
            }

            public TResult Call(object instance, MemberMock memberMock, TParam param)
            {
                // Call directly to next step thus bypassing the condition check.
                return _step.NextStep.Call(instance, memberMock, param);
            }
        }

        private readonly Func<TParam, bool> _condition;
        private readonly MedialMethodStep<TParam, TResult> _branch = new MedialMethodStep<TParam, TResult>();

        public IfMethodStep(Func<TParam, bool> condition, Action<IMethodStepCaller<TParam, TResult>, IMethodStep<TParam, TResult>> ifBranch)
        {
            _condition = condition;
            ifBranch(_branch, new Rejoiner(this));
        }

        public override TResult Call(object instance, MemberMock memberMock, TParam param)
        {
            return _condition(param) ? _branch.Call(instance, memberMock, param) : base.Call(instance, memberMock, param);
        }
    }
}
