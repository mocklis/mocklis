// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IfMethodStepBase.cs">
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

    public abstract class IfMethodStepBase<TParam, TResult> : MedialMethodStep<TParam, TResult>
    {
        public sealed class IfBranchCaller : IMethodStep<TParam, TResult>, IMethodStepCaller<TParam, TResult>
        {
            private IMethodStep<TParam, TResult> _nextStep = MissingMethodStep<TParam, TResult>.Instance;

            public TStep SetNextStep<TStep>(TStep step) where TStep : IMethodStep<TParam, TResult>
            {
                if (step == null)
                {
                    throw new ArgumentNullException(nameof(step));
                }

                _nextStep = step;
                return step;
            }

            TResult IMethodStep<TParam, TResult>.Call(object instance, MemberMock memberMock, TParam param)
            {
                return _nextStep.Call(instance, memberMock, param);
            }

            public IMethodStep<TParam, TResult> ElseBranch { get; }

            public IfBranchCaller(IfMethodStepBase<TParam, TResult> ifMethodStep)
            {
                ElseBranch = new ElseBranchRejoiner(ifMethodStep);
            }
        }

        private sealed class ElseBranchRejoiner : IMethodStep<TParam, TResult>
        {
            private readonly IfMethodStepBase<TParam, TResult> _ifMethodStep;

            public ElseBranchRejoiner(IfMethodStepBase<TParam, TResult> ifMethodStep)
            {
                _ifMethodStep = ifMethodStep;
            }

            public TResult Call(object instance, MemberMock memberMock, TParam param)
            {
                // Call directly to next step thus bypassing the condition check.
                return _ifMethodStep.NextStep.Call(instance, memberMock, param);
            }
        }

        protected IMethodStep<TParam, TResult> IfBranch { get; }

        protected IfMethodStepBase(Action<IfBranchCaller> branch)
        {
            var ifBranch = new IfBranchCaller(this);
            IfBranch = ifBranch;
            branch?.Invoke(ifBranch);
        }
    }
}
