// --------------------------------------------------------------------------------------------------------------------
// <copyright file="InstanceIfPropertyStep.cs">
//   Copyright © 2019 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.Steps.Conditional
{
    #region Using Directives

    using System;
    using Mocklis.Core;

    #endregion

    public class InstanceIfPropertyStep<TValue> : IfPropertyStepBase<TValue>
    {
        private readonly Func<object, bool> _getCondition;
        private readonly Func<object, TValue, bool> _setCondition;

        public InstanceIfPropertyStep(Func<object, bool> getCondition, Func<object, TValue, bool> setCondition,
            Action<IfBranchCaller> branch) : base(branch)
        {
            _getCondition = getCondition;
            _setCondition = setCondition;
        }

        public override TValue Get(IMockInfo mockInfo)
        {
            if (_getCondition?.Invoke(mockInfo.MockInstance) ?? false)
            {
                return IfBranch.Get(mockInfo);
            }

            return base.Get(mockInfo);
        }

        public override void Set(IMockInfo mockInfo, TValue value)
        {
            if (_setCondition?.Invoke(mockInfo.MockInstance, value) ?? false)
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
