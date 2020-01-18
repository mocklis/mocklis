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
        private readonly Func<TKey, TValue, TRecord>? _successSelector;
        private readonly Func<TKey, Exception, TRecord>? _failureSelector;

        /// <summary>
        ///     Initializes a new instance of the <see cref="RecordAfterGetIndexerStep{TKey, TValue, TRecord}" /> class.
        /// </summary>
        /// <param name="successSelector">
        ///     A Func that constructs an entry for when a value has been read.
        ///     Takes the key used and the value as parameters.
        /// </param>
        /// <param name="failureSelector">
        ///     An Func that constructs an entry for an exception thrown when reading a value.
        ///     Takes the key used and the exception as parameters.
        /// </param>
        public RecordAfterGetIndexerStep(Func<TKey, TValue, TRecord>? successSelector, Func<TKey, Exception, TRecord>? failureSelector = null)
        {
            if (successSelector == null && failureSelector == null)
            {
                throw new ArgumentException(@"The successSelector is mandatory if the FailureSelector is null or missing.", nameof(successSelector));
            }

            _successSelector = successSelector;
            _failureSelector = failureSelector;
        }

        /// <summary>
        ///     Called when a value is read from the indexer.
        ///     This implementation records the result of the read (be it value or exception) in the ledger once the read has been
        ///     done.
        /// </summary>
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
                if (_failureSelector != null)
                {
                    Add(_failureSelector(key, exception));
                }

                throw;
            }

            if (_successSelector != null)
            {
                Add(_successSelector(key, value));
            }

            return value;
        }
    }
}
