// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OnlySetIfChangedPropertyStep.cs">
//   Copyright © 2018 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.Steps.Conditional
{
    #region Using Directives

    using System.Collections.Generic;
    using Mocklis.Core;

    #endregion

    public class OnlySetIfChangedPropertyStep<TValue> : MedialPropertyStep<TValue>
    {
        private IEqualityComparer<TValue> Comparer { get; }

        public OnlySetIfChangedPropertyStep(IEqualityComparer<TValue> comparer = null)
        {
            Comparer = comparer ?? EqualityComparer<TValue>.Default;
        }

        public override void Set(MemberMock memberMock, TValue value)
        {
            if (!Comparer.Equals(NextStep.Get(memberMock), value))
            {
                base.Set(memberMock, value);
            }
        }
    }
}
