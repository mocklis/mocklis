// --------------------------------------------------------------------------------------------------------------------
// <copyright file="InstanceIfIndexerStep.cs">
//   Copyright © 2019 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.Steps.Conditional
{
    #region Using Directives

    using System;
    using Mocklis.Core;

    #endregion

    public class InstanceIfIndexerStep<TKey, TValue> : IfIndexerStepBase<TKey, TValue>
    {
        private readonly Func<object, TKey, bool> _getCondition;
        private readonly Func<object, TKey, TValue, bool> _setCondition;

        public InstanceIfIndexerStep(Func<object, TKey, bool> getCondition, Func<object, TKey, TValue, bool> setCondition,
            Action<IfBranchCaller> branch) : base(branch)
        {
            _getCondition = getCondition;
            _setCondition = setCondition;
        }

        public override TValue Get(IMockInfo mockInfo, TKey key)
        {
            if (_getCondition?.Invoke(mockInfo.MockInstance, key) ?? false)
            {
                return IfBranch.Get(mockInfo, key);
            }

            return base.Get(mockInfo, key);
        }

        public override void Set(IMockInfo mockInfo, TKey key, TValue value)
        {
            if (_setCondition?.Invoke(mockInfo.MockInstance, key, value) ?? false)
            {
                IfBranch.Set(mockInfo, key, value);
            }
            else
            {
                base.Set(mockInfo, key, value);
            }
        }
    }
}
