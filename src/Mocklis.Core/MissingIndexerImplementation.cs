// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MissingIndexerImplementation.cs">
//   Copyright © 2018 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.Core
{
    public sealed class MissingIndexerImplementation<TKey, TValue> : IIndexerImplementation<TKey, TValue>
    {
        public static readonly MissingIndexerImplementation<TKey, TValue> Instance = new MissingIndexerImplementation<TKey, TValue>();

        private MissingIndexerImplementation()
        {
        }

        public TValue Get(MemberMock memberMock, TKey key)
        {
            throw new MockMissingException(MockType.IndexerGet, memberMock.InterfaceName, memberMock.MemberName, memberMock.MemberMockName);
        }

        public void Set(MemberMock memberMock, TKey key, TValue value)
        {
            throw new MockMissingException(MockType.IndexerSet, memberMock.InterfaceName, memberMock.MemberName, memberMock.MemberMockName);
        }
    }
}
