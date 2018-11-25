// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IIndexerStep.cs">
//   Copyright © 2018 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.Core
{
    public interface IIndexerStep<in TKey, TValue>
    {
        TValue Get(IMockInfo mockInfo, TKey key);
        void Set(IMockInfo mockInfo, TKey key, TValue value);
    }
}
