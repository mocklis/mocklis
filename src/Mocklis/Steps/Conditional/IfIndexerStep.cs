// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IfIndexerStep.cs">
//   Copyright © 2018 Esbjörn Redmo and contributors. All rights reserved.
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
            _getCondition = getCondition ?? throw new ArgumentNullException(nameof(getCondition));
            _setCondition = setCondition ?? throw new ArgumentNullException(nameof(setCondition));
        }

        public override TValue Get(object instance, MemberMock memberMock, TKey key)
        {
            return _getCondition(key) ? IfBranch.Get(instance, memberMock, key) : base.Get(instance, memberMock, key);
        }

        public override void Set(object instance, MemberMock memberMock, TKey key, TValue value)
        {
            if (_setCondition(key, value))
            {
                IfBranch.Set(instance, memberMock, key, value);
            }
            else
            {
                base.Set(instance, memberMock, key, value);
            }
        }
    }
}
