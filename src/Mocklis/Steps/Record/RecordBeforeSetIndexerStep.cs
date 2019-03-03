// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RecordBeforeSetIndexerStep.cs">
//   Copyright © 2019 Esbjörn Redmo and contributors. All rights reserved.
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
        private readonly Func<TKey, TValue, TRecord> _selection;

        /// <summary>
        ///     Initializes a new instance of the <see cref="RecordBeforeSetIndexerStep{TKey, TValue, TRecord}" /> class.
        /// </summary>
        /// <param name="selection">
        ///     A Func that selects what we want to record. Takes the key used and the value about to be written as parameters.
        /// </param>
        public RecordBeforeSetIndexerStep(Func<TKey, TValue, TRecord> selection)
        {
            _selection = selection ?? throw new ArgumentNullException(nameof(selection));
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
            Add(_selection(key, value));
            base.Set(mockInfo, key, value);
        }
    }
}
