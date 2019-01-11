// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IfSetPropertyStep.cs">
//   Copyright © 2019 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.Steps.Conditional
{
    #region Using Directives

    using System;
    using Mocklis.Core;

    #endregion

    public class IfSetPropertyStep<TValue> : IfPropertyStepBase<TValue>
    {
        public IfSetPropertyStep(Action<IfBranchCaller> branch) : base(branch)
        {
        }

        public override void Set(IMockInfo mockInfo, TValue value)
        {
            IfBranch.Set(mockInfo, value);
        }
    }
}
