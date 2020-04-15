// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RecordAfterGetPropertyStep.cs">
//   SPDX-License-Identifier: MIT
//   Copyright © 2019-2020 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.Steps.Record
{
    #region Using Directives

    using System;
    using Mocklis.Core;

    #endregion

    /// <summary>
    ///     This class represents a 'Record' step that records when a value has been read from an property.
    ///     Inherits from the <see cref="RecordPropertyStepBase{TValue, TRecord}" /> class.
    /// </summary>
    /// <typeparam name="TValue">The type of the indexer value.</typeparam>
    /// <typeparam name="TRecord">The type of data recorded in the ledger.</typeparam>
    /// <seealso cref="RecordPropertyStepBase{TValue, TRecord}" />
    public class RecordAfterGetPropertyStep<TValue, TRecord> : RecordPropertyStepBase<TValue, TRecord>
    {
        private readonly Func<TValue, TRecord>? _successSelector;
        private readonly Func<Exception, TRecord>? _failureSelector;

        /// <summary>
        ///     Initializes a new instance of the <see cref="RecordAfterGetPropertyStep{TValue, TRecord}" /> class.
        /// </summary>
        /// <param name="successSelector">
        ///     A Func that constructs an entry for when a value has been read.
        ///     Takes the value as parameter.
        /// </param>
        /// <param name="failureSelector">
        ///     An Func that constructs an entry for an exception thrown when reading a value.
        ///     Takes the exception as parameter.
        /// </param>
        public RecordAfterGetPropertyStep(Func<TValue, TRecord>? successSelector, Func<Exception, TRecord>? failureSelector = null)
        {
            if (successSelector == null && failureSelector == null)
            {
                throw new ArgumentException(@"The successSelector is mandatory if the FailureSelector is null or missing.", nameof(successSelector));
            }

            _successSelector = successSelector;
            _failureSelector = failureSelector;
        }

        /// <summary>
        ///     Called when a value is read from the property.
        ///     This implementation records the result of the read (be it value or exception) in the ledger once the read has been
        ///     done.
        /// </summary>
        /// <param name="mockInfo">Information about the mock through which the value is read.</param>
        /// <returns>The value being read.</returns>
        public override TValue Get(IMockInfo mockInfo)
        {
            TValue value;
            try
            {
                value = base.Get(mockInfo);
            }
            catch (Exception exception)
            {
                if (_failureSelector != null)
                {
                    Add(_failureSelector(exception));
                }

                throw;
            }

            if (_successSelector != null)
            {
                Add(_successSelector(value));
            }

            return value;
        }
    }
}
