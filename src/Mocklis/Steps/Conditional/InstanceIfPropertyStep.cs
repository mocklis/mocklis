// --------------------------------------------------------------------------------------------------------------------
// <copyright file="InstanceIfPropertyStep.cs">
//   Copyright © 2018 Esbjörn Redmo and contributors. All rights reserved.
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
            _getCondition = getCondition ?? throw new ArgumentNullException(nameof(getCondition));
            _setCondition = setCondition ?? throw new ArgumentNullException(nameof(setCondition));
        }

        public override TValue Get(IMockInfo mockInfo)
        {
            return _getCondition(mockInfo.MockInstance) ? IfBranch.Get(mockInfo) : base.Get(mockInfo);
        }

        public override void Set(IMockInfo mockInfo, TValue value)
        {
            if (_setCondition(mockInfo.MockInstance, value))
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
