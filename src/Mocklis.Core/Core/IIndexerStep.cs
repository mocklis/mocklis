// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IIndexerStep.cs">
//   SPDX-License-Identifier: MIT
//   Copyright © 2019-2024 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.Core
{
    /// <summary>
    ///     Interface that models things that can happen with a mocked indexer.
    /// </summary>
    /// <typeparam name="TKey">The type of the indexer key.</typeparam>
    /// <typeparam name="TValue">The type of the indexer value.</typeparam>
    public interface IIndexerStep<in TKey, TValue>
    {
        /// <summary>
        ///     Called when a value is read from the indexer.
        /// </summary>
        /// <param name="mockInfo">Information about the mock through which the value is read.</param>
        /// <param name="key">The indexer key used.</param>
        /// <returns>The value being read.</returns>
        TValue Get(IMockInfo mockInfo, TKey key);

        /// <summary>
        ///     Called when a value is written to the indexer.
        /// </summary>
        /// <param name="mockInfo">Information about the mock through which the value is written.</param>
        /// <param name="key">The indexer key used.</param>
        /// <param name="value">The value being written.</param>
        void Set(IMockInfo mockInfo, TKey key, TValue value);
    }
}
