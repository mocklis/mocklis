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
        public IfGetIndexerStep(Action<IIndexerStepCaller<TKey, TValue>, IIndexerStep<TKey, TValue>> ifBranchSetup) : base(ifBranchSetup)
        {
        }

        public override TValue Get(object instance, MemberMock memberMock, TKey key)
        {
            return IfBranch.Get(instance, memberMock, key);
        }
    }
}
