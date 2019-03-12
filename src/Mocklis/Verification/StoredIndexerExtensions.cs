// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StoredIndexerExtensions.cs">
//   SPDX-License-Identifier: MIT
//   Copyright © 2019 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.Verification
{
    #region Using Directives

    using System.Collections.Generic;
    using Mocklis.Verification.Checks;

    #endregion

    /// <summary>
    ///     A class with extension methods for adding checks to an existing indexer store.
    /// </summary>
    public static class StoredIndexerExtensions6
    {
        /// <summary>
        ///     Checks the current values in the indexer store. Adds the check to the verification group provided.
        /// </summary>
        /// <typeparam name="TKey">The type of the indexer key.</typeparam>
        /// <typeparam name="TValue">The type of the indexer value.</typeparam>
        /// <param name="indexer">The <see cref="IStoredIndexer{TKey,TValue}" /> whose values to check.</param>
        /// <param name="collector">The verification group to which this check is added.</param>
        /// <param name="name">A name that can be used to identify the check in its verification group.</param>
        /// <param name="expectedValues">
        ///     A list of key-value pairs to check. The check will retrieve the value for each key
        ///     in the list and compare it to the value in the list.
        /// </param>
        /// <param name="comparer">Optional parameter with a comparer used to verify that the values are equal.</param>
        /// <returns>The <see cref="IStoredIndexer{TKey,TValue}" /> instance that can be used to add further checks.</returns>
        public static IStoredIndexer<TKey, TValue> CurrentValuesCheck<TKey, TValue>(this IStoredIndexer<TKey, TValue> indexer,
            VerificationGroup collector,
            string name, IEnumerable<KeyValuePair<TKey, TValue>> expectedValues, IEqualityComparer<TValue> comparer = null)
        {
            collector.Add(new CurrentValuesIndexerCheck<TKey, TValue>(indexer, name, expectedValues, comparer));
            return indexer;
        }
    }
}
