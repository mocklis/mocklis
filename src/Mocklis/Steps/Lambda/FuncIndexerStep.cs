// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FuncIndexerStep.cs">
//   Copyright © 2018 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.Steps.Lambda
{
    #region Using Directives

    using System;
    using Mocklis.Core;

    #endregion

    public class FuncIndexerStep<TKey, TValue> : IndexerStepWithNext<TKey, TValue>
    {
        private readonly Func<TKey, TValue> _func;

        public FuncIndexerStep(Func<TKey, TValue> func)
        {
            _func = func;
        }

        public override TValue Get(IMockInfo mockInfo, TKey key)
        {
            return _func(key);
        }
    }
}
