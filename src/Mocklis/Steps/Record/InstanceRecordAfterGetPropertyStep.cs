// --------------------------------------------------------------------------------------------------------------------
// <copyright file="InstanceRecordAfterGetPropertyStep.cs">
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
    ///     This class represents a 'Record' step that records when a value has been read from an property.
    ///     Inherits from the <see cref="RecordPropertyStepBase{TValue, TRecord}" /> class.
    /// </summary>
    /// <typeparam name="TValue">The type of the indexer value.</typeparam>
    /// <typeparam name="TRecord">The type of data recorded in the ledger.</typeparam>
    /// <seealso cref="RecordPropertyStepBase{TValue, TRecord}" />
    public class InstanceRecordAfterGetPropertyStep<TValue, TRecord> : RecordPropertyStepBase<TValue, TRecord>
    {
        private readonly Func<object, TValue, TRecord> _successSelector;
        private readonly Func<object, Exception, TRecord> _failureSelector;

        /// <summary>
        ///     Initializes a new instance of the <see cref="InstanceRecordAfterGetPropertyStep{TValue, TRecord}" /> class.
        /// </summary>
        /// <param name="successSelector">
        ///     A Func that constructs an entry for when a value has been read.
        ///     Takes the mocked instance and the value as parameters.
        /// </param>
        /// <param name="failureSelector">
        ///     An Func that constructs an entry for an exception thrown when reading a value.
        ///     Takes the mocked instance and the exception as parameters.
        /// </param>
        public InstanceRecordAfterGetPropertyStep(Func<object, TValue, TRecord> successSelector,
            Func<object, Exception, TRecord> failureSelector = null)
        {
            if (successSelector == null && failureSelector == null)
            {
                throw new ArgumentException(@"The successSelector is mandatory if the FailureSelector is null.", nameof(successSelector));
            }

            _successSelector = successSelector;
            _failureSelector = failureSelector;
        }

        /// <summary>
        ///     Called when a value is read from the property.
        ///     This implementation records the result of the read (be it value or exception) in the ledger once the read has been
        ///     done.
        /// </summary>
        /// <remarks>Exceptions are only recorded if the step was given an 'onError' Func.</remarks>
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
                    Add(_failureSelector(mockInfo.MockInstance, exception));
                }

                throw;
            }

            if (_successSelector != null)
            {
                Add(_successSelector(mockInfo.MockInstance, value));
            }

            return value;
        }
    }
}
