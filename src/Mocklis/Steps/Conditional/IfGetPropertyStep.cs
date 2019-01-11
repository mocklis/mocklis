// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IfGetPropertyStep.cs">
//   Copyright © 2019 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.Steps.Conditional
{
    #region Using Directives

    using System;
    using Mocklis.Core;

    #endregion

    public class IfGetPropertyStep<TValue> : IfPropertyStepBase<TValue>
    {
        public IfGetPropertyStep(Action<IfBranchCaller> branch) : base(branch)
        {
        }

        public override TValue Get(IMockInfo mockInfo)
        {
            return IfBranch.Get(mockInfo);
        }
    }
}
