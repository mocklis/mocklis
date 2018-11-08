// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MissingIndexerStep.cs">
//   Copyright © 2018 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.Core
{
    public sealed class MissingIndexerStep<TKey, TValue> : IIndexerStep<TKey, TValue>, IFinalStep
    {
        public static readonly MissingIndexerStep<TKey, TValue> Instance = new MissingIndexerStep<TKey, TValue>();

        private MissingIndexerStep()
        {
        }

        public TValue Get(object instance, MemberMock memberMock, TKey key)
        {
            throw new MockMissingException(MockType.IndexerGet, memberMock.InterfaceName, memberMock.MemberName, memberMock.MemberMockName);
        }

        public void Set(object instance, MemberMock memberMock, TKey key, TValue value)
        {
            throw new MockMissingException(MockType.IndexerSet, memberMock.InterfaceName, memberMock.MemberName, memberMock.MemberMockName);
        }
    }
}
