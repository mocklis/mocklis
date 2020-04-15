// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RecordIndexerStepBase.cs">
//   SPDX-License-Identifier: MIT
//   Copyright © 2019-2020 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.Steps.Record
{
    #region Using Directives

    using System.Collections;
    using System.Collections.Generic;
    using Mocklis.Core;

    #endregion

    /// <summary>
    ///     Base class for steps that track reading from and writing to a mocked indexer. It also acts as a ledger of recorded
    ///     information.
    ///     Inherits from the <see cref="IndexerStepWithNext{TKey,TValue}" /> class
    ///     Implements the <see cref="IReadOnlyList{T}" /> interface.
    /// </summary>
    /// <typeparam name="TKey">The type of the indexer key.</typeparam>
    /// <typeparam name="TValue">The type of the indexer value.</typeparam>
    /// <typeparam name="TRecord">The type of data recorded in the ledger.</typeparam>
    /// <seealso cref="IndexerStepWithNext{TKey, TValue}" />
    /// <seealso cref="IReadOnlyList{TRecord}" />
    public abstract class RecordIndexerStepBase<TKey, TValue, TRecord> : IndexerStepWithNext<TKey, TValue>, IReadOnlyList<TRecord>
    {
        private readonly object _lockObject = new object();
        private readonly List<TRecord> _ledger = new List<TRecord>();

        /// <summary>
        ///     Adds the specified record to the ledger.
        /// </summary>
        protected void Add(TRecord record)
        {
            lock (_lockObject)
            {
                _ledger.Add(record);
            }
        }

        /// <summary>
        ///     Returns an enumerator that iterates through the ledger.
        /// </summary>
        public IEnumerator<TRecord> GetEnumerator() => _ledger.GetEnumerator();

        /// <summary>
        ///     Returns an enumerator that iterates through the ledger.
        /// </summary>
        IEnumerator IEnumerable.GetEnumerator() => _ledger.GetEnumerator();

        /// <summary>
        ///     Gets the number of elements in the ledger.
        /// </summary>
        public int Count => _ledger.Count;

        /// <summary>
        ///     Gets the <typeparamref name="TRecord" /> at the specified index in the ledger.
        /// </summary>
        /// <param name="index">The index of the record we want to retrieve.</param>
        /// <returns>The record at the specified index.</returns>
        public TRecord this[int index] => _ledger[index];
    }
}
