// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IIndexerStep.cs">
//   Copyright © 2018 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.Core
{
    public interface IIndexerStep<in TKey, TValue>
    {
        TValue Get(object instance, MemberMock memberMock, TKey key);
        void Set(object instance, MemberMock memberMock, TKey key, TValue value);
    }
}
