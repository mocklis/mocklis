// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ReturnIndexerStep.cs">
//   Copyright © 2018 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.Steps.Return
{
    #region Using Directives

    using Mocklis.Core;

    #endregion

    public class ReturnIndexerStep<TKey, TValue> : IIndexerStep<TKey, TValue>
    {
        private readonly TValue _value;

        public ReturnIndexerStep(TValue value)
        {
            _value = value;
        }

        public TValue Get(IMockInfo mockInfo, TKey key)
        {
            return _value;
        }

        public void Set(IMockInfo mockInfo, TKey key, TValue value)
        {
        }
    }
}
