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
            _getCondition = getCondition ?? throw new ArgumentNullException(nameof(getCondition));
            _setCondition = setCondition ?? throw new ArgumentNullException(nameof(setCondition));
        }

        public override TValue Get(IMockInfo mockInfo)
        {
            return _getCondition() ? IfBranch.Get(mockInfo) : base.Get(mockInfo);
        }

        public override void Set(IMockInfo mockInfo, TValue value)
        {
            if (_setCondition(value))
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
