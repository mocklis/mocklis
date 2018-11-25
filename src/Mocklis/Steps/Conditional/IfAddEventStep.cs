// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IfAddEventStep.cs">
//   Copyright © 2018 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.Steps.Conditional
{
    #region Using Directives

    using System;
    using Mocklis.Core;

    #endregion

    public class IfAddEventStep<THandler> : IfEventStepBase<THandler> where THandler : Delegate
    {
        public IfAddEventStep(Action<IfBranchCaller> branch) :
            base(branch)
        {
        }

        public override void Add(IMockInfo mockInfo, THandler value)
        {
            IfBranch.Add(mockInfo, value);
        }
    }
}
