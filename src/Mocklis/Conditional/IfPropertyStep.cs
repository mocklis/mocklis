// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IfPropertyStep.cs">
//   Copyright © 2018 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.Conditional
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
            Action<IPropertyStepCaller<TValue>, IPropertyStep<TValue>> ifBranchSetup) :
            base(ifBranchSetup)
        {
            _getCondition = getCondition;
            _setCondition = setCondition;
        }

        public override TValue Get(object instance, MemberMock memberMock)
        {
            return _getCondition() ? IfBranch.Get(instance, memberMock) : base.Get(instance, memberMock);
        }

        public override void Set(object instance, MemberMock memberMock, TValue value)
        {
            if (_setCondition(value))
            {
                IfBranch.Set(instance, memberMock, value);
            }
            else
            {
                base.Set(instance, memberMock, value);
            }
        }
    }
}
