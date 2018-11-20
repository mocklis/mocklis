// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IfSetPropertyStep.cs">
//   Copyright © 2018 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.Conditional
{
    #region Using Directives

    using System;
    using Mocklis.Core;

    #endregion

    public class IfSetPropertyStep<TValue> : IfPropertyStepBase<TValue>
    {
        public IfSetPropertyStep(Action<IPropertyStepCaller<TValue>, IPropertyStep<TValue>> ifBranchSetup) :
            base(ifBranchSetup)
        {
        }

        public override void Set(object instance, MemberMock memberMock, TValue value)
        {
            IfBranch.Set(instance, memberMock, value);
        }
    }
}
