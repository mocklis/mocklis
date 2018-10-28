// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IIndexerImplementation.cs">
//   Copyright © 2018 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.Core
{
    public interface IIndexerImplementation<in TKey, TValue>
    {
        TValue Get(MemberMock memberMock, TKey key);
        void Set(MemberMock memberMock, TKey key, TValue value);
    }
}
