// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MissingIndexerStep.cs">
//   Copyright © 2018 Esbjörn Redmo and contributors. All rights reserved.
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

        public TValue Get(MemberMock memberMock, TKey key)
        {
            throw new MockMissingException(MockType.IndexerGet, memberMock);
        }

        public void Set(MemberMock memberMock, TKey key, TValue value)
        {
            throw new MockMissingException(MockType.IndexerSet, memberMock);
        }
    }
}
