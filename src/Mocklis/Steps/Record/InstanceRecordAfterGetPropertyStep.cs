// --------------------------------------------------------------------------------------------------------------------
// <copyright file="InstanceRecordAfterGetPropertyStep.cs">
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
        private readonly Func<object, TValue, TRecord> _selection;
        private readonly Func<object, Exception, TRecord> _onError;

        /// <summary>
        ///     Initializes a new instance of the <see cref="InstanceRecordAfterGetPropertyStep{TValue, TRecord}" /> class.
        /// </summary>
        /// <param name="selection">
        ///     A Func that selects what we want to record. Takes the entire state of the mock and the value returned as
        ///     parameters.
        /// </param>
        /// <param name="onError">
        ///     An optional Func that selects what we want to record if the call threw an exception. Takes the entire state of the
        ///     mock
        ///     and the exception as parameters.
        /// </param>
        public InstanceRecordAfterGetPropertyStep(Func<object, TValue, TRecord> selection, Func<object, Exception, TRecord> onError = null)
        {
            _selection = selection ?? throw new ArgumentNullException(nameof(selection));
            _onError = onError;
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
                if (_onError != null)
                {
                    Add(_onError(mockInfo.MockInstance, exception));
                }

                throw;
            }

            Add(_selection(mockInfo.MockInstance, value));
            return value;
        }
    }
}
