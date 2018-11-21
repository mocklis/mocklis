// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IfRemoveEventStep.cs">
//   Copyright © 2018 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.Steps.Conditional
{
    #region Using Directives

    using System;
    using Mocklis.Core;

    #endregion

    public class IfRemoveEventStep<THandler> : IfEventStepBase<THandler> where THandler : Delegate
    {
        public IfRemoveEventStep(Action<IfBranchCaller> branch) :
            base(branch)
        {
        }

        public override void Remove(object instance, MemberMock memberMock, THandler value)
        {
            IfBranch.Remove(instance, memberMock, value);
        }
    }
}
