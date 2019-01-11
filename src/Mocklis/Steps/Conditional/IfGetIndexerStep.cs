// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IfGetIndexerStep.cs">
//   Copyright © 2019 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.Steps.Conditional
{
    #region Using Directives

    using System;
    using Mocklis.Core;

    #endregion

    public class IfGetIndexerStep<TKey, TValue> : IfIndexerStepBase<TKey, TValue>
    {
        public IfGetIndexerStep(Action<IfBranchCaller> branch) : base(branch)
        {
        }

        public override TValue Get(IMockInfo mockInfo, TKey key)
        {
            return IfBranch.Get(mockInfo, key);
        }
    }
}
