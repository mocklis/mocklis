// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RecordBeforeSetIndexerStep.cs">
//   SPDX-License-Identifier: MIT
//   Copyright © 2019-2021 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.Steps.Record
{
    #region Using Directives

    using System;
    using Mocklis.Core;

    #endregion

    /// <summary>
    ///     This class represents a 'Record' step that records when a value is about to be written to an indexer.
    ///     Implements the <see cref="RecordIndexerStepBase{TKey, TValue, TRecord}" />
    /// </summary>
    /// <typeparam name="TKey">The type of the indexer key.</typeparam>
    /// <typeparam name="TValue">The type of the indexer value.</typeparam>
    /// <typeparam name="TRecord">The type of data recorded in the ledger.</typeparam>
    /// <seealso cref="RecordIndexerStepBase{TKey, TValue, TRecord}" />
    public class RecordBeforeSetIndexerStep<TKey, TValue, TRecord> : RecordIndexerStepBase<TKey, TValue, TRecord>
    {
        private readonly Func<TKey, TValue, TRecord> _selector;

        /// <summary>
        ///     Initializes a new instance of the <see cref="RecordBeforeSetIndexerStep{TKey, TValue, TRecord}" /> class.
        /// </summary>
        /// <param name="selector">
        ///     A Func that constructs an entry for when a value is written.
        ///     Takes the key used and the value as parameters.
        /// </param>
        public RecordBeforeSetIndexerStep(Func<TKey, TValue, TRecord> selector)
        {
            _selector = selector ?? throw new ArgumentNullException(nameof(selector));
        }

        /// <summary>
        ///     Called when a value is written to the indexer.
        ///     This implementation records the key used and value about to be written in the ledger.
        /// </summary>
        /// <param name="mockInfo">Information about the mock through which the value is written.</param>
        /// <param name="key">The indexer key used.</param>
        /// <param name="value">The value being written.</param>
        public override void Set(IMockInfo mockInfo, TKey key, TValue value)
        {
            Add(_selector(key, value));
            base.Set(mockInfo, key, value);
        }
    }
}
