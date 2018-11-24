// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IIndexerStep.cs">
//   Copyright © 2018 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.Core
{
    public interface IIndexerStep<in TKey, TValue>
    {
        TValue Get(MemberMock memberMock, TKey key);
        void Set(MemberMock memberMock, TKey key, TValue value);
    }
}
