// --------------------------------------------------------------------------------------------------------------------
// <copyright file="InstanceIfIndexerStep.cs">
//   Copyright © 2018 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.Conditional
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
            Action<IIndexerStepCaller<TKey, TValue>, IIndexerStep<TKey, TValue>> ifBranchSetup) : base(ifBranchSetup)
        {
            _getCondition = getCondition;
            _setCondition = setCondition;
        }

        public override TValue Get(object instance, MemberMock memberMock, TKey key)
        {
            return _getCondition(instance, key) ? IfBranch.Get(instance, memberMock, key) : base.Get(instance, memberMock, key);
        }

        public override void Set(object instance, MemberMock memberMock, TKey key, TValue value)
        {
            if (_setCondition(instance, key, value))
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
