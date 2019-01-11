// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MissingIndexerStep.cs">
//   Copyright © 2019 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.Steps.Missing
{
    #region Using Directives

    using Mocklis.Core;

    #endregion

    public sealed class MissingIndexerStep<TKey, TValue> : IIndexerStep<TKey, TValue>
    {
        public static readonly MissingIndexerStep<TKey, TValue> Instance = new MissingIndexerStep<TKey, TValue>();

        private MissingIndexerStep()
        {
        }

        public TValue Get(IMockInfo mockInfo, TKey key)
        {
            throw new MockMissingException(MockType.IndexerGet, mockInfo);
        }

        public void Set(IMockInfo mockInfo, TKey key, TValue value)
        {
            throw new MockMissingException(MockType.IndexerSet, mockInfo);
        }
    }
}
