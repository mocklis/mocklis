// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IfSetIndexerStep.cs">
//   Copyright © 2018 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.Steps.Conditional
{
    #region Using Directives

    using System;
    using Mocklis.Core;

    #endregion

    public class IfSetIndexerStep<TKey, TValue> : IfIndexerStepBase<TKey, TValue>
    {
        public IfSetIndexerStep(Action<IfBranchCaller> branch) : base(branch)
        {
        }

        public override void Set(MemberMock memberMock, TKey key, TValue value)
        {
            IfBranch.Set(memberMock, key, value);
        }
    }
}