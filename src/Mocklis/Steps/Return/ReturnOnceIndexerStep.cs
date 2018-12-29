// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ReturnOnceIndexerStep.cs">
//   Copyright © 2018 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.Steps.Return
{
    #region Using Directives

    using System.Threading;
    using Mocklis.Core;

    #endregion

    public class ReturnOnceIndexerStep<TKey, TValue> : IndexerStepWithNext<TKey, TValue>
    {
        private readonly TValue _value;
        private int _returnCount;

        public ReturnOnceIndexerStep(TValue value)
        {
            _value = value;
        }

        public override TValue Get(IMockInfo mockInfo, TKey key)
        {
            if (Interlocked.Exchange(ref _returnCount, 1) == 0)
            {
                return _value;
            }

            return base.Get(mockInfo, key);
        }
    }
}
