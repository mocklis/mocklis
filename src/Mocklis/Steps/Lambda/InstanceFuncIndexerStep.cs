// --------------------------------------------------------------------------------------------------------------------
// <copyright file="InstanceFuncIndexerStep.cs">
//   Copyright © 2018 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.Steps.Lambda
{
    #region Using Directives

    using System;
    using Mocklis.Core;

    #endregion

    public class InstanceFuncIndexerStep<TKey, TValue> : IIndexerStep<TKey, TValue>
    {
        private readonly Func<object, TKey, TValue> _func;

        public InstanceFuncIndexerStep(Func<object, TKey, TValue> func)
        {
            _func = func;
        }

        public TValue Get(IMockInfo mockInfo, TKey key)
        {
            return _func(mockInfo.MockInstance, key);
        }

        public void Set(IMockInfo mockInfo, TKey key, TValue value)
        {
        }
    }
}
