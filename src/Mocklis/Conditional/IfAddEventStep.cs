// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IfAddEventStep.cs">
//   Copyright © 2018 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.Conditional
{
    #region Using Directives

    using System;
    using Mocklis.Core;

    #endregion

    public class IfAddEventStep<THandler> : IfEventStepBase<THandler> where THandler : Delegate
    {
        public IfAddEventStep(Action<IEventStepCaller<THandler>, IEventStep<THandler>> ifBranchRemoveup) :
            base(ifBranchRemoveup)
        {
        }

        public override void Add(object instance, MemberMock memberMock, THandler value)
        {
            IfBranch.Add(instance, memberMock, value);
        }
    }
}
