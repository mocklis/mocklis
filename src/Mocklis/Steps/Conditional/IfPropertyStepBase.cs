// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IfPropertyStepBase.cs">
//   Copyright © 2019 Esbjörn Redmo and contributors. All rights reserved.
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

    public abstract class IfPropertyStepBase<TValue> : PropertyStepWithNext<TValue>
    {
        public sealed class IfBranchCaller : IPropertyStep<TValue>, ICanHaveNextPropertyStep<TValue>
        {
            private IPropertyStep<TValue> _nextStep = MissingPropertyStep<TValue>.Instance;

            TStep ICanHaveNextPropertyStep<TValue>.SetNextStep<TStep>(TStep step)
            {
                if (step == null)
                {
                    throw new ArgumentNullException(nameof(step));
                }

                _nextStep = step;
                return step;
            }

            TValue IPropertyStep<TValue>.Get(IMockInfo mockInfo)
            {
                return _nextStep.Get(mockInfo);
            }

            void IPropertyStep<TValue>.Set(IMockInfo mockInfo, TValue value)
            {
                _nextStep.Set(mockInfo, value);
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

            public TValue Get(IMockInfo mockInfo)
            {
                // Call directly to next step thus bypassing the condition check.
                return _ifPropertyStep.NextStep.Get(mockInfo);
            }

            public void Set(IMockInfo mockInfo, TValue value)
            {
                // Call directly to next step thus bypassing the condition check.
                _ifPropertyStep.NextStep.Set(mockInfo, value);
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
