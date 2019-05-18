// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IStoredIndexer.cs">
//   SPDX-License-Identifier: MIT
//   Copyright © 2019 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.Core
{
    /// <summary>
    ///     Interface that provides access to stored values in a 'stored' indexer step by key.
    /// </summary>
    /// <typeparam name="TKey">The type of the indexer key.</typeparam>
    /// <typeparam name="TValue">The type of the indexer value.</typeparam>
    public interface IStoredIndexer<in TKey, out TValue>
    {
        /// <summary>
        ///     Gets the <typeparamref name="TValue" /> with the specified key.
        /// </summary>
        /// <param name="key">The <typeparamref name="TKey" /> to use.</param>
        /// <returns>The <typeparamref name="TValue" /> stored by the given key.</returns>
        TValue this[TKey key] { get; }
    }
}
