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
            Action<IPropertyStepCaller<TValue>, IPropertyStep<TValue>> ifBranchSetup) :
            base(ifBranchSetup)
        {
            _getCondition = getCondition;
            _setCondition = setCondition;
        }

        public override TValue Get(object instance, MemberMock memberMock)
        {
            return _getCondition(instance) ? IfBranch.Get(instance, memberMock) : base.Get(instance, memberMock);
        }

        public override void Set(object instance, MemberMock memberMock, TValue value)
        {
            if (_setCondition(instance, value))
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
