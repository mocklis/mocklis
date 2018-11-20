// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OnlySetIfChangedPropertyStep.cs">
//   Copyright © 2018 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.Conditional
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

        public override void Set(object instance, MemberMock memberMock, TValue value)
        {
            if (!Comparer.Equals(NextStep.Get(instance, memberMock), value))
            {
                base.Set(instance, memberMock, value);
            }
        }
    }
}
