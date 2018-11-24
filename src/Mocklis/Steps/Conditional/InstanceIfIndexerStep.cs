// --------------------------------------------------------------------------------------------------------------------
// <copyright file="InstanceIfIndexerStep.cs">
//   Copyright © 2018 Esbjörn Redmo and contributors. All rights reserved.
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
            _getCondition = getCondition ?? throw new ArgumentNullException(nameof(getCondition));
            _setCondition = setCondition ?? throw new ArgumentNullException(nameof(setCondition));
        }

        public override TValue Get(MemberMock memberMock, TKey key)
        {
            return _getCondition(memberMock.MockInstance, key) ? IfBranch.Get(memberMock, key) : base.Get(memberMock, key);
        }

        public override void Set(MemberMock memberMock, TKey key, TValue value)
        {
            if (_setCondition(memberMock.MockInstance, key, value))
            {
                IfBranch.Set(memberMock, key, value);
            }
            else
            {
                base.Set(memberMock, key, value);
            }
        }
    }
}
