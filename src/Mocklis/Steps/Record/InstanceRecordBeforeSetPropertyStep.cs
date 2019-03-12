// --------------------------------------------------------------------------------------------------------------------
// <copyright file="InstanceRecordBeforeSetPropertyStep.cs">
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
    ///     This class represents a 'Record' step that records when a value is about to be written to a property.
    ///     Implements the <see cref="RecordPropertyStepBase{TValue, TRecord}" />
    /// </summary>
    /// <typeparam name="TValue">The type of the property.</typeparam>
    /// <typeparam name="TRecord">The type of data recorded in the ledger.</typeparam>
    /// <seealso cref="RecordPropertyStepBase{TValue, TRecord}" />
    public class InstanceRecordBeforeSetPropertyStep<TValue, TRecord> : RecordPropertyStepBase<TValue, TRecord>
    {
        private readonly Func<object, TValue, TRecord> _selection;

        /// <summary>
        ///     Initializes a new instance of the <see cref="InstanceRecordBeforeSetPropertyStep{TValue, TRecord}" /> class.
        /// </summary>
        /// <param name="selection">
        ///     A Func that selects what we want to record. Takes the entire state of the mock and the value about to be written as
        ///     parameters.
        /// </param>
        public InstanceRecordBeforeSetPropertyStep(Func<object, TValue, TRecord> selection)
        {
            _selection = selection ?? throw new ArgumentNullException(nameof(selection));
        }

        /// <summary>
        ///     Called when a value is written to the property.
        ///     This implementation records the value about to be written in the ledger.
        /// </summary>
        /// <param name="mockInfo">Information about the mock through which the value is written.</param>
        /// <param name="value">The value being written.</param>
        public override void Set(IMockInfo mockInfo, TValue value)
        {
            Add(_selection(mockInfo.MockInstance, value));
            base.Set(mockInfo, value);
        }
    }
}
