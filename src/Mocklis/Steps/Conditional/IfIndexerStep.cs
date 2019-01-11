// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IfIndexerStep.cs">
//   Copyright © 2019 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.Steps.Conditional
{
    #region Using Directives

    using System;
    using Mocklis.Core;

    #endregion

    public class IfIndexerStep<TKey, TValue> : IfIndexerStepBase<TKey, TValue>
    {
        private readonly Func<TKey, bool> _getCondition;
        private readonly Func<TKey, TValue, bool> _setCondition;

        public IfIndexerStep(Func<TKey, bool> getCondition, Func<TKey, TValue, bool> setCondition,
            Action<IfBranchCaller> branch) : base(branch)
        {
            _getCondition = getCondition;
            _setCondition = setCondition;
        }

        public override TValue Get(IMockInfo mockInfo, TKey key)
        {
            if (_getCondition?.Invoke(key) ?? false)
            {
                return IfBranch.Get(mockInfo, key);
            }

            return base.Get(mockInfo, key);
        }

        public override void Set(IMockInfo mockInfo, TKey key, TValue value)
        {
            if (_setCondition?.Invoke(key, value) ?? false)
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
