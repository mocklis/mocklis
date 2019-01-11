// --------------------------------------------------------------------------------------------------------------------
// <copyright file="InstanceFuncIndexerStep.cs">
//   Copyright © 2019 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.Steps.Lambda
{
    #region Using Directives

    using System;
    using Mocklis.Core;

    #endregion

    public class InstanceFuncIndexerStep<TKey, TValue> : IndexerStepWithNext<TKey, TValue>
    {
        private readonly Func<object, TKey, TValue> _func;

        public InstanceFuncIndexerStep(Func<object, TKey, TValue> func)
        {
            _func = func;
        }

        public override TValue Get(IMockInfo mockInfo, TKey key)
        {
            return _func(mockInfo.MockInstance, key);
        }
    }
}
