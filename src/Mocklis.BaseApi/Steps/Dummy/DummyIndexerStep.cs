// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DummyIndexerStep.cs">
//   SPDX-License-Identifier: MIT
//   Copyright © 2019-2024 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.Steps.Dummy
{
    #region Using Directives

    using Mocklis.Core;

    #endregion

    /// <summary>
    ///     Class that represents a 'Dummy' indexer step. This class cannot be inherited.
    ///     Implements the <see cref="IIndexerStep{TKey,TValue}" /> interface.
    /// </summary>
    /// <typeparam name="TKey">The type of the indexer key.</typeparam>
    /// <typeparam name="TValue">The type of the indexer value.</typeparam>
    /// <seealso cref="IIndexerStep{TKey, TValue}" />
    public sealed class DummyIndexerStep<TKey, TValue> : IIndexerStep<TKey, TValue>
    {
        /// <summary>
        ///     The singleton <see cref="DummyIndexerStep{TKey, TValue}" /> instance for this type of mocked indexers.
        /// </summary>
        /// <remarks>
        ///     We can use a singleton for this step as it's both final and keeps no state.
        /// </remarks>
        public static readonly DummyIndexerStep<TKey, TValue> Instance = new DummyIndexerStep<TKey, TValue>();

        private DummyIndexerStep()
        {
        }

        /// <summary>
        ///     Called when a value is read from the indexer.
        ///     This implementation will return a default value.
        /// </summary>
        /// <param name="mockInfo">Information about the mock through which the value is read.</param>
        /// <param name="key">The indexer key used.</param>
        /// <returns>The value being read.</returns>
        public TValue Get(IMockInfo mockInfo, TKey key)
        {
            return default!;
        }

        /// <summary>
        ///     Called when a value is written to the indexer.
        ///     This implementation will do nothing.
        /// </summary>
        /// <param name="mockInfo">Information about the mock through which the value is written.</param>
        /// <param name="key">The indexer key used.</param>
        /// <param name="value">The value being written.</param>
        public void Set(IMockInfo mockInfo, TKey key, TValue value)
        {
        }
    }
}
