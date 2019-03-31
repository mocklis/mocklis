// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StoredAsDictionaryIndexerStep.cs">
//   SPDX-License-Identifier: MIT
//   Copyright © 2019 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.Steps.Stored
{
    #region Using Directives

    using System.Collections.Generic;
    using Mocklis.Core;
    using Mocklis.Verification;

    #endregion

    /// <summary>
    ///     Class that represents a 'Stored' index step that uses a dictionary as backing store.
    ///     Implements the <see cref="IIndexerStep{TKey,TValue}" /> interface.
    ///     Implements the <see cref="IStoredIndexer{TKey,TValue}" /> interface.
    /// </summary>
    /// <typeparam name="TKey">The type of the indexer key.</typeparam>
    /// <typeparam name="TValue">The type of the indexer value.</typeparam>
    /// <seealso cref="IIndexerStep{TKey, TValue}" />
    /// <seealso cref="IStoredIndexer{TKey, TValue}" />
    public class StoredAsDictionaryIndexerStep<TKey, TValue> : IIndexerStep<TKey, TValue>, IStoredIndexer<TKey, TValue>
    {
        private readonly Dictionary<TKey, TValue> _dictionary = new Dictionary<TKey, TValue>();

        /// <summary>
        ///     Gets or sets the <typeparamref name="TValue" /> with the specified key.
        /// </summary>
        /// <param name="key">The key used.</param>
        /// <returns>the <typeparamref name="TValue" /> read or written.</returns>
        public TValue this[TKey key]
        {
            get => _dictionary.ContainsKey(key) ? _dictionary[key] : default;
            set => _dictionary[key] = value;
        }

        /// <summary>
        ///     Gets the underlying dictionary for the store.
        /// </summary>
        public IDictionary<TKey, TValue> Dictionary => _dictionary;

        /// <summary>
        ///     Called when a value is read from the indexer.
        /// </summary>
        /// <param name="mockInfo">Information about the mock through which the value is read.</param>
        /// <param name="key">The indexer key used.</param>
        /// <returns>The value being read.</returns>
        public TValue Get(IMockInfo mockInfo, TKey key) => this[key];

        /// <summary>
        ///     Called when a value is written to the indexer.
        /// </summary>
        /// <param name="mockInfo">Information about the mock through which the value is written.</param>
        /// <param name="key">The indexer key used.</param>
        /// <param name="value">The value being written.</param>
        public void Set(IMockInfo mockInfo, TKey key, TValue value)
        {
            this[key] = value;
        }
    }
}
