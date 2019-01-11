// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IStoredIndexer.cs">
//   Copyright © 2019 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.Verification
{
    public interface IStoredIndexer<in TKey, TValue>
    {
        TValue this[TKey key] { get; set; }
    }
}
