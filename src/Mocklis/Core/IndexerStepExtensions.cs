// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IndexerStepExtensions.cs">
//   SPDX-License-Identifier: MIT
//   Copyright © 2019 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.Core
{
    /// <summary>
    ///     Extension methods for the IIndexerStep interface.
    /// </summary>
    public static class IndexerStepExtensions
    {
        /// <summary>
        ///     Reads a value using an indexer step. If the step is <c>null</c>, uses the strictness of the mock
        ///     to decide whether to throw a <see cref="MockMissingException" /> (VeryStrict) or to return a
        ///     default value (Lenient or Strict).
        /// </summary>
        /// <typeparam name="TKey">The type of the indexer key.</typeparam>
        /// <typeparam name="TValue">The type of the indexer value.</typeparam>
        /// <param name="indexerStep">The indexer step (can be null) through which the value is read.</param>
        /// <param name="mockInfo">Information about the mock through which the value is read.</param>
        /// <param name="key">The indexer key used.</param>
        /// <returns>The value being read.</returns>
        /// <seealso cref="IIndexerStep{TKey, TValue}" />
        public static TValue GetWithStrictnessCheckIfNull<TKey, TValue>(this IIndexerStep<TKey, TValue> indexerStep, IMockInfo mockInfo, TKey key)
        {
            if (indexerStep == null)
            {
                if (mockInfo.Strictness != Strictness.VeryStrict)
                {
                    return default;
                }

                throw new MockMissingException(MockType.IndexerGet, mockInfo);
            }

            return indexerStep.Get(mockInfo, key);
        }

        /// <summary>
        ///     Writes a value using an indexer step. If the step is <c>null</c>, uses the strictness of the mock
        ///     to decide whether to throw a <see cref="MockMissingException" /> (VeryStrict) or to do nothing
        ///     (Lenient or Strict).
        /// </summary>
        /// <typeparam name="TKey">The type of the indexer key.</typeparam>
        /// <typeparam name="TValue">The type of the indexer value.</typeparam>
        /// <param name="indexerStep">The indexer step (can be null) through which the value is read.</param>
        /// <param name="mockInfo">Information about the mock through which the value is read.</param>
        /// <param name="key">The indexer key used.</param>
        /// <param name="value">The value being written.</param>
        public static void SetWithStrictnessCheckIfNull<TKey, TValue>(this IIndexerStep<TKey, TValue> indexerStep, IMockInfo mockInfo, TKey key,
            TValue value)
        {
            if (indexerStep == null)
            {
                if (mockInfo.Strictness != Strictness.VeryStrict)
                {
                    return;
                }

                throw new MockMissingException(MockType.IndexerSet, mockInfo);
            }

            indexerStep.Set(mockInfo, key, value);
        }
    }
}
