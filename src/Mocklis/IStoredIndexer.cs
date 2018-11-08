// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IStoredIndexer.cs">
//   Copyright © 2018 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis
{
    public interface IStoredIndexer<in TKey, TValue>
    {
        TValue this[TKey key] { get; set; }
    }
}
