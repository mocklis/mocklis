// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IStoredIndexer.cs">
//   Copyright © 2019 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.Verification
{
    /// <summary>
    ///     Interface that provides access to stored values in a 'stored' indexer step by key.
    /// </summary>
    /// <typeparam name="TKey">The type of the indexer key.</typeparam>
    /// <typeparam name="TValue">The type of the indexer value.</typeparam>
    public interface IStoredIndexer<in TKey, TValue>
    {
        /// <summary>
        ///     Gets or sets the <typeparamref name="TValue" /> with the specified key.
        /// </summary>
        /// <param name="key">The <typeparamref name="TKey" /> to use.</param>
        /// <returns>The <typeparamref name="TValue" /> to get or set.</returns>
        TValue this[TKey key] { get; set; }
    }
}
