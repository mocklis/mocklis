// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IfGetIndexerStep.cs">
//   Copyright © 2018 Esbjörn Redmo and contributors. All rights reserved.
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

        public override TValue Get(MemberMock memberMock, TKey key)
        {
            return IfBranch.Get(memberMock, key);
        }
    }
}
