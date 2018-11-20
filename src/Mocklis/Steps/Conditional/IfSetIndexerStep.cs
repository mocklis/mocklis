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
        public IfSetIndexerStep(Action<IIndexerStepCaller<TKey, TValue>, IIndexerStep<TKey, TValue>> ifBranchSetup) : base(ifBranchSetup)
        {
        }

        public override void Set(object instance, MemberMock memberMock, TKey key, TValue value)
        {
            IfBranch.Set(instance, memberMock, key, value);
        }
    }
}
