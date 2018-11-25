// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DummyIndexerStep.cs">
//   Copyright © 2018 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.Steps.Dummy
{
    #region Using Directives

    using Mocklis.Core;

    #endregion

    public class DummyIndexerStep<TKey, TValue> : IIndexerStep<TKey, TValue>
    {
        public static readonly DummyIndexerStep<TKey, TValue> Instance = new DummyIndexerStep<TKey, TValue>();

        private DummyIndexerStep()
        {
        }

        public TValue Get(IMockInfo mockInfo, TKey key)
        {
            return default;
        }

        public void Set(IMockInfo mockInfo, TKey key, TValue value)
        {
        }
    }
}
