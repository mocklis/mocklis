// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ReturnIndexerStep.cs">
//   Copyright © 2019 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.Steps.Return
{
    #region Using Directives

    using Mocklis.Core;

    #endregion

    public class ReturnIndexerStep<TKey, TValue> : IndexerStepWithNext<TKey, TValue>
    {
        private readonly TValue _value;

        public ReturnIndexerStep(TValue value)
        {
            _value = value;
        }

        public override TValue Get(IMockInfo mockInfo, TKey key)
        {
            return _value;
        }
    }
}
