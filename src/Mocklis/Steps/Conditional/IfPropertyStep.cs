// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IfPropertyStep.cs">
//   Copyright © 2018 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.Steps.Conditional
{
    #region Using Directives

    using System;
    using Mocklis.Core;

    #endregion

    public class IfPropertyStep<TValue> : IfPropertyStepBase<TValue>
    {
        private readonly Func<bool> _getCondition;
        private readonly Func<TValue, bool> _setCondition;

        public IfPropertyStep(Func<bool> getCondition, Func<TValue, bool> setCondition,
            Action<IfBranchCaller> branch) : base(branch)
        {
            _getCondition = getCondition;
            _setCondition = setCondition;
        }

        public override TValue Get(IMockInfo mockInfo)
        {
            if (_getCondition?.Invoke() ?? false)
            {
                return IfBranch.Get(mockInfo);
            }

            return base.Get(mockInfo);
        }

        public override void Set(IMockInfo mockInfo, TValue value)
        {
            if (_setCondition?.Invoke(value) ?? false)
            {
                IfBranch.Set(mockInfo, value);
            }
            else
            {
                base.Set(mockInfo, value);
            }
        }
    }
}
