// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IfPropertyStepBase.cs">
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

    public abstract class IfPropertyStepBase<TValue> : MedialPropertyStep<TValue>
    {
        public sealed class IfBranchCaller : IPropertyStep<TValue>, IPropertyStepCaller<TValue>
        {
            private IPropertyStep<TValue> _nextStep = MissingPropertyStep<TValue>.Instance;

            public TStep SetNextStep<TStep>(TStep step) where TStep : IPropertyStep<TValue>
            {
                if (step == null)
                {
                    throw new ArgumentNullException(nameof(step));
                }

                _nextStep = step;
                return step;
            }

            TValue IPropertyStep<TValue>.Get(object instance, MemberMock memberMock)
            {
                return _nextStep.Get(instance, memberMock);
            }

            void IPropertyStep<TValue>.Set(object instance, MemberMock memberMock, TValue value)
            {
                _nextStep.Set(instance, memberMock, value);
            }

            public IPropertyStep<TValue> ElseBranch { get; }

            public IfBranchCaller(IfPropertyStepBase<TValue> ifPropertyStep)
            {
                ElseBranch = new ElseBranchRejoiner(ifPropertyStep);
            }
        }

        private sealed class ElseBranchRejoiner : IPropertyStep<TValue>
        {
            private readonly IfPropertyStepBase<TValue> _ifPropertyStep;

            public ElseBranchRejoiner(IfPropertyStepBase<TValue> ifPropertyStep)
            {
                _ifPropertyStep = ifPropertyStep;
            }

            public TValue Get(object instance, MemberMock memberMock)
            {
                // Call directly to next step thus bypassing the condition check.
                return _ifPropertyStep.NextStep.Get(instance, memberMock);
            }

            public void Set(object instance, MemberMock memberMock, TValue value)
            {
                // Call directly to next step thus bypassing the condition check.
                _ifPropertyStep.NextStep.Set(instance, memberMock, value);
            }
        }

        protected IPropertyStep<TValue> IfBranch { get; }

        protected IfPropertyStepBase(Action<IfBranchCaller> branch)
        {
            var ifBranch = new IfBranchCaller(this);
            IfBranch = ifBranch;
            branch?.Invoke(ifBranch);
        }
    }
}
