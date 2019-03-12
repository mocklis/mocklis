// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RecordAfterGetIndexerStep.cs">
//   SPDX-License-Identifier: MIT
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
    ///     This class represents a 'Record' step that records when a value has been read from an indexer.
    ///     Inherits from the <see cref="RecordIndexerStepBase{TKey, TValue, TRecord}" /> class.
    /// </summary>
    /// <typeparam name="TKey">The type of the indexer key.</typeparam>
    /// <typeparam name="TValue">The type of the indexer value.</typeparam>
    /// <typeparam name="TRecord">The type of data recorded in the ledger.</typeparam>
    /// <seealso cref="RecordIndexerStepBase{TKey, TValue, TRecord}" />
    public class RecordAfterGetIndexerStep<TKey, TValue, TRecord> : RecordIndexerStepBase<TKey, TValue, TRecord>
    {
        private readonly Func<TKey, TValue, TRecord> _selection;
        private readonly Func<Exception, TRecord> _onError;

        /// <summary>
        ///     Initializes a new instance of the <see cref="RecordAfterGetIndexerStep{TKey, TValue, TRecord}" /> class.
        /// </summary>
        /// <param name="selection">
        ///     A Func that selects what we want to record. Takes the key used and the value returned as parameters.
        /// </param>
        /// <param name="onError">
        ///     An optional Func that selects what we want to record if the call threw an exception. Takes the exception as
        ///     parameter.
        /// </param>
        public RecordAfterGetIndexerStep(Func<TKey, TValue, TRecord> selection, Func<Exception, TRecord> onError = null)
        {
            _selection = selection ?? throw new ArgumentNullException(nameof(selection));
            _onError = onError;
        }

        /// <summary>
        ///     Called when a value is read from the indexer.
        ///     This implementation records the result of the read (be it value or exception) in the ledger once the read has been
        ///     done.
        /// </summary>
        /// <remarks>Exceptions are only recorded if the step was given an 'onError' Func.</remarks>
        /// <param name="mockInfo">Information about the mock through which the value is read.</param>
        /// <param name="key">The indexer key used.</param>
        /// <returns>The value being read.</returns>
        public override TValue Get(IMockInfo mockInfo, TKey key)
        {
            TValue value;
            try
            {
                value = base.Get(mockInfo, key);
            }
            catch (Exception exception)
            {
                if (_onError != null)
                {
                    Add(_onError(exception));
                }

                throw;
            }

            Add(_selection(key, value));
            return value;
        }
    }
}
